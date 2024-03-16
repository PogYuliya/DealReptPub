using System.ComponentModel.DataAnnotations;

namespace DealRept.Models.ViewModel
{
    public class BankViewModel
    {
        [Display(Name = "Bank")]
        public string Name { get; set; }

        [Display(Name = "Code")]
        public string Code { get; set; }
    }
}
