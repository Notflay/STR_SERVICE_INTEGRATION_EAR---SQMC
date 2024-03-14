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
        public int? STR_RENDICION { get; set; }
        public string STR_FECHA_CONTABILIZA { get; set; }
        public string STR_FECHA_DOC { get; set; }
        public string STR_FECHA_VENCIMIENTO { get; set; }
        public Proveedor STR_PROVEEDOR { get; set; }
        //public string STR_RUC { get; set; }
        public Complemento STR_TIPO_AGENTE { get; set; }
        public Complemento STR_MONEDA { get; set; }
        public string STR_COMENTARIOS { get; set; }
        public Complemento STR_TIPO_DOC { get; set; }
        public string STR_SERIE_DOC { get; set; }
        public string STR_CORR_DOC { get; set; }
        public bool STR_VALIDA_SUNAT { get; set; }
        public string STR_ANEXO_ADJUNTO { get; set; }
        public string STR_OPERACION { get; set; }
        public int? STR_PARTIDAFLUJO { get; set; }
        public double STR_TOTALDOC { get; set; }
        public int STR_RD_ID { get; set; }
        public int STR_CANTIDAD { get; set; }
        public string STR_ALMACEN { get; set; }
        public string STR_RUC { get; set; }
        public string STR_RAZONSOCIAL { get; set; }
        public List<DocumentoDet> detalles { get; set; }
    }
}
