using System.Web.Http;
using System.Web.Http.Cors;
using STR_SERVICE_INTEGRATION_EAR.BL;
using STR_SERVICE_INTEGRATION_EAR.EL.Requests;

namespace STR_SERVICE_INTEGRATION_EAR.Controllers
{
    [RoutePrefix("api/items")]
    
    // [TokenAuthorization]
    public class ObtenerController : ApiController
    {
        [Route]
        [HttpGet]
        [Authorize]
        public IHttpActionResult Get(string tipo)
        {
            Sq_Item item = new Sq_Item();
            var response = item.ObtenerItems(tipo);
            return Ok(response);
        }
        [Route("ceco")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetCeco(string code)
        {
            SQ_Complemento item = new SQ_Complemento();
            var response = item.ObtenerCECO(code);
            return Ok(response);
        }
        [Route("operacion")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetOperacion()
        {
            SQ_Complemento item = new SQ_Complemento();
            var response = item.ObtenerTipoOperacion();
            return Ok(response);
        }
        [Route("cuentas")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetCuentas()
        {
            Sq_Item item = new Sq_Item();
            var response = item.ObtenerCuentasContable();
            return Ok(response);
        }
        [Route("almacenes")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetAlmacenes()
        {
            Sq_Item item = new Sq_Item();
            var response = item.ObtenerAlmacenes();
            return Ok(response);
        }

        [Route("{id}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetUnico(string id)
        {
            Sq_Item item = new Sq_Item();
            var response = item.ObtenerItem(id);
            return Ok(response);
        }

        [Route("listaCup")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult Get(int ceco, int posFinanciera, int anio)
        {
            Sq_Item item = new Sq_Item();
            var response = item.ObtenerCUP(ceco, posFinanciera, anio);

            return Ok(response);
        }

        [Route("listarPrecio")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult Get(string provincia, string distrito, string itemcode)
        {
            Sq_Item item = new Sq_Item();
            var response = item.ObtenerPrecio(provincia, distrito, itemcode);

            return Ok(response);
        }

        [Route("combo/{ItemCode}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetItem(string ItemCode)
        {

            Sq_Item sq_Item = new Sq_Item();
            var response = sq_Item.ObtenerItem(ItemCode);

            return Ok(response);
        }

        [Route("proyectos")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetProyectos()
        {
            Sq_Item sq_Item = new Sq_Item();
            var response = sq_Item.ObtenerProyectos();

            if (response.CodRespuesta == "99")
                return BadRequest(response.DescRespuesta);

            return Ok(response);
        }

        [Route("indicadores")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetIndicadores()
        {
            Sq_Item sq_Item = new Sq_Item();
            var response = sq_Item.ObtenerIndicadores();

            if (response.CodRespuesta == "99")
                return BadRequest(response.DescRespuesta);

            return Ok(response);
        }

        [Route("presupuesto")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetPresupuesto(string ceco, string posf, string anio, decimal prec)
        {
            SQ_Complemento sq = new SQ_Complemento();
            var response = sq.ObtienePresupuesto(new PresupuestoRq { centCostos = ceco, posFinanciera = posf, anio = anio, precio = prec } );

            if (response.CodRespuesta == "99")
                return BadRequest(response.DescRespuesta);

            return Ok(response);
        }
    }
}