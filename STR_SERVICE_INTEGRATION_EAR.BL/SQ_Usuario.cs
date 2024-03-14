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
        /*
        public ConsultationResponse<Item> getItem(string id)
        {

            var qry = "SELECT \"ItemCode\",\"ItemName\" FROM OITM WHERE \"ItemCode\" = '{0}'";
            var respOk = "OK";
            var respIncorrect = "No Hay Items";
            var mensaje = "";

            HanaADOHelper hash = new HanaADOHelper();

            try
            {
                List<Item> list = hash.GetResultAsType(qry, dc =>
                {


                    return new Item
                    {
                        ItemCode = dc["ItemCode"],
                        ItemName = dc["ItemName"]
                    };
                }, id).ToList();

                return new ConsultationResponse<Item>
                {
                    CodRespuesta = list.Count() > 0 ? "00" : "22",
                    DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                    Result = list
                };

            }
            catch (Exception ex)
            {
                return new ConsultationResponse<Item>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message,

                };
            }

        }
        public ConsultationResponse<Item> getItems(int cantidad)
        {

            var qry = "SELECT TOP {0} \"ItemCode\",\"ItemName\" FROM OITM";
            var respOk = "OK";
            var respIncorrect = "No Hay Items";
            var mensaje = "";

            HanaADOHelper hash = new HanaADOHelper();

            try
            {
                List<Item> list = hash.GetResultAsType(qry, dc =>
                {
                    return new Item
                    {
                        ItemCode = dc["ItemCode"],
                        ItemName = dc["ItemName"]
                    };
                }, cantidad.ToString()).ToList();

                return new ConsultationResponse<Item>
                {
                    CodRespuesta = list.Count() > 0 ? "00" : "22",
                    DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                    Result = list
                };

            }
            catch (Exception ex)
            {
                return new ConsultationResponse<Item>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message,

                };
            }

        }
        /*
        public object getUsuario(int id, string campo)
        {
            var respOk = "OK";
            var respIncorrect = "No se encuentra usuario";

            HanaADOHelper hash = new HanaADOHelper();

            try
            {
                List<string> list = new List<string>
                {
                    hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.get_infUser), campo, id.ToString()).ToString()
                };

                return new ConsultationResponse<string>
                {
                    CodRespuesta = list.Count() == 1 ? "00" : list.Count() > 1 ? "01" : "22",
                    DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new ConsultationResponse<string>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message,

                };
            }
        }
        */

        public async Task<ConsultationResponse<LoginElecResponse>> ValidaSesionAsync(EL.Requests.Login login)
        {
            var respOk = "OK";
            var respIncorrect = "No se encuentró sesión ";

            HanaADOHelper hash = new HanaADOHelper();
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

            HanaADOHelper hash = new HanaADOHelper();

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

            HanaADOHelper hash = new HanaADOHelper();
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

            HanaADOHelper hash = new HanaADOHelper();
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

            HanaADOHelper hash = new HanaADOHelper();

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
