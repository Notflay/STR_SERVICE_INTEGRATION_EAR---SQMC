using STR_SERVICE_INTEGRATION_EAR.BL;
using STR_SERVICE_INTEGRATION_EAR.EL.Requests;
using STR_SERVICE_INTEGRATION_EAR.EL.Responses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace STR_SERVICE_INTEGRATION_EAR.Controllers
{
    [RoutePrefix("api/solicitud")]
    
   // [TokenAuthorization]
    public class SolicitudController : ApiController
    {
        [HttpGet]
        [Route("lista")]
        [Authorize]

        public IHttpActionResult Get(string usrCreate, string usrAsign, int perfil, string fecini, string fecfin, string nrrendi, string estados, string area)
        {


            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();
            var response = sq_SolicitudRd.ListarSolicitudesS(usrCreate, usrAsign, perfil, fecini, fecfin, nrrendi, estados, area);

            return Ok(response);
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult Post(SolicitudRD solicitudRD)
        {
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();
            var rsponse = sq_SolicitudRd.CreaSolicitudRd(solicitudRD);

            return Ok(rsponse);
        }

        [HttpPost]
        [Route("detalle")]
        [Authorize]
        public IHttpActionResult Post(SolicitudRDdet detalleSr)
        {
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();
            var rsponse = sq_SolicitudRd.CreaSolicitudSrDet(detalleSr);
            return Ok(rsponse);
        }


        [HttpGet]
        [Route("{id:int}")]
        [Authorize]
        public IHttpActionResult Get(int id, string create)
        {
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();
            var rsponse = sq_SolicitudRd.ObtenerSolicitud(id, create);
            return Ok(rsponse);
        }

        [HttpPatch]
        [Route("{id}")]
        [Authorize]
        public IHttpActionResult Update(int id, SolicitudRD solicitudRD)
        {
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();
            var response = sq_SolicitudRd.ActualizarSolicitudSr(id, solicitudRD);

            return Ok(response);
        }

        [HttpPatch]
        [Route("detalle/{id}")]
        [Authorize]
        public IHttpActionResult UpdateDetalle(int id, SolicitudRDdet solicitudRDdet)
        {
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();
            var response = sq_SolicitudRd.ActualizarSolicitudSrDet(id, solicitudRDdet);

            return Ok(response);
        }

        [HttpDelete]
        [Route("detalle/{id}")]
        [Authorize]
        public IHttpActionResult DeleteDetalle(int id)
        {
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();
            var response = sq_SolicitudRd.BorrarSolicitudSrDet(id);

            return Ok(response);
        }
        [HttpGet]
        [Route("detalle/{id}")]
        [Authorize]
        public IHttpActionResult ObtenerDetalle(int id)
        {
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();
            var response = sq_SolicitudRd.ObtenerSolicitudDet(id);

            return Ok(response);
        }
        [HttpPost]
        [Route("upload")]
        [Authorize]
        public async Task<IHttpActionResult> PostUpload()
        {
            // Verificar si la solicitud tiene el formato de carga de archivos
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest("Formato de contenido no admitido para carga de archivos.");
            }

            SQ_Configuracion sQ_Configuracion = new SQ_Configuracion();

            string ruta = ConfigurationManager.AppSettings["ruta"].ToString();
            //var response = sQ_Configuracion.getRutaAdjSAP();
            //string ruta = response.Result[0].Nombre;

            //string rutaMomentanea = "D:\\Chamba Backend\\Electro Peru\\Pruebas";

            var provider = new MultipartFormDataStreamProvider(ruta);

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                foreach (var file in provider.FileData)
                {
                    var fileName = file.Headers.ContentDisposition.FileName.Trim('"');
                    var filePath = Path.Combine(ruta, fileName);

                    var fileBytes = File.ReadAllBytes(file.LocalFileName);
                    File.WriteAllBytes(filePath, fileBytes);

                    File.Delete(file.LocalFileName);

                    CargaArchivo cargaArchivo = new CargaArchivo();
                    cargaArchivo.filePath = filePath;
                    cargaArchivo.codRespuesta = 00;
                    cargaArchivo.fileName = fileName;

                    return Ok(cargaArchivo);
                }
            }
            catch (System.Exception e)
            {
                return InternalServerError(e);
            }

            return BadRequest("No se han proporcionado archivos para cargar.");
        }

        [HttpPost]
        [Route("aprobacion/{id}")]
        [Authorize]
        public IHttpActionResult SolicitaAprobacion(int id, Rq_Aprobacion request)
        {
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();
            var response = sq_SolicitudRd.EnviarSolicitudAprobacion(id.ToString(), request.usuarioId, request.tipord, request.area, request.monto, request.estado);

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }

        [HttpPatch]
        [Route("aprobacion/acepta")]
        [Authorize]
        public IHttpActionResult AceptaSolicitud(int id, string aprobadorId, string areaAprobador)
        {
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();
            var response = sq_SolicitudRd.AceptarSolicitud(id, aprobadorId, areaAprobador);

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("valida/{id}")]
        [Authorize]
        public IHttpActionResult ValidaSolicitud(int id)
        {
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();
            var response = sq_SolicitudRd.ValidacionSolicitud(id);

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }

        [HttpPatch]
        [Route("aprobacion/rechazar")]
        [Authorize]
        public IHttpActionResult RechazarSolicitud(int id, string aprobadorId, Complemento comentarios, string areaAprobador)
        {
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();
            var response = sq_SolicitudRd.RechazarSolicitud(id.ToString(), aprobadorId, comentarios?.name, areaAprobador);

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }

        [HttpPatch]
        [Route("aprobacion/reintentar/{id}")]
        [Authorize]
        public IHttpActionResult ReintentarSR(int id)
        {
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();
            var response = sq_SolicitudRd.ReintentarSolicitud(id);

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("aprobadores")]
        [Authorize]
        public IHttpActionResult ObtieneAprobadores(string idSolicitud)
        {
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();
            var response = sq_SolicitudRd.ObtieneAprobadores(idSolicitud);

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("adjuntos/{id}")]
        [Authorize]
        public IHttpActionResult ObtieneAdjuntos(int id)
        {
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();
            var response = sq_SolicitudRd.ObtieneAdjuntos(id.ToString());

            if (response != null && response.CodRespuesta == "99")
            {
                return BadRequest(response.DescRespuesta);
            }

            return Ok(response);
        }
    }
}