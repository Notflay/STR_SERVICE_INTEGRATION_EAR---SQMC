using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.EL.Requests
{
    public class ComprobanteRequest
    {
        public string numRuc { get; set; }
        public string codComp { get; set; }
        public string numeroSerie { get; set; }
        public string numero { get; set; }
        public string fechaEmision { get; set; }
        public string monto { get; set; }

        [JsonIgnore]
        public string estado { get; set; }
        [JsonIgnore]    
        public string observaciones { get; set; }
    }
}
