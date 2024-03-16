using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealRept.Models
{
    public class PastContract:Contract
    {
        [Required(ErrorMessage = "Please enter a value for {0}.")]
        [Display(Name = "Transition Date")]
        [Column(TypeName = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public override DateTime TransitionDate { get; set; }

        /*Navigation properties*/
        [NotMapped]
        public override ICollection<CurrentDocument> CurrentDocuments { get; set; }

        [NotMapped]
        public override ICollection<CurrentEvent> CurrentEvents { get; set; }
    }
}
