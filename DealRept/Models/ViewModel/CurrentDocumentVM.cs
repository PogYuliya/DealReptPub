using System;
using System.ComponentModel.DataAnnotations;

namespace DealRept.Models.ViewModel
{
    public class CurrentDocumentVM
    {
        public int ID { get; set; }

        [Required (ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Document")]
        [StringLength(200, ErrorMessage = "Allowed lehgth: {2}-{1} characters.", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [StringLength(255, ErrorMessage = "Allowed lehgth: {2}-{1} characters.", MinimumLength = 2)]
        public string PathToDocument { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Upload Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime DateOfUploading { get; set; }

        public bool IsMarkedToDelete { get; set; } = false;

        public bool IsDeleted { get; set; }
    }

}
