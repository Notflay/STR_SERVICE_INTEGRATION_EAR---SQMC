using STR_SERVICE_INTEGRATION_EAR.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace STR_SERVICE_INTEGRATION_EAR.Controllers
{
    [RoutePrefix("api/estado")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    //[TokenAuthorization]
    public class EstadoController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get(int area,int tipoUsuario, int trans)
        {
            SQ_Complemento sQ_Estado = new SQ_Complemento();
            var response = sQ_Estado.ObtenerEstados(area, tipoUsuario, trans);
            return Ok(response);
        }
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            SQ_Complemento sQ_Estado = new SQ_Complemento();
            var response = sQ_Estado.ObtenerEstado(id);
            return Ok(response);
        }
    }
}