using STR_SERVICE_INTEGRATION_EAR.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Net.Http;
using System.Net;

namespace STR_SERVICE_INTEGRATION_EAR.Controllers
{
    [RoutePrefix("api/reporte")]
    
    [TokenAuthorization]
    public class ReporteController : ApiController
    {
        [HttpPost]
        [Route]
        [Authorize]
        public HttpResponseMessage Post(string id, string numRendicion, string tipo)
        {
            Sq_Reporte sq_Reporte = new Sq_Reporte();
            var response = sq_Reporte.CreaReporte(id, numRendicion, tipo);

            return response;
        }

        [HttpPost]
        [Route]
        [Authorize]
        public HttpResponseMessage PostEAR(string numRendicion)
        {
            Sq_Reporte sq_Reporte = new Sq_Reporte();
            var response = sq_Reporte.CrearReporteEAR(numRendicion);

            return response;
        }
    }
}
