using System.ComponentModel.DataAnnotations;

namespace DealRept.Models.ViewModel
{
    public class BankViewModel
    {
        [Display(Name = "Bank")]
        public Bank Bank { get; set; }

        [Display(Name = "Important Information")]
        public string ErrorMessage { get; set; }
    }
}
