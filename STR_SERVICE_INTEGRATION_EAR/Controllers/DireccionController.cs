using STR_SERVICE_INTEGRATION_EAR.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace STR_SERVICE_INTEGRATION_EAR.Controllers
{
    [RoutePrefix("api/ubicacion")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [TokenAuthorization]
    public class DireccionController : ApiController
    {

        [HttpGet]
        [Route("departamento")]
        public IHttpActionResult ObtieneDepartamentos()
        {
            SQ_Ubicacion sQ_Ubicacion = new SQ_Ubicacion();
            var response = sQ_Ubicacion.ObtenerDepartamentos();
            return Ok(response);
        }
        [HttpGet]
        [Route("departamento/{id}")]
        public IHttpActionResult ObtieneDepartamento(int id)
        {
            SQ_Ubicacion sQ_Ubicacion = new SQ_Ubicacion();
            var response = sQ_Ubicacion.ObtenerDepartamento(id.ToString());
            return Ok(response);
        }
        [HttpGet]
        [Route("provincia/{departamento}")]
        public IHttpActionResult ObtieneProvincias(int departamento)
        {
            SQ_Ubicacion sQ_Ubicacion = new SQ_Ubicacion();
            var response = sQ_Ubicacion.ObtenerProvincias(departamento.ToString());
            return Ok(response);
        }
        [HttpGet]
        [Route("distrito/{provincia}")]
        public IHttpActionResult ObtieneDistritos(int provincia)
        {
            SQ_Ubicacion sQ_Ubicacion = new SQ_Ubicacion();
            var response = sQ_Ubicacion.ObtenerDistritos(provincia.ToString());
            return Ok(response);
        }
        [HttpGet]
        [Route("direccion/{ubigeo}")]
        public IHttpActionResult ObtieneDireccion(int ubigeo)
        {
            SQ_Ubicacion sQ_Ubicacion = new SQ_Ubicacion();
            var response = sQ_Ubicacion.ObtenerDireccion(ubigeo.ToString());
            return Ok(response);
        }
        [HttpGet]
        [Route("direccion/like")]
        public IHttpActionResult ObtenerDistritoPorLetra(string filtro, string letra)
        {
            SQ_Ubicacion sQ_Ubicacion = new SQ_Ubicacion();
            var response = sQ_Ubicacion.ObtenerUbigeoPorLetra(filtro,letra.ToUpper());
            return Ok(response);
        }
    }
}