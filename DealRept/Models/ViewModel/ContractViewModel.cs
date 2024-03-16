using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace DealRept.Models.ViewModel
{
    public class ContractViewModel
    {
        public Contract Contract { get; set; }

        public SelectList Suppliers { get; set; }

        public SelectList Branches { get; set; }

        public SelectList ContractStatuses { get; set; }

        public List<CurrentDocumentVM> CurrentDocumentsToEdit { get; set; }
            = new List<CurrentDocumentVM>();

        public string ViewType { get; set; }
        
        public string ViewDate { get; set; }

        public string StartEventTerm { get; set; }
        
        public string EndEventTerm { get; set; }

    }
}
