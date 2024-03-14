using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.EL.Requests
{
    public class CFGeneral
    {
        public int ID { get; set; }
        public string STR_IMAGEN { get; set; }
        public string STR_SOCIEDAD { get; set; }
        public int? STR_MAXADJSR { get; set; }
        public int? STR_MAXADJRD { get; set; }
        public int? STR_MAXAPRSR { get; set; }
        public int? STR_MAXAPRRD { get; set; }
        public int? STR_MAXRENDI_CURSO { get; set; }
        public string STR_OPERACION { get; set; }
        public int? STR_PARTIDAFLUJO { get; set; }
        public string STR_PLANTILLARD { get; set; }
    }
}
