using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DealRept.Models.ViewModel
{
    public class BranchViewModel
    {
        public Branch Branch { get; set; }

        [Display(Name ="Important Information")]
        public string ErrorMessage { get; set; }

        public SelectList Cities { get; set; }
    }
}
