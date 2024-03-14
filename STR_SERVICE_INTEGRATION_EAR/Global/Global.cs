using STR_SERVICE_INTEGRATION_EAR.EL.Commons;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace STR_SERVICE_INTEGRATION_EAR
{
    public class Global
    {

        public static int ObtieneCampoTipoEar()
        {
            try
            {
                string valor;
                valor = ConfigurationManager.AppSettings["tipoEARfielID"].ToString();

                if (string.IsNullOrEmpty(valor))
                    return -99;
                else
                    return Convert.ToInt32(valor);
            }
            catch (Exception)
            {
                return -99;
            }
        }

        public static int ObtieneCampoTipoRuta()
        {
            try
            {
                string valor;
                valor = ConfigurationManager.AppSettings["tipoRutafielID"].ToString();

                if (string.IsNullOrEmpty(valor))
                    return -99;
                else
                    return Convert.ToInt32(valor);
            }
            catch (Exception)
            {
                return -99;
            }
        }

        public static ConsultationResponse<T> ReturnError<T>(Exception ex)
        {
            return new ConsultationResponse<T>
            {
                CodRespuesta = "99",
                DescRespuesta = ex.Message,

            };
        }

        public static ConsultationResponse<T> ReturnOk<T>(List<T> list, string respIncorrect)
        {
            string respOk = "OK";
            return new ConsultationResponse<T>
            {
                CodRespuesta = list.Count() > 0 ? "00" : "22",
                DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                Result = list
            };
        }
    }
}