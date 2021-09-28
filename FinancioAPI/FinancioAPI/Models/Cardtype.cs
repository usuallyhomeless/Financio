using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FinancioAPI.Models
{
    public partial class Cardtype
    {
        public Cardtype()
        {
            Card = new HashSet<Card>();
        }

        public int Id { get; set; }
        public string Cardname { get; set; }
        public decimal Charge { get; set; }
        public decimal Creditlimit { get; set; }

        public virtual ICollection<Card> Card { get; set; }
    }
}
