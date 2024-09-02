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
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Math;
using RestSharp;
using STR_SERVICE_INTEGRATION_EAR.SL;
using System.Security.Policy;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
                // Obtiene VALOR de la contraseña - Si no hay nada es incorrecta
                string passActual = hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.get_tokenPass), user.username).ToString();

                if (string.IsNullOrWhiteSpace(passActual)) throw new Exception(respIncorrect);

                // Obtiene contraseña y hace la validación
                Encript validacion = new Encript();
                bool reslt = validacion.ValidarCredenciales(passActual, user.password);

                if (!reslt) throw new Exception(respIncorrect);

                // Obtiene la información del usuario
                List<Usuario> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_infoUser), dc =>
                {
                    return new Usuario()
                    {
                        ID = Convert.ToInt32(dc["ID"]),
                        empID = Convert.ToInt32(dc["RML_USUARIOSAP"]),
                        Nombres = dc["Nombres"],
                        activo = Convert.ToInt32(dc["RML_ACTIVO"]),
                        TipoUsuario = new Complemento() { id = dc["RML_IDROL"], name = dc["RML_NOMBRE_ROL"] },
                        SubGerencia = Convert.ToInt32(dc["branch"]),
                        sex = dc["sex"],
                        numeroEAR = dc["U_CE_CEAR"]
                    };
                }, nombreDB, user.username).ToList();

                if (list.Count == 0)
                    throw new Exception(respIncorrect);

                if (list.FirstOrDefault().activo == 0)
                    throw new Exception("Usuario deshabilitado");

                // Si el usuario es válido, generar el token
                Usuario usuario = list.FirstOrDefault();
                string token = GenerarJwtToken(usuario.ID, usuario.TipoUsuario.id);

                // Incluir el token en la respuesta
                return new ConsultationResponse<Usuario>
                {
                    CodRespuesta = "00",
                    DescRespuesta = respOk,
                    Result = new List<Usuario> { usuario },
                    Token = token // Asumiendo que tu modelo tiene un campo para el token
                };
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Usuario>(ex);
            }
        }
        private string GenerarJwtToken(int userId, string roleId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["secret"]); // Utiliza una clave secreta adecuada

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("id", userId.ToString()),
                new Claim("rol", roleId)
             }),
                Expires = DateTime.UtcNow.AddHours(1), // El token expira en 1 hora
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string Fn_CambiarContrasenia(int id, string oldPass, string newPass)
        {
            SqlADOHelper hash = new SqlADOHelper();
            // Mensaje de error
            try
            {
                string passActual = hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.get_passForId), id.ToString()).ToString();
                if (string.IsNullOrEmpty(passActual))
                    throw new Exception("No se tiene contraseña a editar");

                if (string.IsNullOrEmpty(oldPass))
                    throw new Exception("Contraseña antigua vacía");

                if (string.IsNullOrEmpty(newPass))
                    throw new Exception("Contraseña nueva vacía");

                // Validar que la nueva contraseña tenga entre 5 y 20 caracteres
                if (newPass.Length < 5 || newPass.Length > 20)
                    throw new Exception("La nueva contraseña debe tener entre 5 y 20 caracteres");

                if (oldPass.Length < 5 || oldPass.Length > 20)
                    throw new Exception("La nueva contraseña debe tener entre 5 y 20 caracteres");

                Encript validacion = new Encript();
                string _oldPass = validacion.ObtenerCredencialCifrad(oldPass);

                if (_oldPass != passActual)
                    throw new Exception("Contraseña incorrecta");

                string _newPass = validacion.ObtenerCredencialCifrad(newPass);

                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_actualizarContrasenia), id.ToString(), _newPass);

                return "Contraseña actualizada exitosamente";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);  // Código de error
            }
        }
        public Usuario getUsuario(int id)
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
                        empID = Convert.ToInt32(dc["RML_USUARIOSAP"]),
                        Nombres = dc["Nombres"],
                        activo = Convert.ToInt32(dc["RML_ACTIVO"]),
                        TipoUsuario = new Complemento() { id = dc["RML_IDROL"], name = dc["RML_NOMBRE_ROL"] },
                        SubGerencia = Convert.ToInt32(dc["branch"]),
                        sex = dc["sex"],
                        provAsociado = dc["U_CE_PVAS"],
                        numeroEAR = dc["U_CE_CEAR"]
                    };
                }, nombreDB, id.ToString()).ToList();

                return list[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ConsultationResponse<EmpleadoSAP> getUsuarios()
        {
            var respOk = "OK";
            var respIncorrect = "No se encuentra Usuarios";

            SqlADOHelper hash = new SqlADOHelper();

            string bd = ConfigurationManager.AppSettings["CompanyDBINT"];

            try
            {
                List<EmpleadoSAP> list = hash.GetResultAsTypeDirecta(SQ_QueryManager.Generar(SQ_Query.get_usuarios), dc =>
                {
                    return new EmpleadoSAP
                    {
                        empleadoID = Convert.ToInt32(dc["empID"]),
                        nombre = dc["Nombres"]
                    };
                }, bd).ToList();

                return new ConsultationResponse<EmpleadoSAP>
                {
                    CodRespuesta = list.Count() > 0 ? "00" : "22",
                    DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new ConsultationResponse<EmpleadoSAP>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message,

                };
            }
        }
        public ConsultationResponse<UsuarioSAP> getUsuariosPend()
        {
            var respOk = "OK";
            var respIncorrect = "No se encuentra Usuarios";

            SqlADOHelper hash = new SqlADOHelper();

            try
            {
                List<UsuarioSAP> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_empleadosPendientes), dc =>
                {
                    return new UsuarioSAP
                    {
                        EmpleadoId = Convert.ToInt32(dc["empID"]),
                        Nombre = dc["firstName"],
                        Apellido = dc["lastName"],
                        Cargo = dc["jobTitle"],
                        Email = dc["email"]
                    };
                }, string.Empty).ToList();

                return new ConsultationResponse<UsuarioSAP>
                {
                    CodRespuesta = list.Count() > 0 ? "00" : "22",
                    DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new ConsultationResponse<UsuarioSAP>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message,

                };
            }
        }
        public EmpleadoSAP getEmpleado(string id)
        {
            var respOk = "OK";
            var respIncorrect = "No se encuentra Usuarios";

            SqlADOHelper hash = new SqlADOHelper();

            try
            {
                List<EmpleadoSAP> list = hash.GetResultAsTypeDirecta(SQ_QueryManager.Generar(SQ_Query.get_empleado), dc =>
                {
                    return new EmpleadoSAP
                    {
                        empleadoID = Convert.ToInt32(dc["empID"]),
                        nombre = dc["Nombres"]
                    };
                }, id).ToList();

                return list[0];
            }
            catch (Exception ex)
            {
                return null;
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
                        //TipoUsuario = Convert.ToInt32(dc["U_RML_TIPO_USUARIO"]),
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

        public ConsultationResponse<UsuarioPortal> Fn_UsuariosPortal()
        {
            var respOk = "OK";
            var respIncorrect = "No se encuentra Usuarios";

            SqlADOHelper hash = new SqlADOHelper();

            try
            {
                List<UsuarioPortal> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_usuariosPortal), dc =>
                {
                    return new UsuarioPortal
                    {
                        ID = Convert.ToInt32(dc["ID"]),
                        Nombres = dc["Nombres"],
                        Username = dc["RML_USERNAME"],
                        FechaRegistro = string.IsNullOrEmpty(dc["RML_FECHA_REGIRMLO"].ToString())
                            ? null
                            : Convert.ToDateTime(dc["RML_FECHA_REGIRMLO"]).ToString("dd/MM/yyyy"),
                        // RolID = dc["RML_ROL_ID"], 
                        Rol = new Complemento { id = dc["RML_ROL_ID"], name = dc["RML_NOMBRE_ROL"] },
                        Estado = dc["Estado"]
                    };
                }, string.Empty).ToList();

                return new ConsultationResponse<UsuarioPortal>
                {
                    CodRespuesta = list.Count() > 0 ? "00" : "22",
                    DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new ConsultationResponse<UsuarioPortal>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message,

                };
            }
        }
        public string Sb_ResetearContrasenia(int id)
        {
            try
            {
                SqlADOHelper hash = new SqlADOHelper();
                string query = hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_resetUsuario), id);

                return "00";  // Código de éxito
            }
            catch (Exception ex)
            {
                // Puedes registrar el error aquí si es necesario
                return "99";  // Código de error
            }
        }
        public ConsultationResponse<UsuarioInfo> Fn_UsuarioPortal(int id)
        {
            var respOk = "OK";
            var respIncorrect = "No se encuentra Usuarios";

            try
            {
                // Obtener lista de usuarios del portal
                UsuarioPortal listPortal = ObtenerUsuarioPortal(id);

                // Obtener lista de usuarios de SAP
                UsuarioSAP listSap = ObtenerUsuarioSAP(id);

                // Crear objeto UsuarioInfo con ambas listas
                List<UsuarioInfo> usuarioInfo = new List<UsuarioInfo>
                {
                    new UsuarioInfo { usuarioPortal = listPortal, usuarioSAP = listSap }
                };

                // Verificar si las listas contienen datos
                bool tieneDatos = usuarioInfo.Count > 0;

                return new ConsultationResponse<UsuarioInfo>
                {
                    CodRespuesta = tieneDatos ? "00" : "22",
                    DescRespuesta = tieneDatos ? respOk : respIncorrect,
                    Result = usuarioInfo
                };
            }
            catch (Exception ex)
            {
                return new ConsultationResponse<UsuarioInfo>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message
                };
            }
        }

        private UsuarioPortal ObtenerUsuarioPortal(int id)
        {
            SqlADOHelper hash = new SqlADOHelper();

            return hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_usuarioPortal), dc =>
            {
                return new UsuarioPortal
                {
                    ID = Convert.ToInt32(dc["ID"]),
                    //Nombres = dc["Nombres"].ToString(),
                    Username = dc["RML_USERNAME"].ToString(),
                    FechaRegistro = string.IsNullOrEmpty(dc["RML_FECHA_REGIRMLO"].ToString())
                            ? null
                            : Convert.ToDateTime(dc["RML_FECHA_REGIRMLO"]).ToString("dd/MM/yyyy"),
                    Rol = new Complemento { id = dc["RML_ROL_ID"], name = dc["RML_NOMBRE_ROL"] },
                    Estado = dc["Estado"].ToString()
                };
            }, id.ToString()).ToList().FirstOrDefault();
        }

        private UsuarioSAP ObtenerUsuarioSAP(int id)
        {
            SqlADOHelper hash = new SqlADOHelper();

            return hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_usuarioPortal), dc =>
            {
                return new UsuarioSAP
                {
                    EmpleadoId = Convert.ToInt32(dc["empID"]),
                    Nombre = dc["firstName"].ToString(),
                    Apellido = dc["lastName"].ToString(),
                    Cargo = dc["jobTitle"].ToString(),
                    Email = dc["email"].ToString(),
                    ProveedorAsoc = new Proveedor { CardCode = dc["U_CE_PVAS"], CardName = dc["U_CE_PVNM"], LicTradNum = dc["LicTradNum"] },
                    CodEar = dc["U_CE_CEAR"],
                    RendicionesMaxima = dc["U_CE_RNDC"]
                };
            }, id.ToString()).ToList().FirstOrDefault();
        }
        public ConsultationResponse<UsuarioInfo> Sb_ActualizUsuario(UsuarioInfo po_user)
        {
            var respOk = "OK";
            var respIncorrect = "No se encuentra Usuarios";

            try
            {

                // Actualizar en el portal
                bool portalActualizado = ActualizarUsuarioEnPortal(po_user.usuarioPortal);

                if (!portalActualizado)
                {
                    return new ConsultationResponse<UsuarioInfo>
                    {
                        CodRespuesta = "21",
                        DescRespuesta = "Error al actualizar el usuario en el portal"
                    };
                }

                // Actualizar en SAP
                bool sapActualizado = ActualizarUsuarioEnSAP(po_user.usuarioSAP);

                if (!sapActualizado)
                {
                    return new ConsultationResponse<UsuarioInfo>
                    {
                        CodRespuesta = "21",
                        DescRespuesta = "Error al actualizar el usuario en SAP"
                    };
                }

                // Retornar respuesta exitosa
                return new ConsultationResponse<UsuarioInfo>
                {
                    CodRespuesta = "00",
                    DescRespuesta = respOk,
                    Result = new List<UsuarioInfo>
                {
                    new UsuarioInfo { usuarioPortal = po_user.usuarioPortal, usuarioSAP = po_user.usuarioSAP }
                }
                };
            }
            catch (Exception ex)
            {
                return new ConsultationResponse<UsuarioInfo>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message
                };
            }
        }
        public ConsultationResponse<UsuarioInfo> Fn_CrearUsuarioEmpleado(UsuarioInfo po_user)
        {
            var respOk = "OK";
            var respIncorrect = "No se encuentra Usuarios";

            try
            {
                // Verificar si el nombre de usuario ya existe en el portal
                if (VerificarSiUsuarioExisteEnPortal(po_user.usuarioPortal.Username))
                {
                    return new ConsultationResponse<UsuarioInfo>
                    {
                        CodRespuesta = "22",
                        DescRespuesta = "El nombre de usuario ya existe en el portal"
                    };
                }

                UsuarioSAP sapActualizado = new UsuarioSAP();
                // Actualizar en SAP
                if (po_user.usuarioSAP?.EmpleadoId == 0)
                {
                    sapActualizado = CrearUsuarioEnSAP(po_user.usuarioSAP);

                    if (sapActualizado == null)
                    {
                        return new ConsultationResponse<UsuarioInfo>
                        {
                            CodRespuesta = "21",
                            DescRespuesta = "Error al crear el usuario en SAP"
                        };
                    }
                }
                else
                {
                    if (VerificarSiUsuarioDeSapExistePortal((int)po_user.usuarioSAP?.EmpleadoId))
                    {
                        return new ConsultationResponse<UsuarioInfo>
                        {
                            CodRespuesta = "22",
                            DescRespuesta = "El empleado ya fue registrado anteriormente en el portal"
                        };
                    }
                    sapActualizado = po_user.usuarioSAP;
                }
                // Actualizar en el portal
                UsuarioPortal portalActualizado = CrearUsuarioEnPortal(po_user.usuarioPortal, sapActualizado.EmpleadoId);

                if (portalActualizado == null)
                {
                    return new ConsultationResponse<UsuarioInfo>
                    {
                        CodRespuesta = "21",
                        DescRespuesta = "Error al actualizar el usuario en el portal"
                    };
                }
               
                // Retornar respuesta exitosa
                return new ConsultationResponse<UsuarioInfo>
                {
                    CodRespuesta = "00",
                    DescRespuesta = respOk,
                    Result = new List<UsuarioInfo>
                {
                    new UsuarioInfo { usuarioPortal = portalActualizado, usuarioSAP = sapActualizado }
                }
                };
            }
            catch (Exception ex)
            {
                return new ConsultationResponse<UsuarioInfo>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message
                };
            }
        }
        private bool VerificarSiUsuarioExisteEnPortal(string username)
        {
            try
            {
                SqlADOHelper hash = new SqlADOHelper();
                object result = hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.get_usuarioExiste), username);
            
                // Convertir el resultado a int
                int data = result != null ? Convert.ToInt32(result) : 0;

                return data > 0; // Si count > 0, el usuario ya existe
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar si el usuario existe en el portal.", ex);
            }
        }
        private bool VerificarSiUsuarioDeSapExistePortal(int empleadoID)
        {
            try
            {
                SqlADOHelper hash = new SqlADOHelper();
                object result = hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.get_usuarioExisteID), empleadoID.ToString());

                // Convertir el resultado a int
                int data = result != null ? Convert.ToInt32(result) : 0;

                return data > 0; // Si count > 0, el usuario ya existe
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar si el usuario existe en el portal.", ex);
            }
        }
        private bool ActualizarUsuarioEnSAP(UsuarioSAP usuarioSAP)
        {
            try
            {
                EmployeesInfo employe = new EmployeesInfo();
                employe.FirstName = usuarioSAP.Nombre;
                employe.LastName = usuarioSAP.Apellido;
                employe.JobTitle = usuarioSAP.Cargo;
                employe.eMail = usuarioSAP.Email;

                B1SLEndpoint sl = new B1SLEndpoint();
                string json = JsonConvert.SerializeObject(employe);
                IRestResponse response = sl.ActualizarEmpleado(json,usuarioSAP.EmpleadoId);

                if (response.IsSuccessful)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                throw;
                return false;
            }
        }
        private UsuarioSAP CrearUsuarioEnSAP(UsuarioSAP usuarioSAP)
        {
            try
            {
                // Crear un nuevo objeto EmployeesInfo y asignar valores
                EmployeesInfo employe = new EmployeesInfo
                {
                    FirstName = usuarioSAP.Nombre,
                    LastName = usuarioSAP.Apellido,
                    JobTitle = usuarioSAP.Cargo,
                    eMail = usuarioSAP.Email
                };

                // Serializar el objeto EmployeesInfo a JSON
                string json = JsonConvert.SerializeObject(employe);

                // Crear una instancia del servicio y enviar la solicitud
                B1SLEndpoint sl = new B1SLEndpoint();
                IRestResponse response = sl.CrearEmpleado(json);

                // Verificar si la solicitud fue exitosa
                if (!response.IsSuccessful)
                {
                    // Lanzar una excepción con el mensaje de error si la solicitud falló
                    throw new Exception($"Error al crear usuario en SAP: {response.StatusDescription}");
                }

                // Deserializar la respuesta del servicio a un objeto UsuarioSAP
                employe = JsonConvert.DeserializeObject<EmployeesInfo>(response.Content);

                usuarioSAP.CodEar = employe.U_CE_CEAR;
                usuarioSAP.EmpleadoId = employe.EmployeeID;
                return usuarioSAP; // Retornar el usuario creado
            }
            catch (Exception ex)
            {
                // Manejar la excepción de manera adecuada, por ejemplo, registrar el error
                // Aquí solo se lanza la excepción nuevamente para manejarla en un nivel superior
                throw new Exception("Ocurrió un error al intentar crear el usuario en SAP.", ex);
            }
        }
        private bool ActualizarUsuarioEnPortal(UsuarioPortal usuarioPortal)
        {
            try
            {
                SqlADOHelper hash = new SqlADOHelper();
                string query = hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_actualizUsuario),
                                                       usuarioPortal.ID,
                                                       usuarioPortal.Username,
                                                       usuarioPortal.Rol.id,
                                                       usuarioPortal.Estado == "1" ? 1 : 0);
                return true; // Retorna true si se afectó alguna fila
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private UsuarioPortal CrearUsuarioEnPortal(UsuarioPortal usuarioPortal, int empID)
        {
            try
            {
                // Crear una instancia de la clase SqlADOHelper para ejecutar la consulta
                SqlADOHelper hash = new SqlADOHelper();

                // Ejecutar la consulta SQL para crear el usuario en el portal y obtener el ID generado
                string idResult = hash.insertValueSql(
                    SQ_QueryManager.Generar(SQ_Query.post_creaUsuario),
                    usuarioPortal.Username,
                    empID,
                    usuarioPortal.Rol.id
                );

                // Asignar el ID generado al objeto UsuarioPortal
                usuarioPortal.ID = Convert.ToInt32(idResult);

                // Retornar el objeto UsuarioPortal con el ID asignado
                return usuarioPortal;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el usuario en el portal.", ex);
            }
        }
        public ConsultationResponse<Complemento> Fn_Roles()
        {
            var respOk = "OK";
            var respIncorrect = "No se encuentra ROLES";

            SqlADOHelper hash = new SqlADOHelper();

            try
            {
                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_rolesPortal), dc =>
                {
                    return new Complemento
                    {
                       id = dc["RML_IDROL"],
                       name = dc["RML_NOMBRE_ROL"]
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
