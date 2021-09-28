using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancioAPI.ViewModels
{
    public class UserAndCard
    {        
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
        public int Cardtype { get; set; }
        public int Bank { get; set; }
        public string Accountnumber { get; set; }
        public int Ifsc { get; set; }
    }
}
