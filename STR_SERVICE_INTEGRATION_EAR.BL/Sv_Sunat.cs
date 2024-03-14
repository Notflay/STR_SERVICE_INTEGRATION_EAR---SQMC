using STR_SERVICE_INTEGRATION_EAR.EL.Commons;
using STR_SERVICE_INTEGRATION_EAR.EL.Requests;
using STR_SERVICE_INTEGRATION_EAR.EL.Responses;
using STR_SERVICE_INTEGRATION_EAR.SQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.BL
{
    public class Sv_Sunat
    {
        //HanaADOHelper hash = new HanaADOHelper();
        public ConsultationResponse<ComprobanteResponses> ObtenerComprobante(ComprobanteRequest comprobante)
        {
            var respIncorrect = "No se encontró comprobante";
            List<ComprobanteResponses> list = new List<ComprobanteResponses>();
            ConsultaComprobante consulta = new ConsultaComprobante();

            try
            {
                var comp = consulta.ValidarDocumento(comprobante);
                
                list.Add(comp);

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<ComprobanteResponses>(ex);
            }
        }

    }
}
