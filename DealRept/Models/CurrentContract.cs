using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealRept.Models
{
    public class CurrentContract:Contract
    {
        [NotMapped]
        [Display(Name = "Transition Date")]
        [Column(TypeName = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public override DateTime TransitionDate { get; set; }

        /*Navigation properties*/
        [NotMapped]
        public override ICollection<PastDocument> PastDocuments { get; set; }

        [NotMapped]
        public override ICollection<PastEvent> PastEvents { get; set; }
    }
}
