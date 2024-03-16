using System;
using System.ComponentModel.DataAnnotations;

namespace DealRept.Models.ViewModel
{
    public class UserRegistrationViewModel
    {
        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [StringLength(50, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 2)]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 2)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name ="Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Confirmation Password")]
        [Compare("Password",ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Employee Number")]
        [Range(1, (double)int.MaxValue, ErrorMessage = "{0} should be not 0 and positive.")]
        public int EmployeeNumber { get; set; }


    }
}
