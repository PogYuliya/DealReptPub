using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace DealRept.Models
{
    public class Event
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Title")]
        [StringLength(300, ErrorMessage = "Allowed length: {2}-{1} characters.", MinimumLength = 2)]
        public string Title { get; set; }

        [DateGreaterTodayAttributes]
        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Start")]
        [Column(TypeName = "DATETIME ")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy H:mm:ss}")]
        public DateTime Start { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "End")]
        [Column(TypeName = "DATETIME")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy H:mm:ss}")]
        public DateTime End { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Column(TypeName = "Text")]
        [Display(Name = "Description")]
        [DataType(DataType.Text)]
        [StringLength(500, ErrorMessage = "Allowed minimum length: {2} characters.", MinimumLength = 2)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "All Day")]
        public bool AllDay { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (End < Start.AddMinutes(30))
            {
                yield return new ValidationResult(
                    $"Please enter a value greater than or equal {Start.AddMinutes(30)}.",
                    new[] { nameof(End) });
            }

        }
    }
}
