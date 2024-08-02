using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.EL.Requests
{
    public class AperturaRequest
    {
        public string CodigoEAR { get; set; }           // CODIGO RML_NRRENDICION
        public string Solicitante { get; set; }         
        public int IDSolicitud { get; set; }            
        public string NroSolicitud { get; set; }        //  NUMERO DE LA SOLICITUD HACER GET PARA CAPTURAR EL ID
        public string NumeroApertura { get; set; }      // RML_NRRENDICION
        public string CodigoCuenta { get; set; }        
        public string Moneda { get; set; }      
        public double Monto { get; set; }               // NUMERO APERTURA
        public double MontoPago { get; set; }
    }
}
