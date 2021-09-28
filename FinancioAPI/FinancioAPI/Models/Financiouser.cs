using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FinancioAPI.Models
{
    public partial class Financiouser
    {
        public Financiouser()
        {
            Card = new HashSet<Card>();
            Debittransaction = new HashSet<Debittransaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
        public bool? Isadmin { get; set; }

        public virtual ICollection<Card> Card { get; set; }
        public virtual ICollection<Debittransaction> Debittransaction { get; set; }
    }
}
