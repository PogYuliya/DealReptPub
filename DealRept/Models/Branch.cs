using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealRept.Models
{
    public class Branch
    {
        public int ID { get; set; }

        [Required (ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name="Code")]
        [StringLength(5,ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength =2)]
        public string Code { get; set; }

        [Required (ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name ="Name")]
        [StringLength(200, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 2)]
        public string Name { get; set; }

        [Required (ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name ="Street and Building")]
        [StringLength(100, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 2)]
        public string StreetBuilding { get; set; }

        [Required (ErrorMessage = "Please enter a value for {0}.")]
        [Column(TypeName = "char(5)")]
        [RegularExpression(@"^[0-9]{5}$", ErrorMessage = "Allowed: digits only, lehgth: 5 characters.")]
        [Display(Name ="Postal Index")]
        [StringLength(5, ErrorMessage = "Allowed length: {1} characters.", MinimumLength = 5)]
        public string PostalIndex { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Telephone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+38)[\d]{10}$", ErrorMessage = "Allowed format: +38XXXXXXXXXX, length: 13.")]
        [StringLength(13, ErrorMessage = "Allowed length: {1} characters.", MinimumLength = 13)]
        public string BranchTelephone { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 3)]
        public string BranchEmail { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 2)]
        public string HeadFirstName { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 2)]
        public string HeadLastName { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Middle Name")]
        [StringLength(50, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 2)]
        public string HeadMiddleName { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Telephone Number")]
        [RegularExpression(@"^(\+38)[\d]{10}$", ErrorMessage = "Allowed format: +38XXXXXXXXXX, length: 13.")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(13, ErrorMessage = "Allowed length: {1} characters.", MinimumLength = 13)]
        public string HeadTelephone { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 3)]
        public string HeadEmail { get; set; }

        

        [Display(Name="Full Name")]
        public string HeadFullName
        {
            get { return $"{HeadLastName} {HeadFirstName} {HeadMiddleName}"; } 
        }

        [NotMapped]
        [Display(Name ="Current Contracts")]
        public int CurrentContractCount { get; set; }

        [NotMapped]
        [Display(Name = "Past Contracts")]
        public int PastContractCount { get; set; }


        [Display(Name ="Postal Address")]
        public string PostalAddress 
        {
            get {return $"{PostalIndex}, {City?.Name}, {StreetBuilding}"; } 
        }

        /*Foreighn keys*/
        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "City")]
        public int CityID { get; set; }

        /*Navigation properties*/
        public City City { get; set; }
        public ICollection<CurrentContract> CurrentContracts { get; set; }
        public ICollection<PastContract> PastContracts { get; set; }
    }
}
