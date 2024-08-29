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
using STR_SERVICE_INTEGRATION_EAR.EL.Requests;

namespace STR_SERVICE_INTEGRATION_EAR.Controllers
{
    [RoutePrefix("api/usuario")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsuarioController : ApiController
    {
        bool Prod = ConfigurationManager.AppSettings["Prod"].ToString() == "1";

        //[HttpGet]
        //[Route("{id}")]
        //public IHttpActionResult ObtieneUsuario(int id)
        //{
        //    SQ_Usuario consulta = new SQ_Usuario();
        //    var response = consulta.getUsuario(id);

        //    if (response.CodRespuesta == "99")
        //    {
        //        return BadRequest(response.DescRespuesta);
        //    }
        //    return Ok(response);
        //}
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
        [HttpGet]
        [Route("portal-pendientes")]
        public IHttpActionResult ObtenerUsuariosPendientes()
        {
            SQ_Usuario consulta = new SQ_Usuario();
            var response = consulta.getUsuariosPend();
            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }
        [HttpPut]
        public IHttpActionResult ActualizarUsuario(UsuarioInfo po_user)
        {
            SQ_Usuario consulta = new SQ_Usuario();
            var response = consulta.Sb_ActualizUsuario(po_user);
            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }
        [HttpPost]
        public IHttpActionResult CrearUsuario(UsuarioInfo po_user)
        {
            SQ_Usuario consulta = new SQ_Usuario();
            var response = consulta.Fn_CrearUsuarioEmpleado(po_user);
            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }
        [HttpGet]
        [Route("portal")]
        public IHttpActionResult ObtenerUsuariosPortal()
        {
            SQ_Usuario consulta = new SQ_Usuario();
            var response = consulta.Fn_UsuariosPortal();
            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }
        [HttpPut]
        [Route("resetear-contrasenia/{id:int}")]
        public IHttpActionResult ResetearContrasenia(int id)
        {
            SQ_Usuario consulta = new SQ_Usuario();
            var response = consulta.Sb_ResetearContrasenia(id);

            if (response == "99")
            {
                return BadRequest("Error al reiniciar la contraseña.");
            }

            return Ok();
        }
        [HttpGet]
        [Route("portal/{id}")]
        public IHttpActionResult ObtetenerUsuarioPortal(int id)
        {
            SQ_Usuario consulta = new SQ_Usuario();
            var response = consulta.Fn_UsuarioPortal(id);
            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }
        [HttpGet]
        [Route("roles")]
        public IHttpActionResult ObtenerUsuarioRoles()
        {
            SQ_Usuario consulta = new SQ_Usuario();
            var response = consulta.Fn_Roles();
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
        public async Task<IHttpActionResult> ObtieneSesion(EL.Requests.LoginRequest login)
        {
            SQ_Usuario sq = new SQ_Usuario();
            var response = sq.ObtieneSesion(login);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
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

