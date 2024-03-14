using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.EL.Responses
{
    public class FileRS
    {
        public string name { get; set; }
        public string status { get; set; }
        public long size { get; set; }
        public string type { get; set; }
        public byte[] data { get; set; }
    }
}
