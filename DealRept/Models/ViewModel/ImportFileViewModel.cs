using System.ComponentModel.DataAnnotations;

namespace DealRept.Models.ViewModel
{
    public class ImportFileViewModel
    {
        [Display(Name = "File To Import")]
        [StringLength(200, ErrorMessage = "Allowed lehgth: {2}-{1} characters.", MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(255, ErrorMessage = "Allowed lehgth: {2}-{1} characters", MinimumLength = 2)]
        public string PathToDocument { get; set; }

        [Required(ErrorMessage = "Choose a file please.")]
        public FileUpload FileToUpload { get; set; }

    }
}
