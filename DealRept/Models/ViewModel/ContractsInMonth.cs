using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealRept.Models.ViewModel
{
    public class ContractsInMonth
    {
        public DateTime DateInOneMonth { get; set; }
        public int MonthNumber { get; set; }
        public int MonthlyTotalContractQuantity { get; set; }
        public int MonthlyCurrentContractQuantity { get; set; }
        public string MonthName { get; set; }

        public decimal MonthlyTotalAmmount { get; set; }
        public decimal MonthlyCurrentAmmount { get; set; }
    }
}
