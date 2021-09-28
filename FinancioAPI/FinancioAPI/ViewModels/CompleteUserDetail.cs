using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancioAPI.ViewModels
{
    public class CompleteUserDetail
    {
        public string Name { get; set; }
        public DateTime Dob { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public string CardName { get; set; }
        public string Bankname { get; set; }
        public string Ifcscode { get; set; }
        public string Accountnumber { get; set; }
        public bool? Isadmin { get; set; }
        public bool? Isactive { get; set; }
     }
}
