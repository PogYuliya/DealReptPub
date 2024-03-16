using System;
using System.ComponentModel.DataAnnotations;

namespace DealRept.Models.ViewModel
{
    public class DocumentViewModel
    {
        
        [Display(Name = "Document")]
        [StringLength(200, ErrorMessage = "Allowed lehgth: {2}-{1} characters.", MinimumLength = 2)]
        public string Name { get; set; }

       
        [StringLength(255, ErrorMessage = "Allowed lehgth: {2}-{1} characters", MinimumLength = 2)]
        public string PathToDocument { get; set; }

        [Required (ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Modification Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime DateOfUploading { get; set; }
       
        [Required (ErrorMessage ="Choose a document please.")]
        public FileUpload FileToUpload { get; set; }

        /*Foreign key*/
        public int CurrentContractID { get; set; }

        /*Navigation properties*/
        public CurrentContract CurrentContract { get; set; }
    }
}
