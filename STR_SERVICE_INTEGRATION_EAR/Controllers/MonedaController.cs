using STR_SERVICE_INTEGRATION_EAR.BL;
using STR_SERVICE_INTEGRATION_EAR.EL.Commons;
using STR_SERVICE_INTEGRATION_EAR.EL.Requests;
using STR_SERVICE_INTEGRATION_EAR.EL.Responses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace STR_SERVICE_INTEGRATION_EAR.Controllers
{
    [RoutePrefix("api/moneda")]
    
    //[TokenAuthorization]
    public class MonedaController : ApiController
    {
        [Route]
        [HttpGet]
        [Authorize]
        public IHttpActionResult Get()
        {
            SQ_Complemento sQ_Complemento = new SQ_Complemento();
            var response = sQ_Complemento.ObtenerMonedas();

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }
        [Route("wtliable")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetWliable(string tipo)
        {
            SQ_Complemento sQ_Complemento = new SQ_Complemento();
            var response = sQ_Complemento.ObtenerWtliable(tipo);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }
        [Route("tablair")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetRetencion()
        {
            SQ_Complemento sQ_Complemento = new SQ_Complemento();
            var response = sQ_Complemento.ObtieneDataTablaIR();

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }
    }
}