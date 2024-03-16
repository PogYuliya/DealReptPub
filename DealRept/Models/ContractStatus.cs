using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DealRept.Models
{
    public class ContractStatus
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Contract Status")]
        [StringLength(20, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 2)]
        public string Name { get; set; }

        /*Navigation properties*/

        public ICollection<CurrentContract> CurrentContracts { get; set; }
        public ICollection<PastContract> PastContracts { get; set; }
    }
}
