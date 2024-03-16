using DealRept.Models;
using DealRept.Models.ViewModel;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;

namespace DealRept.Controllers
{
    public class ErrorsController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            string requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            string referrer = HttpContext.Request.Headers["referer"];

            string exceptionMessage;

            var exceptionHandlerPathFeature =
                HttpContext.Features.Get<IExceptionHandlerPathFeature>();


            if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
            {
                exceptionMessage = "The file or directory cannot be found, error thrown";
            }
            else if (exceptionHandlerPathFeature?.Error is DirectoryNotFoundException)
            {
                exceptionMessage = "The file or directory cannot be found, error thrown";
            }
            else if (exceptionHandlerPathFeature?.Error is DriveNotFoundException)
            {
                exceptionMessage = "The drive specified in 'path' is invalid, error thrown";
            }
            else if (exceptionHandlerPathFeature?.Error is PathTooLongException)
            {
                exceptionMessage = "The 'path' exceeds the maxium supported path length, error thrown";
            }
            else if (exceptionHandlerPathFeature?.Error is UnauthorizedAccessException)
            {
                exceptionMessage = "You do not have permission to create this file, error thrown";
            }
            else if (exceptionHandlerPathFeature?.Error is IOException &&
                (exceptionHandlerPathFeature?.Error.HResult & 0x0000FFFF) == 32)
            {
                exceptionMessage = "A sharing violation error thrown";
            }
            else if (exceptionHandlerPathFeature?.Error is IOException &&
                (exceptionHandlerPathFeature?.Error.HResult & 0x0000FFFF) == 80)
            {
                exceptionMessage = "The file already exists error thrown";
            }
            else if (exceptionHandlerPathFeature?.Error is ElementNotFoundException ENFex)
            {
                exceptionMessage = ENFex.Message + " error thrown";
            }
            else if (exceptionHandlerPathFeature?.Error is ProccesingException Prex)
            {
                exceptionMessage = Prex.Message + " error thrown";
            }
            else
            {
                exceptionMessage = "Error thrown";
            }

            if ((bool)exceptionHandlerPathFeature?.Path.Contains("/Contracts"))
            {
                exceptionMessage += " from Contracts page.";
            }
            else if ((bool)exceptionHandlerPathFeature?.Path.Contains("/Account"))
            {
                exceptionMessage += " from Account page.";
            }
            else if ((bool)exceptionHandlerPathFeature?.Path.Contains("/Suppliers"))
            {
                exceptionMessage += " from Suppliers page.";
            }
            else if ((bool)exceptionHandlerPathFeature?.Path.Contains("/Branches"))
            {
                exceptionMessage += " from Branches page.";
            }
            else if ((bool)exceptionHandlerPathFeature?.Path.Contains("/Home"))
            {
                exceptionMessage += " from Home page.";
            }
            else if ((bool)exceptionHandlerPathFeature?.Path.Contains("/Users"))
            {
                exceptionMessage += " from Users page.";
            }
            else if ((bool)exceptionHandlerPathFeature?.Path.Contains("/Banks"))
            {
                exceptionMessage += " from Banks page.";
            }
            else if ((bool)exceptionHandlerPathFeature?.Path.Contains("/Cities"))
            {
                exceptionMessage += " from Cities page.";
            }
            else
            {
                exceptionMessage += ".";
            }
            if (!string.IsNullOrEmpty(exceptionMessage))
            {
                exceptionMessage += " Try again, and if the problem persists " +
                "see your system administrator.";
            }
            return View(nameof(Error), new ErrorViewModel { RequestId = requestId, Referrer = referrer, ExceptionMessage = exceptionMessage });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult StatusCode(string statusCode)
        {
            string requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            string errorStatusCode = statusCode;
            string originalURL = string.Empty;
            string exceptionMessage = "Try again, and if the problem persists " +
                "see your system administrator.";

            var statusCodeReExecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            if (statusCodeReExecuteFeature != null)
            {
                originalURL =
                    statusCodeReExecuteFeature.OriginalPathBase
                    + statusCodeReExecuteFeature.OriginalPath
                    + statusCodeReExecuteFeature.OriginalQueryString;
            }

            return View(new StatusCodeViewModel
            {
                ErrorStatusCode = errorStatusCode,
                RequestId = requestId,
                OriginalURL = originalURL,
                ErrorMessage = exceptionMessage
            });
        }
    }
}
