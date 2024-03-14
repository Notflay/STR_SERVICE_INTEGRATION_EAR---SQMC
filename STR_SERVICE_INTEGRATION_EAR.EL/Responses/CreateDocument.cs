using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.EL.Responses
{
    public class CreateDocument
    {
        public bool Exitoso { get; set; }
        public byte[] data { get; set; }
    }
}
