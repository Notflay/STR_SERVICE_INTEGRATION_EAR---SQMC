using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.EL.Responses
{
    public class LoginElecResponse
    {
        public string username { get; set; }
        public bool isValid { get; set; }
        public string token { get; set; }
    }
}
