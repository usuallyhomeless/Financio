using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FinancioAPI.Models
{
    public partial class Scheme
    {
        public Scheme()
        {
            Debittransaction = new HashSet<Debittransaction>();
        }

        public int Id { get; set; }
        public string Schemename { get; set; }
        public int Schemeduration { get; set; }

        public virtual ICollection<Debittransaction> Debittransaction { get; set; }
    }
}
