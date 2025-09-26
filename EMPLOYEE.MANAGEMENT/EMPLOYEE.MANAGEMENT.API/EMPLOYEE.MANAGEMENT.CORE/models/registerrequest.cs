using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPLOYEE.MANAGEMENT.CORE.models
{



    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // You may want to restrict allowed roles
        public string ApprovalCode { get; set; } // Required when registering as Moderator
    }
}

