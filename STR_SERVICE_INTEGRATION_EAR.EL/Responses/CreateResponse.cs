using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.EL.Responses
{
    public class CreateResponse
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        //[JsonIgnore]
        public int AprobacionFinalizada { get; set; }
    }
}
