using System.Web.Http;
using System.Web.Http.Cors;
using STR_SERVICE_INTEGRATION_EAR.BL;
using STR_SERVICE_INTEGRATION_EAR.EL.Requests;

namespace STR_SERVICE_INTEGRATION_EAR.Controllers
{
    [RoutePrefix("api/items")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [TokenAuthorization]
    public class ObtenerController : ApiController
    {
        [Route]
        [HttpGet]
        public IHttpActionResult Get(string ear, string cta)
        {
            Sq_Item item = new Sq_Item();
            var response = item.ObtenerItems(ear, cta);
            return Ok(response);
        }

        [Route("listaCup")]
        [HttpGet]
        public IHttpActionResult Get(int ceco, int posFinanciera, int anio)
        {
            Sq_Item item = new Sq_Item();
            var response = item.ObtenerCUP(ceco, posFinanciera, anio);

            return Ok(response);
        }

        [Route("listarPrecio")]
        [HttpGet]
        public IHttpActionResult Get(string provincia, string distrito, string itemcode)
        {
            Sq_Item item = new Sq_Item();
            var response = item.ObtenerPrecio(provincia, distrito, itemcode);

            return Ok(response);
        }

        [Route("combo/{ItemCode}")]
        [HttpGet]
        public IHttpActionResult GetItem(string ItemCode)
        {

            Sq_Item sq_Item = new Sq_Item();
            var response = sq_Item.ObtenerItem(ItemCode);

            return Ok(response);
        }

        [Route("proyectos")]
        [HttpGet]
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