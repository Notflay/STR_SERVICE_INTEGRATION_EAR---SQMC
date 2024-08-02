using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.EL.Requests
{
    public class SolicitudRDSerializer
    {
        // public int Series { get; set; }
        // public int AttachmentEntry { get; set; }
        public string RequriedDate { get; set; }
        public string RequesterEmail { get; set; }
        public string U_CE_TPRN { get; set; }
        public int U_STR_WEB_COD { get; set; }
        public string Requester { get; set; }
        public string RequesterName { get; set; }
        public int RequesterBranch { get; set; }
        public int RequesterDepartment { get; set; }
        public int ReqType { get; set; }
        public string DocDate { get; set; }
        public string DocDueDate { get; set; }
        public string Comments { get; set; }
        public string DocCurrency { get; set; }
        public double DocRate { get; set; }
        public string DocType { get; set; }
        public string Printed { get; set; }
        public string AuthorizationStatus { get; set; }
        public string TaxCode { get; set; }
        public string TaxLiable { get; set; }
        public string U_STR_WEB_AUTPRI { get; set; }
        public string U_STR_WEB_AUTSEG { get; set; }
        public string U_STR_WEB_EMPASIG { get; set; }
        public string U_CE_MNDA {get; set;}
        public List<DetalleSerializar> DocumentLines { get; set; }
    }

    public class DetalleSerializar
    {
        public string ItemDescription { get; set; }
        public string ItemCode { get; set; }
        public string AccountCode { get; set; }
        public string RequiredDate { get; set; }
        public string LineVendor { get; set; }
        public decimal U_CE_IMSL { get; set; }

    }

}
