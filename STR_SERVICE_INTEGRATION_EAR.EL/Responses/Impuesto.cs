using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.EL.Responses
{
    public class Impuesto
    {
        public string code { get; set; }
        public string descripcion { get; set; }
        public decimal rate { get; set; }
    }
}
