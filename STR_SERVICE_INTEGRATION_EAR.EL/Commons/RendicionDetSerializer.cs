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
        [JsonProperty("U_ER_CDPV")]
        public string CardCode { get; set; }
        [JsonProperty("U_ER_NMPV")]
        public string CardName { get; set; }
        [JsonProperty("U_ER_FCDC")]
        public string FechaConta { get; set; }
        [JsonProperty("U_ER_CMNT")]
        public string Comentarios { get; set; }
        [JsonProperty("U_ER_TDOC")]
        public string TipoDocumento { get; set; }
        [JsonProperty("U_ER_SDOC")]
        public string SerieDocumento { get; set; }
        [JsonProperty("U_ER_CDOC")]
        public string CorrelativoDocumento { get; set; }
        [JsonProperty("U_ER_CDAR")]
        public string ItemCode { get; set; }
        [JsonProperty("U_ER_DSAR")]
        public string Descripcion { get; set; }
        [JsonProperty("U_ER_CNAR")]
        public int Cantidad { get; set; }
        [JsonProperty("U_ER_ALAR")]
        public string Almacen { get; set; }
        [JsonProperty("U_ER_CSYS")]
        public string CuentaContable { get; set; }
        [JsonProperty("U_ER_IMPD")]
        public string IndicadorImpuesto { get; set; }
        [JsonProperty("U_ER_DIM1")]
        public string Dimension1 { get; set; }
        [JsonProperty("U_ER_DIM3")]
        public string Dimension3 { get; set; }
        [JsonProperty("U_ER_MNDC")]
        public string Moneda { get; set; }
        [JsonProperty("U_ER_PRPU")]
        public decimal PrecioUnidad { get; set; }
        [JsonProperty("U_ER_TTLN")]
        public decimal PrecioTotal { get; set; }
        [JsonProperty("U_ER_FCCT")]
        public string FechaDocumento { get; set; }
        [JsonProperty("U_ER_PW_RUTAD")]
        public string RutaAdjunto { get; set; }
        [JsonProperty("U_ER_CLDC")]
        public string Inventario { get; set; }
        public string U_ER_ESTD { get; set; }
        public string U_ER_SLCC { get; set; }
        public string U_ER_PW_TPDC { get; set; }
        /*
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
        //[JsonProperty("U_ODUM_U_BPP_MDTD")]
        //public string TipoDocumento { get; set; }
        //[JsonProperty("U_ODUM_U_BPP_MDCD")]
        //public string CorrelativoDocumento { get; set; }
        //[JsonProperty("U_ODUM_U_BPP_MDSD")]
        //public string SerieDocumento { get; set; }
        [JsonProperty("U_ODUM_DocDate")]
        public string FechaEmision { get; set; }
        [JsonProperty("U_ODUM_TaxDate")]
        public string FechaVencimiento { get; set; }
      
        [JsonProperty("U_CE_ESTD")]
        public string U_CE_ESTD { get; set; }
        //[JsonProperty("U_ODUM_Comments")]
        //public string Comentarios { get; set; }
        [JsonProperty("U_ODUM_U_CNCUP")]
        public string CUP { get; set; }
        [JsonProperty("U_CE_RTNC")]
        public string Retencion { get; set; }
        [JsonProperty("U_ER_PW_RUTAD")]
        public string RutaAdjunto { get; set; }*/

    }
}
