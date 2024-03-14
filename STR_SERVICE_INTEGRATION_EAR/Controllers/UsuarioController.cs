using STR_SERVICE_INTEGRATION_EAR.BL;
using System.Web.Http;
using System.Security.Principal;
using System.DirectoryServices.AccountManagement;
using System.Web.Http.Cors;
using STR_SERVICE_INTEGRATION_EAR.EL.Responses;
using System.Net.Http;
using System.Configuration;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace STR_SERVICE_INTEGRATION_EAR.Controllers
{
    [RoutePrefix("api/usuario")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsuarioController : ApiController
    {
        bool Prod = ConfigurationManager.AppSettings["Prod"].ToString() == "1";

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult ObtieneUsuario(int id)
        {
            SQ_Usuario consulta = new SQ_Usuario();
            var response = consulta.getUsuario(id);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }
        [HttpGet]
        public IHttpActionResult ObtenerUsuarios()
        {
            SQ_Usuario consulta = new SQ_Usuario();
            var response = consulta.getUsuarios();
            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }
        //[HttpPost]
        //[Route("login")]
        //public IHttpActionResult ObtieneSesion(EL.Requests.Login login)
        //{
        //    WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
        //    int empleado = Convert.ToInt32(ConfigurationManager.AppSettings["usuario"]);

        //    if (ValidateCredentials(login.usuario, login.pass))
        //    {
        //        string token = EncrypHelper.GenerarToken(login.usuario + login.pass);

        //        return Ok(new LoginResponse
        //        {
        //            correcto = true,
        //            empId = empleado, // 853 usuario // 848 aprobador 1 // 910 aprobador 2 // 1481 Administrador // 1474 Contable
        //            mensaje = "Logeado correctamente",
        //            token = token
        //        });
        //    }
        //    else
        //    {
        //        return Content(System.Net.HttpStatusCode.Unauthorized, new LoginResponse { correcto = false, mensaje = "Contraseña o Usuario incorrecto, intentar nuevamente" });
        //    }
        //}
        [HttpPost]
        [Route("login")]
        public async Task<IHttpActionResult> ObtieneSesion(EL.Requests.Login login)
        {
            SQ_Usuario sQ = new SQ_Usuario();
            if (Prod)
            {

                var response = await sQ.ValidaSesionAsync(login);

                if (response.CodRespuesta == "99")
                {
                    return BadRequest(response.DescRespuesta);
                }
                else
                {
                    // Continua validando información del usuario 

                    var body = response.Result[0];

                    var respo2 = sQ.getUsuario2(body.username);

                    if (respo2.CodRespuesta == "22")
                    {
                        return BadRequest(response.DescRespuesta);
                    }
                    else
                    {
                        string token = EncrypHelper.GenerarToken(login.username + login.password);
                        respo2.Token = token;

                        return Ok(respo2);
                    }
                }
            }
            else
            {
                // Usuario Basico          -- proveedor06   853
                // Usuario Autorizador     -- luis1         848 
                // Usuario Autorizador2    -- pedro1        910
                // Usuario Contable        -- roony1        1474
                // Usuario Admin           -- jasmin1       1481 

                var response = sQ.getUsuario2(login.username);

                if (response.CodRespuesta == "22")
                {
                    return BadRequest(response.DescRespuesta);
                }
                else
                {
                    string token = EncrypHelper.GenerarToken(login.username + login.password);
                    response.Token = token;

                    return Ok(response);
                }
            }
        }

        static bool ValidateCredentials(string username, string password)
        {
            using (PrincipalContext context = new PrincipalContext(ContextType.Machine))
            {
                return context.ValidateCredentials(username, password);
            }
        }

        [HttpGet]
        [Route("ceco/{id}")]
        public IHttpActionResult ObtieneCentroCosto(int id)
        {
            SQ_Usuario sq_consulta = new SQ_Usuario();
            var response = sq_consulta.getCentroCosto(id);
            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }
    }
}

