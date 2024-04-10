using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STR_SERVICE_INTEGRATION_EAR.SQ;
using STR_SERVICE_INTEGRATION_EAR.EL;
using STR_SERVICE_INTEGRATION_EAR.EL.Commons;
using System.Collections;
using STR_SERVICE_INTEGRATION_EAR.EL.Requests;
using STR_SERVICE_INTEGRATION_EAR.EL.Responses;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;

namespace STR_SERVICE_INTEGRATION_EAR.BL
{
    public class SQ_Usuario
    {
       
        public async Task<ConsultationResponse<LoginElecResponse>> ValidaSesionAsync(EL.Requests.Login login)
        {
            var respOk = "OK";
            var respIncorrect = "No se encuentró sesión ";

            SqlADOHelper hash = new SqlADOHelper();
            List<LoginElecResponse> loginElecResponses = new List<LoginElecResponse>();

            try
            {

                string uri = ConfigurationManager.AppSettings["rutaLogin"].ToString();
                if (string.IsNullOrEmpty(uri))
                {
                    throw new Exception("No se encontró ruta de inicio de sesión, conctactar al administrador");
                }
             

                var json = JsonConvert.SerializeObject(login);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(uri),
                    Method = HttpMethod.Post,
                };



                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(new Uri(uri), content);


                    if (response.IsSuccessStatusCode)
                    {
                        LoginElecResponse respon = JsonConvert.DeserializeObject<LoginElecResponse>(response.Content.ReadAsStringAsync().Result);

                        if (respon.isValid)
                        {
                            loginElecResponses.Add(respon);
                            return Global.ReturnOk(loginElecResponses, respIncorrect);
                        }
                        else
                        {
                            throw new Exception("Contraseña o Usuario incorrecto, intentar nuevamente");
                        }
                    }
                    else
                    {
                        throw new Exception("Contraseña o Usuario incorrecto, intentar nuevamente");
                    }
                }

            }
            catch (Exception ex)
            {
                return new ConsultationResponse<LoginElecResponse>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message,

                };
            }

        }

        public ConsultationResponse<Usuario> getUsuarios()
        {
            var respOk = "OK";
            var respIncorrect = "No se encuentra Usuarios";

            SqlADOHelper hash = new SqlADOHelper();

            try
            {
                List<Usuario> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_usuarios), dc =>
                {
                    return new Usuario
                    {
                        empID = Convert.ToInt32(dc["empID"]),
                        Nombres = dc["Nombres"]
                    };
                }, string.Empty).ToList();

                return new ConsultationResponse<Usuario>
                {
                    CodRespuesta = list.Count() > 0 ? "00" : "22",
                    DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new ConsultationResponse<Usuario>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message,

                };
            }
        }
        public ConsultationResponse<Usuario> getUsuario(int id)
        {
            var respOk = "OK";
            var respIncorrect = "No se encuentra usuario registrado en SAP";

            SqlADOHelper hash = new SqlADOHelper();
            SQ_Complemento sQ = new SQ_Complemento();

            try
            {
                List<Usuario> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_infUser), dc =>
                {
                    return new Usuario
                    {
                        empID = Convert.ToInt32(dc["empID"]),
                        sex = dc["sex"],
                        jobTitle = dc["jobTitle"],
                        TipoUsuario = Convert.ToInt32(dc["U_STR_TIPO_USUARIO"]),
                        Nombres = dc["Nombres"],
                        SubGerencia = Convert.ToInt32(dc["branch"]),
                        dept = Convert.ToInt32(dc["dept"]),
                        email = dc["email"],
                        fax = dc["fax"],
                        numeroEAR = dc["U_CE_CEAR"]
                    };
                }, id.ToString()).ToList();

                return new ConsultationResponse<Usuario>
                {
                    CodRespuesta = list.Count() > 0 ? "00" : "22",
                    DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new ConsultationResponse<Usuario>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message,

                };
            }
        }

        public ConsultationResponse<Usuario> getUsuario2(string id)
        {
            var respOk = "OK";
            var respIncorrect = "No se encuentra usuario registrado en SAP";

            SqlADOHelper hash = new SqlADOHelper();
            SQ_Complemento sQ = new SQ_Complemento();

            try
            {
                List<Usuario> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_infUser2), dc =>
                {
                    return new Usuario
                    {
                        empID = Convert.ToInt32(dc["empID"]),
                        sex = dc["sex"],
                        jobTitle = dc["jobTitle"],
                        TipoUsuario = Convert.ToInt32(dc["U_STR_TIPO_USUARIO"]),
                        Nombres = dc["Nombres"],
                        SubGerencia = Convert.ToInt32(dc["branch"]),
                        dept = Convert.ToInt32(dc["dept"]),
                        email = dc["email"],
                        fax = dc["fax"],
                        numeroEAR = dc["U_CE_CEAR"],
                        CostCenter = dc["CostCenter"]
                    };
                }, id).ToList();

                return new ConsultationResponse<Usuario>
                {
                    CodRespuesta = list.Count() > 0 ? "00" : "22",
                    DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new ConsultationResponse<Usuario>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message,

                };
            }
        }

        public ConsultationResponse<CentroCosto> getCentroCosto(int id)
        {
            var respOk = "OK";
            var respIncorrect = "No se encuentra usuario";

            SqlADOHelper hash = new SqlADOHelper();

            try
            {
                List<CentroCosto> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_centroCosto), dc =>
                {
                    return new CentroCosto()
                    {
                        CostCenter = dc["CostCenter"],
                    };
                }, id.ToString()).ToList();

                return new ConsultationResponse<CentroCosto>
                {
                    CodRespuesta = list.Count() > 0 ? "00" : "22",
                    DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new ConsultationResponse<CentroCosto>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message,

                };
            }
        }
    }
}
