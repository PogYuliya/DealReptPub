using System;
using System.ComponentModel.DataAnnotations;

namespace DealRept.Models.ViewModel
{
    public class BriefContractInfoVM
    {
        [Display(Name = "Contract Number")]
        public string ContractNumber { get; set; }

        [Display(Name = "Conclusion Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime ConclusionDate { get; set; }

        [Display(Name = "Expiration Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime ExpirationDate { get; set; }

        [Display(Name = "Transition Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public virtual DateTime TransitionDate { get; set; }


        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        [Display(Name = "Amount")]
        public decimal ContractAmount { get; set; }

        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Display(Name = "Prolonged")]
        public bool IsProlonged { get; set; }

        [Display(Name = "Is Court Dispute?")]
        public bool IsCourtDispute { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Type of Subject")]
        public bool IsGoods { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Supplier")]
        public string SupplierName { get; set; }

        [Display(Name = "Branch")]
        public string BranchName { get; set; }

        [Display(Name = "Contract Status")]
        public string ContractStatus { get; set; }

    }
}
