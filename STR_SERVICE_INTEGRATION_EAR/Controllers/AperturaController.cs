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
    [RoutePrefix("api/apertura")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AperturaController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Post(AperturaRequest request)
        {
            sQ_Apertura sQ_Apertura = new sQ_Apertura();
            var response = sQ_Apertura.CreaApertura(request);
            return Ok(response);
        }
    }
}