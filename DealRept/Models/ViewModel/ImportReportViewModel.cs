using System.Collections.Generic;

namespace DealRept.Models.ViewModel
{
    public class ImportReportViewModel
    {
        //public IDictionary<string,string> ImportedData { get; set; }
        ////public IDictionary<string,string> UnuccessImportedData { get; set; }
        public int SuccessImportCount { get; set; }
        public int UnSuccessImportCount { get; set; }


    }
}
