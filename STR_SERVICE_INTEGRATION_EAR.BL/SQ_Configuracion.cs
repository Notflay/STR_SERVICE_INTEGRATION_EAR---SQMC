using STR_SERVICE_INTEGRATION_EAR.EL.Commons;
using STR_SERVICE_INTEGRATION_EAR.EL.Requests;
using STR_SERVICE_INTEGRATION_EAR.EL.Responses;
using STR_SERVICE_INTEGRATION_EAR.SQ;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.BL
{
    public class SQ_Configuracion
    {
        public ConsultationResponse<CFGeneral> getCFGeneral(string sociedad = "ZZZ_PRB_EAR")
        {
            var respOk = "OK";
            var respIncorrect = "No se encuentra Sociedad";
            SqlADOHelper hash = new SqlADOHelper();

           sociedad =  ConfigurationManager.AppSettings["CompanyDB"].ToString();


            try
            {
                List<CFGeneral> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_cfGeneral), dc =>
                {
                    return new CFGeneral
                    {
                        ID = Convert.ToInt32(dc["ID"]),
                        STR_IMAGEN = dc["STR_IMAGEN"],
                        STR_SOCIEDAD = dc["STR_SOCIEDAD"],
                        STR_MAXADJRD = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_MAXADJRD"])) ? (int?)null : Convert.ToInt32(dc["STR_MAXADJRD"]),
                        STR_MAXADJSR = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_MAXADJSR"])) ? (int?)null : Convert.ToInt32(dc["STR_MAXADJSR"]),
                        STR_MAXAPRRD = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_MAXAPRRD"])) ? (int?)null : Convert.ToInt32(dc["STR_MAXAPRRD"]),
                        STR_MAXAPRSR = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_MAXAPRSR"])) ? (int?)null : Convert.ToInt32(dc["STR_MAXAPRSR"]),
                        STR_MAXRENDI_CURSO = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_MAXRENDI_CURSO"])) ? (int?)null : Convert.ToInt32(dc["STR_MAXRENDI_CURSO"]),
                        STR_OPERACION = dc["STR_OPERACION"],
                        STR_PARTIDAFLUJO = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_PARTIDAFLUJO"])) ? (int?)null : Convert.ToInt32(dc["STR_PARTIDAFLUJO"]),
                        STR_PLANTILLARD = dc["STR_PLANTILLARD"]
                    };
                }, sociedad).ToList();

                return new ConsultationResponse<CFGeneral>
                {
                    CodRespuesta = list.Count() > 0 ? "00" : "22",
                    DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new ConsultationResponse<CFGeneral>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message,

                };
            }
        }

        public ConsultationResponse<Complemento> getRutaAdjSAP()
        {
            var respOk = "OK";
            var respIncorrect = "No se encuentra Ruta de anexo";
            SqlADOHelper hash = new SqlADOHelper();

            try
            {
                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_rutaAdjSap), dc =>
                {
                    return new Complemento
                    {
                        Id = 1,
                        Nombre = dc["Ruta"]
                    };
                }, string.Empty).ToList();

                return new ConsultationResponse<Complemento>
                {
                    CodRespuesta = list.Count() > 0 ? "00" : "22",
                    DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new ConsultationResponse<Complemento>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message,

                };
            }
        }
    }
}
