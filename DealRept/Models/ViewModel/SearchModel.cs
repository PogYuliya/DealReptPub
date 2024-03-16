using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DealRept.Models.ViewModel
{
    public class SearchModel
    {

        [Display(Name = "Legal Address")]
        public string LegalAddressSearch { get; set; }

        [Display(Name = "Postal Address")]
        public string PostalAddressSearch { get; set; }

        [Display(Name="Branch Name or Code")]
        public string NameBranchSearch { get; set; }

        [Display(Name = "Head`s Full Name")]
        public string NameHead { get; set; }

        [Display(Name ="Supplier Name or Legal Code")]
        public string NameCodeSupplierSearch { get; set; }

        [Display(Name = "Specialization")]
        public string SpecializationSearch { get; set; }

        public string NegativeRemarksSearch { get; set; }

        public string ContractType { get; set; }

        [Display(Name = "Contract Number")]
        public string ContractNumber { get; set; }

        //[RegularExpression(@"((\d+)((\,\d{1,2})?))$", ErrorMessage = "Allowed: interger or decimal with 2 decimal places and comma separator.")]
        [RegularExpression(@"((\d+)((\.\d{1,2})?))$", ErrorMessage = "Allowed: interger or decimal with 2 decimal places and dot separator.")]
        [StringLength(13, ErrorMessage = "Allowed length: {2}-{1}.", MinimumLength = 1)]
        public string ContractAmountFrom { get; set; }

        //[RegularExpression(@"((\d+)((\,\d{1,2})?))$", ErrorMessage = "Allowed: interger or decimal with 2 decimal places and comma separator.")]
        [RegularExpression(@"((\d+)((\.\d{1,2})?))$", ErrorMessage = "Allowed: interger or decimal with 2 decimal places and dot separator.")]
        [StringLength(13, ErrorMessage = "Allowed length: {2}-{1}.", MinimumLength = 1)]
        public string ContractAmountTo { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? ConclusionDateFrom { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? ConclusionDateTo { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ExpirationDateFrom { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ExpirationDateTo { get; set; }

        public string CourtDisputeSearch { get; set; }

        public int PastContractStatusID { get; set; }

        public List<ContractStatus> PastContractStatuses { get; set; }
        
        public string RoleID { get; set; }
        
        public IEnumerable<IdentityRole> Roles { get; set; }

        [Display(Name ="Full Name")]
        public string UserFullName { get; set; }

        [Display(Name ="Employee Number")]
        public int EmployeeNumber { get; set; }
        
        public string AccountApprovalSearch { get; set; }
        
        public DateTime? RegistrationDateFrom { get; set; }

        public DateTime? RegistrationDateTo { get; set; }

        [Display(Name = "City Name")]
        public string NameCitySearch { get; set; }

        [Display(Name = "Last Added")]
        public int LastAdded { get; set; }

        [Display(Name = "Bank Name or Code")]
        public string NameCodeBankSearch { get; set; }

    }
}
