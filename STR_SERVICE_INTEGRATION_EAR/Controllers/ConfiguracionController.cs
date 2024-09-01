using STR_SERVICE_INTEGRATION_EAR.BL;
using STR_SERVICE_INTEGRATION_EAR.EL.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace STR_SERVICE_INTEGRATION_EAR.Controllers
{
    [RoutePrefix("api/configuracion")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ConfiguracionController : ApiController
    {
        [HttpGet]
        [Route("cfgeneral")]
        [Authorize]
        public IHttpActionResult CreaCFGeneral()
        {
            // Valida si hay alguna configuración con el mismo codigo 
            SQ_Configuracion consulta = new SQ_Configuracion();
            var response = consulta.getCFGeneral();
            //response
            // var response = "";
            return Ok(response);
        }
    }
}