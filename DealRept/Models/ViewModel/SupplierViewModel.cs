using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DealRept.Models.ViewModel
{
    public class SupplierViewModel
    {
        public Supplier Supplier { get; set; }

        public SelectList Banks { get; set; }

        public SelectList Cities { get; set; }

        [Display (Name ="Important Information")]
        public string ErrorMessage { get; set; }

        public SelectList Specializations { get; set; }
    }
}

