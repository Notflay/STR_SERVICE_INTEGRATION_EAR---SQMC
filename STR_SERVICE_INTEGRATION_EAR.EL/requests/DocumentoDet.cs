using STR_SERVICE_INTEGRATION_EAR.EL.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.EL.Requests
{
    public class DocumentoDet
    {
        public int ID { get; set; }
        public Item STR_CODARTICULO { get; set; }
        public double STR_SUBTOTAL { get; set; }
        public Complemento STR_INDIC_IMPUESTO { get; set; }
        public Complemento STR_PROYECTO { get; set; }
        public CentroCosto STR_CENTCOSTO { get; set; }
        public Complemento STR_POS_FINANCIERA { get; set; }
        public Cup STR_CUP { get; set; }
        public string STR_ALMACEN { get; set; }
        public int STR_CANTIDAD { get; set; }
        public string STR_TPO_OPERACION { get; set; }
        public int STR_DOC_ID { get; set; }
    }
}
