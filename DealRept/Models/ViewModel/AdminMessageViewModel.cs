using Microsoft.AspNetCore.Http;
using MimeKit;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DealRept.Models.ViewModel
{
    public class AdminMessageViewModel
    {
        public List<MailboxAddress> To { get; set; }
        public List<RoleWithSendTo> Roles { get; set; }

        [Display(Name = "Employee Numbers")]
        public string EmployeeNumbersTo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a value for {0}.")]
        [Display (Name ="Subject")]
        public string Subject { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a value for {0}.")]
        [Display (Name ="Content")]
        public string Content { get; set; }

        [Display(Name = "Attachment")]
        public IFormFile Attachment { get; set; }
    }
}
