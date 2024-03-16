using DealRept.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealRept.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "Allowed length: {2} - {1} characters.", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [StringLength(50, ErrorMessage = "Allowed length: {2} - {1} characters.", MinimumLength = 2)]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Allowed length: {2} - {1} characters.", MinimumLength = 2)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Employee Number")]
        [Range(1, (double)int.MaxValue, ErrorMessage = "{0} should be not 0 and positive.")]
        public int EmployeeNumber { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Is Approved?")]
        public bool IsApproved { get; set; }
        = false;

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
        [Display(Name = "Registration Date (UTC)")]
        public DateTime RegistrationDateUTC { get; set; }

        [Display(Name = "Full Name")]
        public string UserFullName
        {
            get { return $"{LastName} {FirstName} {MiddleName}"; }
        }

        /*Navigation Property*/
        [NotMapped]
        public IEnumerable<string> Roles { get; set; }
    }
}
