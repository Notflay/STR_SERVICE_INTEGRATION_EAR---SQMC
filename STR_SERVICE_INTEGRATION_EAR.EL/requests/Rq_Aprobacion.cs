using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.EL.Requests
{
    public class Rq_Aprobacion
    {
        public int usuarioId { get; set; }
        public string tipord { get; set; }
        public string area { get; set; }
        public double monto { get; set; }
        public int conta { get; set; }
        public int estado { get; set; }
        //public List<P_borrador> p_Borradores { get; set; }
    }

    public class P_borrador
    {
        public double RML_TOTAL { get; set; }
        public string RML_PENDIENTE { get; set; }
        public string RML_CENTCOST { get; set; }
        public string RML_POSFIN { get; set; }
        public string RML_FECHAREGIS { get; set; }
    }
}
