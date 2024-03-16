using System.ComponentModel.DataAnnotations;

namespace DealRept.Models.ViewModel
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [EmailAddress]
        [Display(Name ="Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [DataType(DataType.Password)]
        [Display(Name ="Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        = false;

        public string ReturnUrl { get; set; }
    }
}
