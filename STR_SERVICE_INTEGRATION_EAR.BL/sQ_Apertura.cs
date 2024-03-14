using DocumentFormat.OpenXml.Office2010.Excel;
using STR_SERVICE_INTEGRATION_EAR.EL.Commons;
using STR_SERVICE_INTEGRATION_EAR.EL.Requests;
using STR_SERVICE_INTEGRATION_EAR.EL.Responses;
using STR_SERVICE_INTEGRATION_EAR.SQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.BL
{
    public class sQ_Apertura
    {
        HanaADOHelper hash = new HanaADOHelper();
        public ConsultationResponse<Complemento> CreaApertura(AperturaRequest apertura)
        {
            var respIncorrect = "No se terminó de crear Apertura";
            SQ_Complemento sQ = new SQ_Complemento();
            List<SolicitudRD> list = new List<SolicitudRD>();
            Sq_SolicitudRd sq_Solicitud = new Sq_SolicitudRd();
            try
            {
                 list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_solicitudRendicionPorDocn), dc =>
                {
                    return new SolicitudRD()
                    {
                        ID = Convert.ToInt32(dc["ID"]),
                        STR_DOCENTRY = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_DOCENTRY"])) ? (int?)null : Convert.ToInt32(dc["STR_DOCENTRY"]),
                        STR_NRSOLICITUD = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_NRSOLICITUD"])) ? (int?)null : Convert.ToInt32(dc["STR_NRSOLICITUD"]),
                        STR_ESTADO = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_ESTADO"])) ? (int?)null : Convert.ToInt32(dc["STR_ESTADO"]),
                        STR_MOTIVO = dc["STR_MOTIVO"],
                        STR_UBIGEO = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_UBIGEO"])) ? (int?)null : Convert.ToInt32(dc["STR_UBIGEO"]),
                        STR_RUTA = dc["STR_RUTA"],
                        STR_RUTAANEXO = dc["STR_RUTAANEXO"],
                        STR_MONEDA = dc["STR_MONEDA"],
                        STR_TIPORENDICION = dc["STR_TIPORENDICION"],
                        STR_TOTALSOLICITADO = Convert.ToDouble(dc["STR_TOTALSOLICITADO"]),
                        STR_MOTIVOMIGR = dc["STR_MOTIVOMIGR"],
                        STR_EMPLDASIG = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_EMPLDASIG"])) ? (int?)null : Convert.ToInt32(dc["STR_EMPLDASIG"]),
                        STR_EMPLDREGI = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_EMPLDREGI"])) ? (int?)null : Convert.ToInt32(dc["STR_EMPLDREGI"]),
                        STR_FECHAREGIS = dc["STR_FECHAREGIS"],
                        STR_FECHAINI = dc["STR_FECHAINI"],
                        STR_FECHAFIN = dc["STR_FECHAFIN"],
                        STR_FECHAVENC = dc["STR_FECHAVENC"],

                    };
                }, apertura.NroSolicitud.ToString()).ToList();

                var s = list[0].ID;

                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.post_insertRendicion),s, apertura.NumeroApertura,apertura.IDSolicitud,null,"8", list[0].STR_EMPLDASIG, list[0].STR_EMPLDREGI, 0, DateTime.Now.ToString("yyyy-MM-dd"), null, null, list[0].STR_TOTALSOLICITADO);

                return null;
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }

        }
    }
}
