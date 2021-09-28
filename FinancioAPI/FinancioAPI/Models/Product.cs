using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FinancioAPI.Models
{
    public partial class Product
    {
        public Product()
        {
            Debittransaction = new HashSet<Debittransaction>();
        }

        public int Id { get; set; }
        public string Productname { get; set; }
        public string Productdetails { get; set; }
        public decimal Cost { get; set; }
        public string Imageurl { get; set; }
        public string Extrafeatures { get; set; }

        public virtual ICollection<Debittransaction> Debittransaction { get; set; }
    }
}
