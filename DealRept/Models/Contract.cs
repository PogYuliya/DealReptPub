using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealRept.Models
{
    public class Contract: IValidatableObject
    {
        public int ID { get; set; }

        [Required (ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Contract Number")]
        [StringLength(20, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 2)]
        public string ContractNumber { get; set; }

        [DateGreaterTodayAttributes]
        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Conclusion Date")]
        [Column(TypeName = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime ConclusionDate { get; set; }
        = DateTime.Now.Date;

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Expiration Date")]
        [Column(TypeName = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime ExpirationDate { get; set; }
       = DateTime.Now.Date;

        [Display(Name = "Transition Date")]
        [Column(TypeName = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public virtual DateTime TransitionDate { get; set; }


        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Column(TypeName = "Decimal(10,2)")]
        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        [Display(Name = "Amount")]
        [MinMaxDecimalAttributes(0,1000000000)]
        public decimal ContractAmount { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Column(TypeName = "Text")]
        [Display(Name = "Subject")]
        [DataType(DataType.Text)]
        [StringLength(int.MaxValue, ErrorMessage = "Allowed minimum length: {2} characters.", MinimumLength = 2)]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Prolonged")]
        public bool IsProlonged { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Is Court Dispute?")]
        public bool IsCourtDispute { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Type of Subject")]
        public bool IsGoods { get; set; }
        = true;

        [Column(TypeName = "text")]
        public string Remarks { get; set; }


        /*foreighn keys*/
        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Supplier")]
        public int SupplierID { get; set; }


        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Branch")]
        public int BranchID { get; set; }

        [Display(Name = "Contract Status")]
        public int ContractStatusID { get; set; }
        
     

        /*Navigation properties*/

        [Display(Name ="Supplier")]
        public Supplier Supplier { get; set; }
        [Display(Name ="Branch")]
        public Branch Branch { get; set; }
        public ContractStatus ContractStatus {get; set; }


        public virtual ICollection<CurrentDocument> CurrentDocuments { get; set; }
        public virtual ICollection<PastDocument> PastDocuments { get; set; }
        public virtual ICollection<CurrentEvent> CurrentEvents { get; set; }
        public virtual ICollection<PastEvent> PastEvents { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ConclusionDate.Date >= ExpirationDate.Date)
            {
                yield return new ValidationResult(
                    "Contract Date of Conclusion must be before Expiration date.",
                    new[] { nameof(ExpirationDate) });
            }
           
        }
    }
}
