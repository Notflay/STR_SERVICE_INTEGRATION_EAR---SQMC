using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.EL.Commons
{
    public class RendicionDetSerializer
    {
        [JsonProperty("U_DUM1_PriceAfVAT")]
        public double PrecioUnidad { get; set; }
        [JsonProperty("U_CE_TTLN")]
        public double TotalLinea { get; set; }
        [JsonProperty("U_DUM1_Quantity")]
        public double Cantidad { get; set; }
        [JsonProperty("U_ODUM_DocCur")]
        public string Moneda { get; set; }
        [JsonProperty("U_DUM1_TaxCode")]
        public string Impuesto { get; set; }
        [JsonProperty("U_DUM1_WhsCode")]
        public string Almacen { get; set; }
        [JsonProperty("U_ObjType")]
        public string U_ObjType { get; set; }
        [JsonProperty("U_DUM1_OcrCode2")]
        public string PosFinanciera { get; set; }
        [JsonProperty("U_DUM1_OcrCode")]
        public string CentroDCosto { get; set; }
        [JsonProperty("U_DUM1_ItemCode")]
        public string ItemCode { get; set; }
        [JsonProperty("U_ODUM_CardCode")]
        public string CardCode { get; set; }
        [JsonProperty("U_DUM1_Project")]
        public string Proyecto { get; set; }
        [JsonProperty("U_ODUM_LicTradNum")]
        public string RUC { get; set; }
        [JsonProperty("U_DUM1_U_tipoOpT12")]
        public string TipoOperacion { get; set; }
        [JsonProperty("U_DUM1_Dscription")]
        public string ItemDescription { get; set; }
        [JsonProperty("U_CE_SLCC")]
        public string U_CE_SLCC { get; set; }
        [JsonProperty("U_ODUM_DocType")]
        public string U_ODUM_DocType { get; set; }
        [JsonProperty("U_ODUM_U_BPP_MDTD")]
        public string TipoDocumento { get; set; }
        [JsonProperty("U_ODUM_U_BPP_MDCD")]
        public string CorrelativoDocumento { get; set; }
        [JsonProperty("U_ODUM_U_BPP_MDSD")]
        public string SerieDocumento { get; set; }
        [JsonProperty("U_ODUM_DocDate")]
        public string FechaEmision { get; set; }
        [JsonProperty("U_ODUM_TaxDate")]
        public string FechaVencimiento { get; set; }
        [JsonProperty("U_ODUM_CardName")]
        public string CardName { get; set; }
        [JsonProperty("U_CE_ESTD")]
        public string U_CE_ESTD { get; set; }
        [JsonProperty("U_ODUM_Comments")]
        public string Comentarios { get; set; }
        [JsonProperty("U_ODUM_U_CNCUP")]
        public string CUP { get; set; }
        [JsonProperty("U_CE_RTNC")]
        public string Retencion { get; set; }
        [JsonProperty("U_ER_PW_RUTAD")]
        public string RutaAdjunto { get; set; }
       
    }
}
