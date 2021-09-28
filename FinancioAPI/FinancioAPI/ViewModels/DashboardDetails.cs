using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancioAPI.ViewModels
{
    public class DashboardDetails
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int Cardnumber { get; set; }
        public DateTime Validupto { get; set; }
        public string Cardname { get; set; }
        public bool isCardActive { get; set; }
        public float TotalCredit { get; set; }
        public float CardLimit { get; set; }
    }
}
