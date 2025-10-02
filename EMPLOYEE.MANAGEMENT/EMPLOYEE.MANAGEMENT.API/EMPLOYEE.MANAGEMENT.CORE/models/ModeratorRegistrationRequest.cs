using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPLOYEE.MANAGEMENT.CORE.models
{
    public class ModeratorRegistrationRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ApprovalCode { get; set; }
    }
}
