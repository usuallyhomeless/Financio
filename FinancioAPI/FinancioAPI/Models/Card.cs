using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FinancioAPI.Models
{
    public partial class Card
    {
        public int Id { get; set; }
        public int? Financiouser { get; set; }
        public DateTime Registrationdate { get; set; }
        public DateTime Validupto { get; set; }
        public decimal? Cardlimit { get; set; }
        public bool? Isactive { get; set; }
        public int Cardtypeid { get; set; }
        public int Bankid { get; set; }
        public string Accountnumber { get; set; }
        public int Ifscid { get; set; }

        public virtual Bank Bank { get; set; }
        public virtual Cardtype Cardtype { get; set; }
        public virtual Financiouser FinanciouserNavigation { get; set; }
        public virtual Ifsc Ifsc { get; set; }
    }
}
