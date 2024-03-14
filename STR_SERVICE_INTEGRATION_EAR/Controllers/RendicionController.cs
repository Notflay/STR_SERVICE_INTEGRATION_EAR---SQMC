using STR_SERVICE_INTEGRATION_EAR.BL;
using STR_SERVICE_INTEGRATION_EAR.EL.Requests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Xml.Linq;
using System.Net;
using STR_SERVICE_INTEGRATION_EAR.EL.Responses;


namespace STR_SERVICE_INTEGRATION_EAR.Controllers
{
    [RoutePrefix("api/rendicion")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [TokenAuthorization]
    public class RendicionController : ApiController
    {

        [HttpGet]
        [Route("lista")]
        public IHttpActionResult Get(string usrCreate, string usrAsign, int perfil, string fecini, string fecfin, string nrrendi, string estados, string area)
        {

            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.ListarSolicitudesS(usrCreate, usrAsign, perfil, fecini, fecfin, nrrendi, estados, area);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {

            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.ObtenerRendicion(id);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("documento/{id}")]
        public IHttpActionResult GetDocumento(string id)
        {

            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.ObtenerDocumento(id);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("documento")]
        public IHttpActionResult Post(Documento documento)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.CrearDocumento(documento);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);

        }

        [HttpPatch]
        [Route]
        public IHttpActionResult Update(Rendicion rendicion)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.ActualizarRendicion(rendicion);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("aprobacion/{id}")]
        public IHttpActionResult CreateAprobacion(int id, string idSolicitud, int usuarioId, int estado, string areaAprobador)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.EnviarAprobacion(id.ToString(), idSolicitud, usuarioId, estado, areaAprobador);

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }
        [HttpPatch]
        [Route("aprobacion/acepta")]
        public IHttpActionResult AceptaSolicitud(int solicitudId, string aprobadorId, string areaAprobador, int estado, int rendicionId, int area)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.AceptarAprobacion(solicitudId, aprobadorId, areaAprobador, estado, rendicionId, area);

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }
        [HttpPatch]
        [Route("documento")]
        public IHttpActionResult Update(Documento documento)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.ActualizarDocumento(documento);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }

        [HttpPatch]
        [Route("documento/snt/{id}")]
        public IHttpActionResult Updatesnt(int id, string estado)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.ActualizarSntDocumento(id, estado);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }

        [HttpDelete]
        [Route("documento")]
        public IHttpActionResult Delete(int id, int rdId)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.BorrarDocumento(id, rdId);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }
        [HttpPost]
        [Route("documento/validacion/{id}")]
        public IHttpActionResult Validacion(int id)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.ValidacionDocumento(id);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }

        [HttpDelete]
        [Route("documento/detalle")]
        public IHttpActionResult DeleteDet(int id, int docId)
        {
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var response = sq_Rendicion.BorrarDetalleDcoumento(id, docId);

            if (response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("documento/plantilla")]
        public HttpResponseMessage DescargarPlantilla()
        {
            SQ_Complemento sQ_Complemento = new SQ_Complemento();
            var response = sQ_Complemento.DescargarPlantillaDefecto();

            return response;
        }

        [HttpPost]
        [Route("documento/plantilla/{id}")]
        public async Task<IHttpActionResult> PostUpload(int id)
        {
            SQ_Complemento sQ_Complemento = new SQ_Complemento();
            // Verificar si hay algún archivo en la solicitud
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest("La solicitud no contiene un archivo.");
            }

            var provider = new MultipartMemoryStreamProvider();

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                if (provider.Contents.Count == 0)
                {
                    return BadRequest("No se proporcionó ningún archivo.");
                }

                var response = sQ_Complemento.UploadPlantillaAsync(provider.Contents[0], id);

                return Ok(response);
            }
            catch (Exception)
            {

                return BadRequest("No se proporcionó ningún archivo.");
            }
        }

        [HttpPatch]
        [Route("aprobacion/reintentar/{id}")]
        public IHttpActionResult ReintentarSR(int id)
        {
            Sq_Rendicion sq_SolicitudRd = new Sq_Rendicion();
            var response = sq_SolicitudRd.ReintentarRendicion(id.ToString());

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("aprobadores")]
        public IHttpActionResult ObtieneAprobadores(int idRendicion)
        {
            Sq_Rendicion sq_SolicitudRd = new Sq_Rendicion();
            var response = sq_SolicitudRd.ObtieneAprobadores(idRendicion.ToString());

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }

        [HttpPatch]
        [Route("aprobacion/rechazar")]
        public IHttpActionResult RechazarSolicitud(int solicitudId, string aprobadorId, string areaAprobador, int estado, int rendicionId, int area)
        {
            Sq_Rendicion sq_SolicitudRd = new Sq_Rendicion();
            var response = sq_SolicitudRd.RechazarRendicion(solicitudId, aprobadorId, areaAprobador, estado, rendicionId, area);

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("adjuntos/{id}")]
        public IHttpActionResult ObtieneAdjuntos(int id)
        {
            Sq_Rendicion sq_SolicitudRd = new Sq_Rendicion();
            var response = sq_SolicitudRd.ObtieneAdjuntos(id.ToString());

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }
    }
}