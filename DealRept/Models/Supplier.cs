using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DealRept.Models
{
    public class Supplier
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Company (IE) Name")]
        [StringLength(200, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 2)]
        public string Name { get; set; }


        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Street and Building")]
        [StringLength(100, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 2)]
        public string StreetBuilding { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Column(TypeName = "char(5)")]
        [RegularExpression(@"^[0-9]{5}$", ErrorMessage = "Allowed: digits only, lehgth: 5 characters.")]
        [Display(Name = "Postal Index")]
        [StringLength(5, ErrorMessage = "Allowed length: {1} characters.", MinimumLength = 5)]
        public string PostalIndex { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Legal Code")]
        [RegularExpression(@"^[0-9]{8}$|[0-9]{10}$", ErrorMessage = "Allowed: digits only, lehgth: 8, 10 characters.")]
        [StringLength(10, ErrorMessage = "Allowed length: {2}, {1} characters.", MinimumLength = 8)]
        public string LegalCode { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Column(TypeName = "char(29)")]
        [Display(Name = "Bank Account")]
        [RegularExpression(@"^[a-zA-Z]{2}[\d]{27}$", ErrorMessage = "Allowed format: UAXXXXXXXXXXXXXXXXXXXXXXXXX, length: 29 characters.")]
        [StringLength(29, ErrorMessage = "Allowed length: {1} characters.", MinimumLength = 29)]
        public string BankAccount { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Telephone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+38)[\d]{10}$", ErrorMessage = "Allowed format: +38XXXXXXXXXX, length: 13.")]
        [StringLength(13
            , ErrorMessage = "Allowed length: {1} characters.", MinimumLength = 13)]
        public string CompanyTelephone { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 3)]
        public string CompanyEmail { get; set; }

        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 2)]
        public string ContactFirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 2)]
        public string ContactLastName { get; set; }

        [Display(Name = "Middle Name")]
        [StringLength(50, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 2)]
        public string ContactMiddleName { get; set; }

        [Display(Name = "Telephone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+38)[\d]{10}$", ErrorMessage = "Allowed format: +38XXXXXXXXXX, length: 13.")]
        [StringLength(13, ErrorMessage = "Allowed length: {1} characters.", MinimumLength = 13)]
        public string ContactTelephone { get; set; }

        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 3)]
        public string ContactEmail { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 2)]
        public string DirectorFirstName { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 2)]
        public string DirectorLastName { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Middle Name")]
        [StringLength(50, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 2)]
        public string DirectorMiddleName { get; set; }

        [Display(Name = "Negative Remarks")]
        [Column(TypeName = "text")]
        public string NegativeRemarks { get; set; }

        [NotMapped]
        [Display(Name = "Current Contracts")]
        public int CurrentContractCount { get; set; }

        [NotMapped]
        [Display(Name = "Past Contracts")]
        public int PastContractCount { get; set; }

        [Display(Name = "Legal Address")]
        public string LegalAddress
        {
            get { return $"{PostalIndex}, {City?.Name}, {StreetBuilding}"; }
        }

        //?
        public string BankingDetails 
        {
            get { return $"{BankAccount} in {Bank?.Name}, bank code {Bank?.Code}"; }
        }

        [Display(Name = "Full Name")]
        public string ContactFullName
        {
            get { return $"{ContactLastName} {ContactFirstName} {ContactMiddleName}"; }
        }

        [Display(Name = "Director")]
        public string DirectorFullName
        {
            get { return $"{DirectorLastName} {DirectorFirstName} {DirectorMiddleName}"; }
        }

        /*foreighn keys*/
        [Display(Name = "City")]
        [Required(ErrorMessage = "Please enter a value for {0}.")]
        public int CityID { get; set; }

        [Display(Name = "Bank")]
        [Required(ErrorMessage = "Please enter a value for {0}.")]
        public int BankID { get; set; }

        [Display(Name="Specialization")]
        [Required(ErrorMessage = "Please enter a value for {0}.")]
        public int SpecializationID { get; set; }

        /*Navigation properties*/
        public City City { get; set; }
        public Bank Bank { get; set; }
        public Specialization Specialization { get; set; }
        public ICollection<CurrentContract> CurrentContracts { get; set; }
        public ICollection<PastContract> PastContracts { get; set; }
    }
}
