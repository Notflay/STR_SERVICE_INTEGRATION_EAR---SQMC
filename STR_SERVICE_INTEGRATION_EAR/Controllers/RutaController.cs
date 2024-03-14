using STR_SERVICE_INTEGRATION_EAR.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace STR_SERVICE_INTEGRATION_EAR.Controllers
{
    [RoutePrefix("api/ruta")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [TokenAuthorization]
    public class RutaController : ApiController
    {
        [Route]
        [HttpGet]
        public IHttpActionResult Get()
        {
            int campo = 0;
            campo = Global.ObtieneCampoTipoRuta();

            SQ_Complemento sQ_Complemento = new SQ_Complemento();
            var response = sQ_Complemento.ObtenerDesplegable(campo);

            return Ok(response);
        }
    }
}