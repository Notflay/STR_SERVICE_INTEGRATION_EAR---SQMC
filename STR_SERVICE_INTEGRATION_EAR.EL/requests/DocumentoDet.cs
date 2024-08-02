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
        public string ID { get; set; }
        public Item RML_CODARTICULO { get; set; }
        public decimal RML_SUBTOTAL { get; set; }
        public Impuesto RML_INDIC_IMPUESTO { get; set; }
        public Complemento RML_DIM1 { get; set; }
        public Complemento RML_DIM3 { get; set; }
        public Complemento RML_ALMACEN { get; set; }
        public CuentaContable RML_CUENTA_CNTBL { get; set; }
        public int RML_CANTIDAD { get; set; }
        public string RML_TPO_OPERACION { get; set; }
        public decimal RML_PRECIO { get; set; }
        public int? RML_DOC_ID { get; set; }
    }
}
