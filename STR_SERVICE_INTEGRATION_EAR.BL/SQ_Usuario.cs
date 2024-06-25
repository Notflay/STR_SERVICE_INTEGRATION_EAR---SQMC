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
using DocumentFormat.OpenXml.Math;

namespace STR_SERVICE_INTEGRATION_EAR.BL
{
    public class SQ_Usuario
    {
        public string nombreDB = ConfigurationManager.AppSettings["CompanyDB"];

        public ConsultationResponse<Usuario> ObtieneSesion(LoginRequest user)
        {
            var respOk = "OK";
            var respIncorrect = "Usuario y/o contraseña incorrecta";

            SqlADOHelper hash = new SqlADOHelper();

            try
            {
                // Obtiene VALOR de la contraseña - Si no hay nada Es incorrecta
                string passActual = hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.get_tokenPass), user.username);

                if (string.IsNullOrWhiteSpace(passActual)) throw new Exception(respIncorrect);

                // Obtiencontraseña y hace la validación
                Encript validacion = new Encript();
                bool reslt = validacion.ValidarCredenciales(passActual, user.password);

                if (!reslt) throw new Exception(respIncorrect);

                List<Usuario> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_infoUser), dc =>
                {
                    return new Usuario()
                    {
                        empID = Convert.ToInt32(dc["RML_USUARIOSAP"]),
                        Nombres = dc["Nombres"],
                        activo = Convert.ToInt32(dc["RML_ACTIVO"]),
                        TipoUsuario = Convert.ToInt32(dc["RML_RODL_ID"]),
                        SubGerencia = Convert.ToInt32(dc["branch"]),
                        sex = dc["sex"]
                    };
                }, nombreDB, user.username).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Usuario>(ex);
            }
        }

        public ConsultationResponse<Usuario> getUsuarios()
        {
            var respOk = "OK";
            var respIncorrect = "No se encuentra Usuarios";

            SqlADOHelper hash = new SqlADOHelper();

            string bd = ConfigurationManager.AppSettings["CompanyDBINT"];

            try
            {
                List<Usuario> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_usuarios), dc =>
                {
                    return new Usuario
                    {
                        empID = Convert.ToInt32(dc["empID"]),
                        Nombres = dc["Nombres"]
                    };
                }, bd).ToList();

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
                        //sex = dc["sex"],
                        //jobTitle = dc["jobTitle"],
                        //TipoUsuario = Convert.ToInt32(dc["U_STR_TIPO_USUARIO"]),
                        //Nombres = dc["Nombres"],
                        //SubGerencia = Convert.ToInt32(dc["branch"]),
                        //dept = Convert.ToInt32(dc["dept"]),
                        //email = dc["email"],
                        //fax = dc["fax"],
                        //numeroEAR = dc["U_CE_CEAR"]
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
                        //sex = dc["sex"],
                        //jobTitle = dc["jobTitle"],
                        //TipoUsuario = Convert.ToInt32(dc["U_STR_TIPO_USUARIO"]),
                        //Nombres = dc["Nombres"],
                        //SubGerencia = Convert.ToInt32(dc["branch"]),
                        //dept = Convert.ToInt32(dc["dept"]),
                        //email = dc["email"],
                        //fax = dc["fax"],
                        //numeroEAR = dc["U_CE_CEAR"],
                        //CostCenter = dc["CostCenter"]
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
