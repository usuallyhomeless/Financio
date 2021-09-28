using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FinancioAPI.Models
{
    public partial class Debittransaction
    {
        public Debittransaction()
        {
            Credittransaction = new HashSet<Credittransaction>();
        }

        public int Id { get; set; }
        public int? Financiouser { get; set; }
        public int? Productid { get; set; }
        public int? Schemeid { get; set; }
        public DateTime? Transactiondatetime { get; set; }
        public decimal? Installmentamount { get; set; }
        public DateTime? Lastpaymentdatetime { get; set; }
        public decimal? Balanceleft { get; set; }
        public bool? Isactive { get; set; }

        public virtual Financiouser FinanciouserNavigation { get; set; }
        public virtual Product Product { get; set; }
        public virtual Scheme Scheme { get; set; }
        public virtual ICollection<Credittransaction> Credittransaction { get; set; }
    }
}
