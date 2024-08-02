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
        SqlADOHelper hash = new SqlADOHelper();
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
                        RML_DOCENTRY = string.IsNullOrWhiteSpace(Convert.ToString(dc["RML_DOCENTRY"])) ? (int?)null : Convert.ToInt32(dc["RML_DOCENTRY"]),
                        RML_NRSOLICITUD = string.IsNullOrWhiteSpace(Convert.ToString(dc["RML_NRSOLICITUD"])) ? (int?)null : Convert.ToInt32(dc["RML_NRSOLICITUD"]),
                        //RML_ESTADO = string.IsNullOrWhiteSpace(Convert.ToString(dc["RML_ESTADO"])) ? (int?)null : Convert.ToInt32(dc["RML_ESTADO"]),
                        //RML_MOTIVO = dc["RML_MOTIVO"],
                        //RML_UBIGEO = string.IsNullOrWhiteSpace(Convert.ToString(dc["RML_UBIGEO"])) ? (int?)null : Convert.ToInt32(dc["RML_UBIGEO"]),
                        //RML_RUTA = dc["RML_RUTA"],
                        //RML_RUTAANEXO = dc["RML_RUTAANEXO"],
                        //RML_MONEDA = dc["RML_MONEDA"],
                        //RML_TIPORENDICION = dc["RML_TIPORENDICION"],
                        //RML_TOTALSOLICITADO = Convert.ToDouble(dc["RML_TOTALSOLICITADO"]),
                        //RML_MOTIVOMIGR = dc["RML_MOTIVOMIGR"],
                        //RML_EMPLDASIG = string.IsNullOrWhiteSpace(Convert.ToString(dc["RML_EMPLDASIG"])) ? (int?)null : Convert.ToInt32(dc["RML_EMPLDASIG"]),
                        //RML_EMPLDREGI = string.IsNullOrWhiteSpace(Convert.ToString(dc["RML_EMPLDREGI"])) ? (int?)null : Convert.ToInt32(dc["RML_EMPLDREGI"]),
                        //RML_FECHAREGIS = dc["RML_FECHAREGIS"],
                        //RML_FECHAINI = dc["RML_FECHAINI"],
                        RML_FECHAFIN = dc["RML_FECHAFIN"],
                        RML_FECHAVENC = dc["RML_FECHAVENC"],

                    };
                }, apertura.NroSolicitud.ToString()).ToList();

                var s = list[0].ID;

                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.post_insertRendicion),s, apertura.NumeroApertura,apertura.IDSolicitud,null,"8", list[0].RML_EMPLDASIG, list[0].RML_EMPLDREGI, 0, DateTime.Now.ToString("yyyy-MM-dd"), null, null, list[0].RML_TOTALSOLICITADO);

                return null;
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }

        }
    }
}
