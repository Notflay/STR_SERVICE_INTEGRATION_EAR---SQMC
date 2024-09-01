﻿using STR_SERVICE_INTEGRATION_EAR.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace STR_SERVICE_INTEGRATION_EAR.Controllers
{
    [RoutePrefix("api/tipoear")]
    
    //[TokenAuthorization]
    public class TipoController : ApiController
    {
        [Route]
        [HttpGet]
        [Authorize]
        public IHttpActionResult Get()
        {
            // Obtiene Campo ID

            int campo = 0;
            campo = Global.ObtieneCampoTipoEar();

            SQ_Complemento sQ_Complemento = new SQ_Complemento();
            var response = sQ_Complemento.ObtenerDesplegable(campo);

            return Ok(response);
        }

        [Route("documentos")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetDocumentos()
        {
            // Obtiene Campo ID


            SQ_Complemento sQ_Complemento = new SQ_Complemento();
            var response = sQ_Complemento.ObtenerTpoDocumentos();

            return Ok(response);
        }

        [Route("documentos/{id}")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetDocumentos(string id)
        {
            // Obtiene Campo ID


            SQ_Complemento sQ_Complemento = new SQ_Complemento();
            var response = sQ_Complemento.ObtenerTpoDocumentos();

            return Ok(response);
        }

        [Route("plantilla")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetPlantilla()
        {
            // Obtiene Campo ID


            SQ_Complemento sQ_Complemento = new SQ_Complemento();
            var response = sQ_Complemento.DescargarPlantilla();

            return Ok(response);
        }
    }
}