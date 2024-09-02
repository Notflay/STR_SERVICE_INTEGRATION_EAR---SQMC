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
    [RoutePrefix("api/proveedor")]
    
    //[TokenAuthorization]
    public class ProveedorController : ApiController
    {
        [Route]
        [HttpGet]
        [Authorize]
        public IHttpActionResult Get()
        {
            SQ_Proveedor sQ_Proveedor = new SQ_Proveedor();
            var response = sQ_Proveedor.ObtenerProveedores();

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }
        [Route("empleados")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetProveedorEmpleados()
        {
            SQ_Proveedor sQ_Proveedor = new SQ_Proveedor();
            var response = sQ_Proveedor.ObtenerProveedorEmp();

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }
        [Route("ruc/{id}")]
        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> GetAsync(string id)
        {
            SQ_Proveedor sQ_Proveedor = new SQ_Proveedor();
            var response = await sQ_Proveedor.ObtenerInfoPorRuc(id);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }

        [Route("comprobante")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult GetComprobante(ComprobanteRequest comprobante)
        {
            Sv_Sunat sV_sunat = new Sv_Sunat();
            var response = sV_sunat.ObtenerComprobante(comprobante);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }
    }
}