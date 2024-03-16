using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealRept.Models
{
    public class Bank
    {
        public int ID { get; set; }

        [Required (ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Name")]
        [StringLength(200, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 2)]
        [RegularExpression("^[a-zA-Z][a-zA-Z\\s-\\.\",]+[a-zA-Z\"]$", ErrorMessage = "Allowed characters: letters, can't start or end with whitespace or hyphen.")]
        public string Name { get; set; }

        [Required (ErrorMessage = "Please enter a value for {0}.")]
        [Column(TypeName = "char(6)")]
        [Display(Name = "Code")]
        [StringLength(6, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 2)]
        [RegularExpression("[0-9]{6}", ErrorMessage = "Allowed characters: numbers, length: 6.")]
        public string Code { get; set; }

        [NotMapped]
        [Display(Name = "Suppliers")]
        public int SuppliersCount { get; set; }

        /*Navigation property*/
        public ICollection<Supplier> Suppliers { get; set; }

    }
}
