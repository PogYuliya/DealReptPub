using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DealRept.Models.ViewModel
{
    public class MessageToAdminViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Display(Name = "Attachment")]
        public IFormFile Attachment { get; set; }

        [Required]
        [Display(Name = "From")]
        public string UserEmail { get; set; }
    }
}
