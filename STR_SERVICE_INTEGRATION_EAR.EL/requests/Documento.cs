using STR_SERVICE_INTEGRATION_EAR.EL.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.EL.Requests
{
    public class Documento
    {
        public int? ID { get; set; }
      //  public int? RML_RENDICION { get; set; }
        public string RML_FECHA_CONTABILIZA { get; set; }
        public string RML_FECHA_DOC { get; set; }
        public string RML_FECHA_VENCIMIENTO { get; set; }
        public Proveedor RML_PROVEEDOR { get; set; }
        //public string RML_RUC { get; set; }
        public Complemento RML_TIPO_AGENTE { get; set; }
        public Complemento RML_MONEDA { get; set; }
        public string RML_COMENTARIOS { get; set; }
        public Complemento RML_TIPO_DOC { get; set; }
        public Impuesto RML_WLIABLE {get;set;}
        public string RML_SERIE_DOC { get; set; }
        public string RML_CORR_DOC { get; set; }
        public bool RML_VALIDA_SUNAT { get; set; }
        public Complemento RML_RETE_IMPST { get; set; }
        public string RML_ANEXO_ADJUNTO { get; set; }
        public string RML_OPERACION { get; set; }
        public int? RML_PARTIDAFLUJO { get; set; }
        public double RML_TOTALDOC { get; set; }
        public int RML_RD_ID { get; set; }
        public string RML_RUC { get; set; }
        public string RML_RAZONSOCIAL { get; set; }
        public List<DocumentoDet> detalles { get; set; }
    }
}
