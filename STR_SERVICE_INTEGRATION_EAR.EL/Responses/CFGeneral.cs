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
        public string RML_IMAGEN { get; set; }
        public string RML_SOCIEDAD { get; set; }
        public int? RML_MAXADJSR { get; set; }
        public int? RML_MAXADJRD { get; set; }
        public int? RML_MAXAPRSR { get; set; }
        public int? RML_MAXAPRRD { get; set; }
        public int? RML_MAXRENDI_CURSO { get; set; }
        public string RML_OPERACION { get; set; }
        public int? RML_PARTIDAFLUJO { get; set; }
        public string RML_PLANTILLARD { get; set; }
    }
}
