﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace DealRept.Utilities
{
    public static class FileHelpers
    {
        private static readonly byte[] _allowedChars = { };
        
        private static readonly Dictionary<string, List<byte[]>> _fileSignature = new Dictionary<string, List<byte[]>>
        {
            {".pdf",new List<byte[]>
                {
                new byte[]{ 0x25, 0x50, 0x44, 0x46, 0x2D }
                }
            },
            {".xls",new List<byte[]>
                {
                new byte[]{ 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 }
                }
            },
            {".xlsx",new List<byte[]>
                {
                new byte[]{ 0x50, 0x4B, 0x03, 0x04 }
                }
            },
        };

        // **WARNING!**
        // In the following file processing methods, the file's content isn't scanned.
        // In most production scenarios, an anti-virus/anti-malware scanner API is
        // used on the file before making the file available to users or other
        // systems. For more information, see the topic that accompanies this sample
        // app.

        public static async Task<byte[]> ProcessFormFile<T>(IFormFile formFile,
            ModelStateDictionary modelState, string[] permittedExtensions,
            long sizeLimit, ILogger logger)
        {
            var fieldDisplayName = string.Empty;

            // Use reflection to obtain the display name for the model
            // property associated with this IFormFile. If a display
            // name isn't found, error messages simply won't show
            // a display name.
            MemberInfo property =
                typeof(T).GetProperty(
                    formFile.Name.Substring(formFile.Name.IndexOf(".",
                    StringComparison.Ordinal) + 1));

            if (property != null)
            {
                if (property.GetCustomAttribute(typeof(DisplayAttribute)) is
                    DisplayAttribute displayAttribute)
                {
                    fieldDisplayName = $"{displayAttribute.Name} ";
                }
            }

            // Don't trust the file name sent by the client. To display
            // the file name, HTML-encode the value.
            var trustedFileNameForDisplay = WebUtility.HtmlEncode(
                formFile.FileName);

            // Check the file length. This check doesn't catch files that only have 
            // a BOM as their content.
            if (formFile.Length == 0)
            {
                modelState.AddModelError(formFile.Name,
                    $"{fieldDisplayName}({trustedFileNameForDisplay}) is empty.");

                return Array.Empty<byte>();
            }

            if (formFile.Length > sizeLimit)
            {
                var megabyteSizeLimit = sizeLimit / 1048576;
                modelState.AddModelError(formFile.Name,
                    $"{fieldDisplayName}({trustedFileNameForDisplay}) exceeds " +
                    $"{megabyteSizeLimit:N1} MB.");

                return Array.Empty<byte>();
            }

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await formFile.CopyToAsync(memoryStream);

                    // Check the content length in case the file's only
                    // content was a BOM and the content is actually
                    // empty after removing the BOM.
                    if (memoryStream.Length == 0)
                    {
                        modelState.AddModelError(formFile.Name,
                            $"{fieldDisplayName}({trustedFileNameForDisplay}) is empty.");
                    }

                    if (!IsValidFileExtensionAndSignature(
                        formFile.FileName, memoryStream, permittedExtensions))
                    {
                        modelState.AddModelError(formFile.Name,
                            $"{fieldDisplayName}({trustedFileNameForDisplay}) file " +
                            "type isn't permitted or the file's signature " +
                            "doesn't match the file's extension.");
                    }
                    else
                    {
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{FieldDisplayName}({TrustedFileNameForDisplay}) upload failed.  Error: {ErrorCode}", fieldDisplayName, trustedFileNameForDisplay, ex.HResult);
                modelState.AddModelError(formFile.Name,
                    $"{fieldDisplayName}({trustedFileNameForDisplay}) upload failed. " +
                    "Try again, and if the problem persists " +
               "see your system administrator.");
            }

            return Array.Empty<byte>();
        }

        private static bool IsValidFileExtensionAndSignature(string fileName, Stream data, string[] permittedExtensions)
        {
            if (string.IsNullOrEmpty(fileName) || data == null || data.Length == 0)
            {
                return false;
            }

            var ext = Path.GetExtension(fileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return false;
            }

            data.Position = 0;

            using (var reader = new BinaryReader(data))
            {
                if (ext.Equals(".txt") || ext.Equals(".csv") || ext.Equals(".prn"))
                {
                    if (_allowedChars.Length == 0)
                    {
                        // Limits characters to ASCII encoding.
                        for (var i = 0; i < data.Length; i++)
                        {
                            if (reader.ReadByte() > sbyte.MaxValue)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        // Limits characters to ASCII encoding and
                        // values of the _allowedChars array.
                        for (var i = 0; i < data.Length; i++)
                        {
                            var b = reader.ReadByte();
                            if (b > sbyte.MaxValue ||
                                !_allowedChars.Contains(b))
                            {
                                return false;
                            }
                        }
                    }

                    return true;
                }


                // File signature check
                // --------------------
                // With the file signatures provided in the _fileSignature
                // dictionary, the following code tests the input content's
                // file signature.
                var signatures = _fileSignature[ext];
                var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

                return signatures.Any(signature =>
                    headerBytes.Take(signature.Length).SequenceEqual(signature));
            }
        }
    }
}
