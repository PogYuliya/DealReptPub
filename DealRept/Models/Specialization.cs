using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DealRept.Models
{
    public class Specialization
    {
        
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Specialization")]
        [StringLength(255, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 2)]
        public string Name { get; set; }


        /*Navigation property*/
        public ICollection<Supplier> Suppliers { get; set; }
    }
}
