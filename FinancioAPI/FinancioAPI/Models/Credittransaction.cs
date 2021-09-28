using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FinancioAPI.Models
{
    public partial class Credittransaction
    {
        public int Id { get; set; }
        public int? Debittransactionid { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Transactiondatetime { get; set; }

        public virtual Debittransaction Debittransaction { get; set; }
    }
}
