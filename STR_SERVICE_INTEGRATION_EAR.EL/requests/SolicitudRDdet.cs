using STR_SERVICE_INTEGRATION_EAR.EL.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.EL.Requests
{
    public class SolicitudRDdet
    {
        public int id { get; set; }
        public int ID { get; set; }
        public string RML_CODARTICULO { get; set; }
        public string RML_CONCEPTO { get; set; }
        public double RML_TOTAL { get; set; }
        public int RML_CANTIDAD { get; set; }
        public string RML_POSFINAN { get; set; }
        public string RML_CUP { get; set; }
        public int SR_ID { get; set; }
        public Item articulo { get; set; }
        //public List<Complemento> centCostos { get; set; }
        public Cup cup { get; set; }
        public Complemento posFinanciera { get; set; }
        public int cantidad { get; set; }
        public double precioUnitario { get; set; }
        public double precioTotal { get; set; }
        public string ceco { get; set; }
        public string ctc { get; set; }
    }
}
