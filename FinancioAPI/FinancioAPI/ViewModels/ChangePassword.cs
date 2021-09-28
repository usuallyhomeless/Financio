using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancioAPI.ViewModels
{
    public class ChangePassword
    {
        public string currentPassword { get; set; }
        public string newPassword { get; set; }
        public string confirmNewPassword { get; set; }
    }
}
