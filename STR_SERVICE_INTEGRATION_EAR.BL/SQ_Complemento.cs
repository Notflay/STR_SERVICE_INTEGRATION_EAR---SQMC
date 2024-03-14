using STR_SERVICE_INTEGRATION_EAR.EL.Commons;
using STR_SERVICE_INTEGRATION_EAR.EL.Requests;
using STR_SERVICE_INTEGRATION_EAR.EL.Responses;
using STR_SERVICE_INTEGRATION_EAR.SQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using STR_SERVICE_INTEGRATION_EAR.BL;
using System.Web.Mvc;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Web.Http;
using OfficeOpenXml;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;

namespace STR_SERVICE_INTEGRATION_EAR.BL
{
    public class SQ_Complemento
    {
        public ConsultationResponse<Complemento> ObtenerEstados(string filtro)
        {
            var respOk = "OK";
            var respIncorrect = "No se encuentra Estados";
            HanaADOHelper hash = new HanaADOHelper();

            try
            {
                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_estados), dc =>
                {
                    return new Complemento
                    {
                        Id = Convert.ToInt32(dc["ID"]),
                        Nombre = dc["DESCRIPCION"]
                    };
                }, filtro).ToList();

                return new ConsultationResponse<Complemento>
                {
                    CodRespuesta = list.Count() > 0 ? "00" : "22",
                    DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new ConsultationResponse<Complemento>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message,

                };
            }
        }

        public ConsultationResponse<Complemento> ObtenerEstado(int id)
        {
            var respOk = "OK";
            var respIncorrect = "No se encuentra Estado";
            HanaADOHelper hash = new HanaADOHelper();
            try
            {
                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_estado), dc =>
                {
                    return new Complemento
                    {
                        Id = Convert.ToInt32(dc["ID"]),
                        Nombre = dc["DESCRIPCION"]
                    };
                }, id.ToString()).ToList();

                return new ConsultationResponse<Complemento>
                {
                    CodRespuesta = list.Count() > 0 ? "00" : "22",
                    DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new ConsultationResponse<Complemento>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message,

                };
            }
        }

        public ConsultationResponse<Complemento> ObtenerDesplegable(int id)
        {
            var respOk = "OK";
            var respIncorrect = "No se encuentra data con el Field ID " + id.ToString();
            HanaADOHelper hash = new HanaADOHelper();
            try
            {
                if (id == -99) throw new Exception("No se encuentra data con el Field ID " + id.ToString());

                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_comboTipos), dc =>
                {
                    return new Complemento
                    {
                        Id = Convert.ToInt32(dc["Id"]),
                        Nombre = dc["Nombre"],
                        Descripcion = dc["Descripcion"]
                    };
                }, id.ToString()).ToList();

                return new ConsultationResponse<Complemento>
                {
                    CodRespuesta = list.Count() > 0 ? "00" : "22",
                    DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new ConsultationResponse<Complemento>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message,

                };
            }
        }
        public ConsultationResponse<Complemento> ObtenerTpoDocumentos()
        {
            var respOk = "OK";
            var respIncorrect = "No se encontró tipo de documentos";
            HanaADOHelper hash = new HanaADOHelper();
            try
            {
                // if (id == "-99") throw new Exception("No se encuentra data con el Field ID " + id.ToString());

                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_tpoDocumentos), dc =>
                {
                    return new Complemento
                    {
                        id = dc["id"],
                        name = dc["name"],
                    };
                }, string.Empty).ToList();

                return new ConsultationResponse<Complemento>
                {
                    CodRespuesta = list.Count() > 0 ? "00" : "22",
                    DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new ConsultationResponse<Complemento>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message,

                };
            }
        }
        public ConsultationResponse<Complemento> ObtenerTpoDocumento(string id)
        {
            var respOk = "OK";
            var respIncorrect = "No se encontró tipo de documento";
            HanaADOHelper hash = new HanaADOHelper();
            try
            {
                // if (id == "-99") throw new Exception("No se encuentra data con el Field ID " + id.ToString());

                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_tpoDocumento), dc =>
                {
                    return new Complemento
                    {
                        id = dc["id"],
                        name = dc["name"],
                    };
                }, id).ToList();

                return new ConsultationResponse<Complemento>
                {
                    CodRespuesta = list.Count() > 0 ? "00" : "22",
                    DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new ConsultationResponse<Complemento>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message,

                };
            }
        }

        public ConsultationResponse<Complemento> ObtenerDesplegablePorId(string campo, string id)
        {
            var respOk = "OK";
            var respIncorrect = "No se encuentra data con el Field ID " + id.ToString();
            HanaADOHelper hash = new HanaADOHelper();
            try
            {
                if (id == "-99") throw new Exception("No se encuentra data con el Field ID " + id.ToString());

                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_comboTiposPorId), dc =>
                {
                    return new Complemento
                    {
                        Id = Convert.ToInt32(dc["Id"]),
                        Nombre = dc["Nombre"],
                        Descripcion = dc["Descripcion"]
                    };
                }, campo, id).ToList();

                return new ConsultationResponse<Complemento>
                {
                    CodRespuesta = list.Count() > 0 ? "00" : "22",
                    DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new ConsultationResponse<Complemento>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message,

                };
            }
        }
        public ConsultationResponse<Complemento> DescargarPlantilla()
        {
            var respOk = "OK";
            var respIncorrect = "No se encontró plantilla";
            List<Complemento> list = new List<Complemento>();

            HanaADOHelper hash = new HanaADOHelper();
            try
            {

                Plantilla plantilla = new Plantilla();
                //plantilla.ObtienePlantilla();


                return new ConsultationResponse<Complemento>
                {
                    CodRespuesta = list.Count() > 0 ? "00" : "22",
                    DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new ConsultationResponse<Complemento>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message,

                };
            }
        }

        public HttpResponseMessage DescargarPlantillaDefecto()
        {
            var respOk = "OK";
            var respIncorrect = "No se encontró plantilla";

            try
            {
                string ruta = ConfigurationManager.AppSettings["plantillaExcel"].ToString();
                string tipoMIME = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                var content = new StreamContent(File.OpenRead(ruta));
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = content
                };

                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(tipoMIME);
                content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = "PlantillaPortalEAR.xlsx"
                };

                return response;
            }
            catch (Exception ex)
            {
                // Manejar la excepción según tu lógica de negocio
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ConsultationResponse<Complemento>> UploadPlantillaAsync(HttpContent file, int id)
        {
            Plantilla sq_Plantilla = new Plantilla();
            Sq_Rendicion sq_Rendicion = new Sq_Rendicion();
            var respOk = "OK";
            var respIncorrect = "No se encontró plantilla";
            List<Complemento> list = new List<Complemento>();

            HanaADOHelper hash = new HanaADOHelper();
            try
            {

                var stream = await file.ReadAsStreamAsync();
                var package = new ExcelPackage(stream);
                var docs = sq_Plantilla.ObtienePlantilla(package, id);
                docs.ForEach((e) =>
                {
                    sq_Rendicion.CrearDocumento(e);
                });

                return new ConsultationResponse<Complemento>
                {
                    CodRespuesta = list.Count() > 0 ? "00" : "22",
                    DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                    Result = list
                };

            }
            catch (Exception ex)
            {
                return new ConsultationResponse<Complemento>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message,

                };
            }
        }

        public ConsultationResponse<Complemento> ObtienePresupuesto(PresupuestoRq presupuesto)
        {
            var respOk = "OK";
            var respIncorrect = "No se encontró presupuesto";


            HanaADOHelper hash = new HanaADOHelper();
            try
            {                                                                               // get_presupuesto
                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_presupuestoPrd), dc =>
                {

                    return new Complemento
                    {
                        id = "1",
                        name = dc["RESULT"]
                    };
                }, presupuesto.centCostos, presupuesto.posFinanciera, presupuesto.anio, presupuesto.precio.ToString("F2")).ToList();


                return new ConsultationResponse<Complemento>
                {
                    CodRespuesta = list.Count() > 0 ? "00" : "22",
                    DescRespuesta = list.Count() > 0 ? respOk : respIncorrect,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new ConsultationResponse<Complemento>
                {
                    CodRespuesta = "99",
                    DescRespuesta = ex.Message,

                };
            }

        }
    }
}
