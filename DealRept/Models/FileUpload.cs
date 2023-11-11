using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DealRept.Models.ViewModel
{
    public class FileUpload
    {
        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "File")]
        public IFormFile FormFile { get; set; }
    }
}
