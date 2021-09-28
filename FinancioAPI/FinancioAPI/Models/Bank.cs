using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FinancioAPI.Models
{
    public partial class Bank
    {
        public Bank()
        {
            Card = new HashSet<Card>();
            Ifsc = new HashSet<Ifsc>();
        }

        public int Id { get; set; }
        public string Bankname { get; set; }

        public virtual ICollection<Card> Card { get; set; }
        public virtual ICollection<Ifsc> Ifsc { get; set; }
    }
}
