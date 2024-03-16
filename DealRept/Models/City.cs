using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealRept.Models
{
    public class City
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Column(TypeName = "varchar(300)")]
        [StringLength(300, ErrorMessage = "Allowed length: {2}-{1} characters", MinimumLength = 2)]
        [RegularExpression("^[a-zA-Z][a-zA-Z\\s-,]+[a-zA-Z]$", ErrorMessage = "Allowed characters: letters, can't start or end with whitespace or hyphen.")]
        public string Name { get; set; }

        [NotMapped]
        [Display(Name = "Branches")]
        public int BranchesCount { get; set; }

        [NotMapped]
        [Display(Name = "Suppliers")]
        public int SuppliersCount { get; set; }

        /*Navigation property*/
        public ICollection<Branch> Branches { get; set; }
        public ICollection<Supplier> Suppliers { get; set; }

    }
}
