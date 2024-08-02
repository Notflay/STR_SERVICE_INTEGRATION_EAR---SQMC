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
                        RML_IMAGEN = dc["RML_IMAGEN"],
                        RML_SOCIEDAD = dc["RML_SOCIEDAD"],
                        RML_MAXADJRD = string.IsNullOrWhiteSpace(Convert.ToString(dc["RML_MAXADJRD"])) ? (int?)null : Convert.ToInt32(dc["RML_MAXADJRD"]),
                        RML_MAXADJSR = string.IsNullOrWhiteSpace(Convert.ToString(dc["RML_MAXADJSR"])) ? (int?)null : Convert.ToInt32(dc["RML_MAXADJSR"]),
                        RML_MAXAPRRD = string.IsNullOrWhiteSpace(Convert.ToString(dc["RML_MAXAPRRD"])) ? (int?)null : Convert.ToInt32(dc["RML_MAXAPRRD"]),
                        RML_MAXAPRSR = string.IsNullOrWhiteSpace(Convert.ToString(dc["RML_MAXAPRSR"])) ? (int?)null : Convert.ToInt32(dc["RML_MAXAPRSR"]),
                        RML_MAXRENDI_CURSO = string.IsNullOrWhiteSpace(Convert.ToString(dc["RML_MAXRENDI_CURSO"])) ? (int?)null : Convert.ToInt32(dc["RML_MAXRENDI_CURSO"]),
                        RML_OPERACION = dc["RML_OPERACION"],
                        RML_PARTIDAFLUJO = string.IsNullOrWhiteSpace(Convert.ToString(dc["RML_PARTIDAFLUJO"])) ? (int?)null : Convert.ToInt32(dc["RML_PARTIDAFLUJO"]),
                        RML_PLANTILLARD = dc["RML_PLANTILLARD"]
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
                        //Id = 1,
                        //Nombre = dc["Ruta"]
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
