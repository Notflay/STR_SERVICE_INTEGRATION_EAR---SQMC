using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.EL.Requests
{
    public class PresupuestoRq
    {
        public string centCostos { get; set; }
        public string posFinanciera { get; set; }
        public string anio { get; set; }
        public decimal precio { get; set; }
    }
}
