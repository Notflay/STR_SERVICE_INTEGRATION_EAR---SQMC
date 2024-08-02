using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.EL.Commons
{
    public class RendicionSerializer
    {
        [JsonProperty("Creator")]
        public string Creador { get; set; }
        [JsonProperty("U_ER_MNDA")]
        public string Moneda { get; set; }
        [JsonProperty("U_ER_FCRG")]
        public string FechaCargaDocs { get; set; }   // Fecha de Carga
        [JsonProperty("U_ER_NMBR")]
        public string UsuarioEARCod { get; set; }
        [JsonProperty("U_ER_NMRO")]
        public string NumRendicion { get; set; }
        [JsonProperty("U_ER_SLDI")]
        public double SaldoApertura { get; set; }
        [JsonProperty("U_ER_SLDF")]
        public double SaldoFinal { get; set; } // Defecto 0.0
        [JsonProperty("U_ER_TTDC")]
        public double DocsTotal { get; set; }
        [JsonProperty("U_STR_WEB_AUTPRI")]
        public string PrimerAutorizador { get; set; }
        [JsonProperty("U_STR_WEB_AUTSEG")]
        public string SegundoAutorizador { get; set; }
        [JsonProperty("U_STR_WEB_AUTCON")]
        public string ContableAutorizador { get; set; }
        /*
        public string Moneda { get; set; }
        [JsonProperty("U_CE_NMBR")]
        public string UsuarioEARCod { get; set; }
        [JsonProperty("U_CE_NMRO")]
        public string NumRendicion { get; set; }
        [JsonProperty("U_CE_TTDC")]
        public double DocsTotal { get; set; }
        [JsonProperty("U_CE_SLDI")]
        public double SaldoApertura { get; set; }
        [JsonProperty("U_CE_SLDF")]
        public double SaldoFinal { get; set; } // Defecto 0.0
        [JsonProperty("U_CE_FCRG")]
        public string FechaCargaDocs { get; set; }   // Fecha de Carga
        [JsonProperty("U_CE_TPOACTV")]
        public string TipoActividad { get; set; }
        [JsonProperty("U_CE_TPORND")]
        public string TipoRendicion { get; set; }       // EAR
        [JsonProperty("U_CE_ESTADO")]
        public string Estado { get; set; }
        */
        //public string U_RML_WEB_EMPASIG { get; set; }
        //public string U_RML_WEB_PRIID { get; set; }
        //public string U_RML_WEB_SEGID { get; set; }
        //public string U_RML_WEB_CONID { get; set; }

        public List<RendicionDetSerializer> STR_EARCRGDETCollection { get; set; }
    }
}
