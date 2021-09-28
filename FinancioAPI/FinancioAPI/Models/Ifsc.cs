using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FinancioAPI.Models
{
    public partial class Ifsc
    {
        public Ifsc()
        {
            Card = new HashSet<Card>();
        }

        public int Id { get; set; }
        public string Ifsccode { get; set; }
        public int? Bankid { get; set; }

        public virtual Bank Bank { get; set; }
        public virtual ICollection<Card> Card { get; set; }
    }
}
