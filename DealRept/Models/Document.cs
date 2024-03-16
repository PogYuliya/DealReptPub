using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealRept.Models
{
    public class Document
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Document")]
        [StringLength(200, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name ="Path to Document")]
        [StringLength(255, ErrorMessage = "Allowed length: {2}-{1} characters", MinimumLength = 2)]
        public string PathToDocument { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Modification Date")]
        [Column(TypeName = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime DateOfUploading { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        public bool IsDeleted { get; set; } = false;
    }
}
