using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.EL.Responses
{
    public class LoginResponse
    {
        public bool correcto { get; set; }
        public string mensaje { get; set; }
        public int empId { get; set; }
        public string token { get; set; }
    }
}
