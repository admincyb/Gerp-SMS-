using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.AccountManagement
{
    public class ChangePasswordBO
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        //Nxt line code for Reset password

        public int ReturnVal { get; set; }
        public string EmailId { get; set; }
    }
}
