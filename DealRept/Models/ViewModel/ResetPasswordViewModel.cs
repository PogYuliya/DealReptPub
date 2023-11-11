using System.ComponentModel.DataAnnotations;

namespace DealRept.Models.ViewModel
{
    public class ResetPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name ="Confirmation Password")]
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }
        public string Token { get; set; }  

    }
}
