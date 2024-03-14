using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.EL.Requests
{
    public class SolicitudRDSerializer
    {
        public int Series { get; set; }
        public int AttachmentEntry { get; set; }
        public string RequriedDate { get; set; }
        public string RequesterEmail { get; set; }
        public string U_STR_TIPOEAR { get; set; }
        public string U_DEPARTAMENTO { get; set; }
        public string U_PROVINCIA { get; set; }
        public string U_DISTRITO { get; set; }
        public string U_STR_TIPORUTA { get; set; }
        public string U_FECINI { get; set; }
        public string U_FECFIN { get; set; }
        public string U_CE_MNDA { get; set; }
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
        public string U_ELE_SEDE { get; set; }
        public string U_ELE_SUBGER { get; set; }
        public string TaxCode { get; set; }
        public string TaxLiable { get; set; }
        public string U_STR_WEB_AUTPRI { get; set; }
        public string U_STR_WEB_AUTSEG { get; set; }
        public string U_STR_WEB_ORDV { get; set; }
        public string U_STR_WEB_PRIID { get; set; }
        public string U_STR_WEB_SEGID { get; set; }
        public string U_STR_WEB_EMPASIG { get; set; }
        public List<DetalleSerializar> DocumentLines { get; set; }
    }

    public class DetalleSerializar
    {
        public string ItemCode { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public string Currency { get; set; }
        public string CostingCode { get; set; }
        public string CostingCode2 { get; set; }
        public string U_CNCUP { get; set; }
        public string TaxCode { get; set; }
    }

}
