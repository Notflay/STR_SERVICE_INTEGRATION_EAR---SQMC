using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.EL.Responses
{
    public class ComprobanteResponses
    {
        public bool success { get; set; }
        public string message { get; set; }
        public Data data { get; set; }


    }
    public class Data
    {
        public string estadoCp { get; set; }
        public string estadoRuc { get; set; }
        public string condDomiRuc { get; set; }
        public string[] observaciones { get; set; }
    }
}
