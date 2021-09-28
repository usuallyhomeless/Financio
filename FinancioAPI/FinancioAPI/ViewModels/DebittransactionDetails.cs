using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancioAPI.ViewModels
{
    public class DebittransactionDetails
    {
        public int userid { get; set; }
        public string debitproductname { get; set; }
        public int debitamountpaid { get; set; }
        public int debitbalance { get; set; }
    }
}
