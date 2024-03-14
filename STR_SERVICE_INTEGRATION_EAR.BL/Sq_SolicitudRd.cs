using RestSharp;
using STR_SERVICE_INTEGRATION_EAR.EL.Commons;
using STR_SERVICE_INTEGRATION_EAR.EL.Requests;
using STR_SERVICE_INTEGRATION_EAR.EL.Responses;
using STR_SERVICE_INTEGRATION_EAR.SL;
using STR_SERVICE_INTEGRATION_EAR.SQ;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Newtonsoft.Json;
using System.Globalization;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using STR_SERVICE_INTEGRATION_EAR.EL;
using System.IO;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Web.WebSockets;
using DocumentFormat.OpenXml.Office2010.Excel;
using CrystalDecisions.CrystalReports.Engine;
using System.Web;

namespace STR_SERVICE_INTEGRATION_EAR.BL
{
    public class Sq_SolicitudRd
    {
        HanaADOHelper hash = new HanaADOHelper();
        public ConsultationResponse<Complemento> CreaSolicitudRd(SolicitudRD solicitudRD)
        {
            var respIncorrect = "Solicitud de Detalle";
            try
            {

                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.post_insertSR), solicitudRD.STR_EMPLDREGI, solicitudRD.STR_NRSOLICITUD, solicitudRD.STR_NRRENDICION, solicitudRD.STR_ESTADO, solicitudRD.STR_EMPLDASIG,
                solicitudRD.STR_FECHAREGIS, solicitudRD.STR_UBIGEO, solicitudRD.STR_RUTA, solicitudRD.STR_RUTAANEXO, solicitudRD.STR_MOTIVO, solicitudRD.STR_FECHAINI,
                solicitudRD.STR_FECHAFIN, solicitudRD.STR_FECHAVENC, solicitudRD.STR_MONEDA, solicitudRD.STR_TIPORENDICION, solicitudRD.STR_IDAPROBACION,
                solicitudRD.STR_TOTALSOLICITADO, solicitudRD.STR_MOTIVOMIGR, solicitudRD.STR_DOCENTRY, solicitudRD.STR_ORDENVIAJE, solicitudRD.STR_AREA, solicitudRD.STR_NOMBRES);

                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_idSR), dc =>
                {
                    return new Complemento()
                    {
                        Id = Convert.ToInt32(dc["Id"]),
                        Nombre = dc["Id"],
                    };
                }, solicitudRD.STR_EMPLDREGI.ToString()).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }

            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }

        public ConsultationResponse<FileRS> ObtieneAdjuntos(string idSolicitud)
        {
            var respIncorrect = "No tiene adjuntos";
            List<FileRS> files = null;
            List<string> adjuntos = null;
            try
            {
                adjuntos = new List<string>();

                adjuntos = hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.get_adjuntos), idSolicitud).Split(',').ToList();

                files = new List<FileRS>();
                adjuntos.ForEach((e) =>
                {
                    FileInfo fileInfo = new FileInfo(e);

                    if (fileInfo.Exists)
                    {
                        files.Add(
                            new FileRS
                            {
                                name = fileInfo.Name,
                                size = fileInfo.Length, // Tamaño en bytes
                                type = GetMimeType(e), // Tipo de archivo (extensión),
                                status = "Cargado",
                                data = /*EsTipoDeImagen(GetMimeType(e)) ?*/ File.ReadAllBytes(e) //: null
                            }
                            );

                    };
                });
                return Global.ReturnOk(files, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<FileRS>(ex);
            }
        }

        public static bool EsTipoDeImagen(string mimeType)
        {
            // Puedes ajustar esta lógica según los tipos MIME que consideres como imágenes
            return mimeType.StartsWith("image/");
        }
        static string GetMimeType(string filePath)
        {
            string mimeType = null;

            try
            {
                // Usa MimeMapping.GetMimeMapping para obtener el tipo MIME basado en la extensión del archivo
                mimeType = MimeMapping.GetMimeMapping(Path.GetFileName(filePath));
            }
            catch (Exception)
            {
                // Maneja cualquier error al obtener el tipo MIME
            }

            return mimeType;
        }

        public ConsultationResponse<Complemento> CreaSolicitudSrDet(SolicitudRDdet detalle)
        {
            var respIncorrect = "No se completo la creación de la solicitud RD";
            try
            {

                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.post_insertSrDet), detalle.articulo?.id, detalle.articulo?.name, detalle.precioTotal, detalle.cantidad, detalle.posFinanciera?.name, detalle.cup?.U_CUP, detalle.SR_ID, detalle.ceco, detalle.ctc);

                string id = hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.get_idSrDet), string.Empty);

                /*
                if (detalle.centCostos.Count > 0)
                {
                    for (int i = 0; i < detalle.centCostos.Count; i++)
                    {
                        hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.post_insertSrDetCentC), detalle.centCostos[i].name, id);
                    }
                }*/

                List<Complemento> list = new List<Complemento>() {
                    new Complemento() { id = id }
                };

                return Global.ReturnOk(list, respIncorrect);

            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }
        public ConsultationResponse<Complemento> ActualizarSolicitudSrDet(int id, SolicitudRDdet detalle)
        {
            var respIncorrect = "No se completo la actualización de la solicitud";
            try
            {
                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_idSRDet), detalle.articulo.id, detalle.articulo.name, detalle.precioTotal, detalle.cantidad, detalle.posFinanciera.name, detalle.cup?.U_CUP, detalle.ceco, id);

                List<Complemento> list = new List<Complemento>();
                Complemento complemento = new Complemento()
                {
                    id = id.ToString(),
                    Descripcion = "Actualizado exitosamente",
                };

                list.Add(complemento);

                return Global.ReturnOk(list, respIncorrect);

            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }

        }
        public ConsultationResponse<Complemento> CreaSolicitudSrDetCent(CentroCosto centCosto)
        {
            var respIncorrect = "No se completo la creación de la solicitud RD";
            try
            {

                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.post_insertSrDetCentC), centCosto.id, centCosto.name);

                List<Complemento> list = new List<Complemento>();


                return Global.ReturnOk(list, respIncorrect);

            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }
        public ConsultationResponse<Complemento> ActualizarSolicitudSr(int id, SolicitudRD solicitudRD)
        {
            var respIncorrect = "No se completo la actualización de Sr";
            try
            {
                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_idSR), solicitudRD.STR_EMPLDREGI, solicitudRD.STR_NRSOLICITUD, solicitudRD.STR_NRRENDICION, solicitudRD.STR_ESTADO, solicitudRD.STR_EMPLDASIG,
                    solicitudRD.STR_FECHAREGIS, solicitudRD.STR_UBIGEO, solicitudRD.STR_RUTA, solicitudRD.STR_RUTAANEXO, solicitudRD.STR_MOTIVO, solicitudRD.STR_FECHAINI,
                    solicitudRD.STR_FECHAFIN, solicitudRD.STR_FECHAVENC, solicitudRD.STR_MONEDA, solicitudRD.STR_TIPORENDICION, solicitudRD.STR_IDAPROBACION,
                    solicitudRD.STR_TOTALSOLICITADO, solicitudRD.STR_MOTIVOMIGR, solicitudRD.STR_DOCENTRY, solicitudRD.STR_ORDENVIAJE, id);

                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_idSR), dc =>
                {
                    return new Complemento()
                    {
                        Id = Convert.ToInt32(dc["Id"]),
                        Nombre = dc["Id"],
                    };
                }, solicitudRD.STR_EMPLDREGI.ToString()).ToList();

                return Global.ReturnOk(list, respIncorrect);

            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }
        public ConsultationResponse<Complemento> BorrarSolicitudSrDet(int id)
        {
            var respIncorrect = "No se termino de eliminar el detalle de la solicitud";
            try
            {
                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.dlt_centrodeCostoPorItem), id);

                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.dlt_solicitudRdDetalle), id);


                List<Complemento> list = new List<Complemento>();
                Complemento complemento = new Complemento()
                {
                    id = id.ToString(),
                    Descripcion = "Actualizado exitosamente",
                };

                list.Add(complemento);

                return Global.ReturnOk(list, respIncorrect);

            }
            catch (Exception ex)
            {

                return Global.ReturnError<Complemento>(ex);
            }
        }
        public ConsultationResponse<SolicitudRD> ListarSolicitudesS(string usrCreate, string usrAsig, int perfil, string fecIni, string fecFin, string nrRendi, string estado, string area)
        {
            var respIncorrect = "No trajo la lista de solicitudes de rendición";

            try
            {
                List<SolicitudRD> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_listaSolicitudes), dc =>
                {
                    return new SolicitudRD()
                    {
                        ID = string.IsNullOrWhiteSpace(Convert.ToString(dc["ID"])) ? (int?)null : Convert.ToInt32(dc["ID"]),
                        STR_DOCENTRY = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_DOCENTRY"])) ? (int?)null : Convert.ToInt32(dc["STR_DOCENTRY"]),
                        STR_NRSOLICITUD = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_NRSOLICITUD"])) ? (int?)null : Convert.ToInt32(dc["STR_NRSOLICITUD"]),
                        STR_ESTADO = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_ESTADO"])) ? (int?)null : Convert.ToInt32(dc["STR_ESTADO"]),
                        STR_ESTADO_INFO = dc["STR_ESTADO_INFO"],
                        STR_MOTIVO = dc["STR_MOTIVO"],
                        STR_UBIGEO = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_UBIGEO"])) ? (int?)null : Convert.ToInt32(dc["STR_UBIGEO"]),
                        STR_RUTA = dc["STR_RUTA"],
                        STR_RUTAANEXO = dc["STR_RUTAANEXO"],
                        STR_MONEDA = dc["STR_MONEDA"],
                        STR_TIPORENDICION = dc["STR_TIPORENDICION"],
                        STR_TOTALSOLICITADO = Convert.ToDouble(dc["STR_TOTALSOLICITADO"]),
                        STR_MOTIVOMIGR = dc["STR_MOTIVOMIGR"],
                        STR_EMPLDASIG = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_EMPLDASIG"])) ? (int?)null : Convert.ToInt32(dc["STR_EMPLDASIG"]),
                        STR_EMPLDREGI = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_EMPLDREGI"])) ? (int?)null : Convert.ToInt32(dc["STR_EMPLDREGI"]),
                        STR_FECHAREGIS = string.IsNullOrWhiteSpace(dc["STR_FECHAREGIS"]) ? "" : Convert.ToDateTime(dc["STR_FECHAREGIS"]).ToString("dd/MM/yyyy"),
                        STR_FECHAINI = string.IsNullOrWhiteSpace(dc["STR_FECHAINI"]) ? "" : Convert.ToDateTime(dc["STR_FECHAINI"]).ToString("dd/MM/yyyy"),
                        STR_FECHAFIN = string.IsNullOrWhiteSpace(dc["STR_FECHAFIN"]) ? "" : Convert.ToDateTime(dc["STR_FECHAFIN"]).ToString("dd/MM/yyyy"),
                        STR_FECHAVENC = string.IsNullOrWhiteSpace(dc["STR_FECHAVENC"]) ? "" : Convert.ToDateTime(dc["STR_FECHAVENC"]).ToString("dd/MM/yyyy"),
                        STR_NRRENDICION = dc["STR_NRRENDICION"],
                        CREATE = dc["CREATE"]
                    };
                }, usrCreate, usrAsig, perfil.ToString(), fecIni, fecFin, nrRendi, estado, area).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<SolicitudRD>(ex);
            }
        }
        public ConsultationResponse<Complemento> ListaCentrosCostoPorSrDet(int detalleId)
        {

            var respIncorrect = "Lista de Centro de costo";
            try
            {

                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_centrodeCostoPorItem), dc =>
                {
                    return new Complemento()
                    {
                        Id = Convert.ToInt32(dc["STR_CENT_COSTO"]),
                        Nombre = dc["STR_CENT_COSTO"],
                        Descripcion = dc["STR_DET_ID"]
                    };
                }, detalleId.ToString()).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }

        public Cup obtenerCup(string cup)
        {
            List<Cup> listaCup = new List<Cup>();

            listaCup = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_obtieneCup), dc =>
            {
                return new Cup()
                {
                    CRP = Convert.ToInt32(dc["CRP"]),
                    Partida = Convert.ToInt32(dc["Partida"]),
                    U_CUP = dc["U_CUP"],
                    U_DESCRIPTION = dc["U_DESCRIPTION"]
                };
            }, cup).ToList();

            return listaCup[0];
        }

        public ConsultationResponse<SolicitudRDdet> ObtenerSolicitudDet(int id)
        {
            var respIncorrect = "No se obtuvo detalle";
            List<SolicitudRDdet> listDet = null;
            Sq_Item sq_item = new Sq_Item();
            try
            {

                listDet = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_solicitudRendicionDet), dc =>
                {
                    /*
                    List<Complemento> listDetCentC = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_centrodeCostoPorItem), sc =>
                    {
                        return new Complemento()
                        {
                            id = sc["ID"],
                            name = sc["STR_CENTCOSTO"],
                            Descripcion = sc["STR_DET_ID"]
                        };
                    }, dc["ID"]).ToList();
                    */
                    /*
                    List<Cup> listaCUP = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_obtieneCup), dc =>
                    {
                        return new Cup()
                        {
                            CRP = Convert.ToInt32(dc["CRP"]),
                            Partida = Convert.ToInt32(dc["Partida"]),
                            U_CUP = dc["U_CUP"],
                            U_DESCRIPTION = dc["U_DESCRIPTION"]
                        };
                    }, dc["STR_CUP"]).ToList();
                    */
                    return new SolicitudRDdet()
                    {
                        id = Convert.ToInt32(dc["ID"]),
                        ID = Convert.ToInt32(dc["ID"]),
                        STR_CANTIDAD = Convert.ToInt32(dc["STR_CANTIDAD"]),
                        cantidad = Convert.ToInt32(dc["STR_CANTIDAD"]),
                        STR_CONCEPTO = dc["STR_CONCEPTO"],
                        STR_CODARTICULO = dc["STR_CODARTICULO"],
                        STR_CUP = dc["STR_CUP"],
                        STR_POSFINAN = dc["STR_POSFINAN"],
                        STR_TOTAL = Convert.ToDouble(dc["STR_TOTAL"]),
                        SR_ID = Convert.ToInt32(dc["SR_ID"]),
                        articulo = sq_item.ObtenerItem(dc["STR_CODARTICULO"]).Result[0],
                        ceco = dc["STR_CECO"],
                        cup = string.IsNullOrEmpty(dc["STR_CUP"]) ? null : obtenerCup(dc["STR_CUP"]),
                        posFinanciera = new Complemento { id = dc["STR_POSFINAN"], name = dc["STR_POSFINAN"] },
                        precioUnitario = Convert.ToDouble(dc["STR_TOTAL"]) / Convert.ToDouble(dc["STR_CANTIDAD"]),
                        precioTotal = Convert.ToDouble(dc["STR_TOTAL"]),

                    };
                }, id.ToString()).ToList();

                return Global.ReturnOk(listDet, respIncorrect);
            }

            catch (Exception ex)
            {
                return Global.ReturnError<SolicitudRDdet>(ex);
            }
        }
        public ConsultationResponse<SolicitudRD> ObtenerSolicitud(int id, string create, bool masDetalle = true)
        {
            var respIncorrect = "No trajo la la solicitud de rendición";

            try
            {

                SQ_Ubicacion sQ_Ubicacion = new SQ_Ubicacion();
                Sq_Item sq_item = new Sq_Item();
                List<SolicitudRDdet> listDet = null;
                if (masDetalle)
                {


                    listDet = hash.GetResultAsType(SQ_QueryManager.Generar(create == "PWB" ? SQ_Query.get_solicitudRendicionDet : SQ_Query.get_solicitudRendicionDetSAP), dc =>
                    {
                        /*
                        List<Complemento> listDetCentC = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_centrodeCostoPorItem), sc =>
                        {
                            return new Complemento()
                            {
                                id = sc["ID"],
                                name = sc["STR_CENTCOSTO"],
                                Descripcion = sc["STR_DET_ID"]
                            };
                        }, dc["ID"]).ToList();
                        */
                        /*
                        List<Cup> listaCUP = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_obtieneCup), dc =>
                        {
                            return new Cup()
                            {
                                CRP = Convert.ToInt32(dc["CRP"]),
                                Partida = Convert.ToInt32(dc["Partida"]),
                                U_CUP = dc["U_CUP"],
                                U_DESCRIPTION = dc["U_DESCRIPTION"]
                            };
                        }, dc["STR_CUP"]).ToList();
                        */

                        return new SolicitudRDdet()
                        {
                            id = Convert.ToInt32(dc["ID"]),
                            ID = Convert.ToInt32(dc["ID"]),
                            STR_CANTIDAD = Convert.ToInt32(dc["STR_CANTIDAD"]),
                            cantidad = Convert.ToInt32(dc["STR_CANTIDAD"]),
                            STR_CONCEPTO = dc["STR_CONCEPTO"],
                            STR_CODARTICULO = dc["STR_CODARTICULO"],
                            STR_CUP = dc["STR_CUP"],
                            STR_POSFINAN = dc["STR_POSFINAN"],
                            STR_TOTAL = Convert.ToDouble(dc["STR_TOTAL"]),
                            SR_ID = Convert.ToInt32(dc["SR_ID"]),
                            articulo = sq_item.ObtenerItem(dc["STR_CODARTICULO"]).Result[0],
                            ceco = dc["STR_CECO"],
                            cup = string.IsNullOrEmpty(dc["STR_CUP"]) ? null : obtenerCup(dc["STR_CUP"]),
                            posFinanciera = new Complemento { id = dc["STR_POSFINAN"], name = dc["STR_POSFINAN"] },
                            precioUnitario = Convert.ToDouble(dc["STR_TOTAL"]) / Convert.ToDouble(dc["STR_CANTIDAD"]),
                            precioTotal = Convert.ToDouble(dc["STR_TOTAL"]),
                            ctc = dc["STR_CTC"]

                        };
                    }, id.ToString()).ToList(); // DocEntry
                }



                // Obtiene Ruta
                SQ_Complemento sQ_Complemento = new SQ_Complemento();
                int campo = 0;
                campo = ObtieneCampoTipoRuta();

                // Obitiene EAR
                int campEar = 0;
                campEar = ObtieneCampoTipoEar();

                // Obtiene usuario
                SQ_Usuario sQ_Usuario = new SQ_Usuario();

                List<SolicitudRD> list = hash.GetResultAsType(SQ_QueryManager.Generar(create == "PWB" ? SQ_Query.get_solicitudRendicion : SQ_Query.get_solicitudRendicionSAP), dc =>
                {
                    return new SolicitudRD()
                    {
                        ID = Convert.ToInt32(dc["ID"]),
                        STR_DOCENTRY = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_DOCENTRY"])) ? (int?)null : Convert.ToInt32(dc["STR_DOCENTRY"]),
                        STR_NRSOLICITUD = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_NRSOLICITUD"])) ? (int?)null : Convert.ToInt32(dc["STR_NRSOLICITUD"]),
                        STR_NRRENDICION = dc["STR_NRRENDICION"],
                        STR_ESTADO = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_ESTADO"])) ? (int?)null : Convert.ToInt32(dc["STR_ESTADO"]),
                        STR_MOTIVO = dc["STR_MOTIVO"],
                        STR_UBIGEO = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_UBIGEO"])) ? (int?)null : Convert.ToInt32(dc["STR_UBIGEO"]),
                        STR_RUTA = dc["STR_RUTA"],
                        STR_RUTA_INFO = masDetalle ? (string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_RUTA"])) ? null : sQ_Complemento.ObtenerDesplegablePorId(campo.ToString(), dc["STR_RUTA"]).Result[0]) : null,
                        STR_DIRECCION = masDetalle ? (string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_UBIGEO"])) ? null : sQ_Ubicacion.ObtenerDireccion(dc["STR_UBIGEO"]).Result[0]) : null,
                        STR_RUTAANEXO = dc["STR_RUTAANEXO"],
                        STR_MONEDA = dc["STR_MONEDA"],
                        STR_TIPORENDICION = dc["STR_TIPORENDICION"],
                        STR_TIPORENDICION_INFO = masDetalle ? (string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_TIPORENDICION"])) ? null : sQ_Complemento.ObtenerDesplegablePorId(campEar.ToString(), dc["STR_TIPORENDICION"]).Result[0]) : null,
                        STR_TOTALSOLICITADO = Convert.ToDouble(dc["STR_TOTALSOLICITADO"]),
                        STR_MOTIVOMIGR = dc["STR_MOTIVOMIGR"],
                        STR_EMPLDASIG = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_EMPLDASIG"])) ? (int?)null : Convert.ToInt32(dc["STR_EMPLDASIG"]),
                        STR_EMPLDREGI = string.IsNullOrWhiteSpace(Convert.ToString(dc["STR_EMPLDREGI"])) ? (int?)null : Convert.ToInt32(dc["STR_EMPLDREGI"]),
                        STR_FECHAREGIS = string.IsNullOrWhiteSpace(dc["STR_FECHAREGIS"]) ? "" : Convert.ToDateTime(dc["STR_FECHAREGIS"]).ToString("dd/MM/yyyy"),
                        STR_FECHAINI = string.IsNullOrWhiteSpace(dc["STR_FECHAINI"]) ? "" : Convert.ToDateTime(dc["STR_FECHAINI"]).ToString("dd/MM/yyyy"),
                        STR_FECHAFIN = string.IsNullOrWhiteSpace(dc["STR_FECHAFIN"]) ? "" : Convert.ToDateTime(dc["STR_FECHAFIN"]).ToString("dd/MM/yyyy"),
                        STR_FECHAVENC = string.IsNullOrWhiteSpace(dc["STR_FECHAVENC"]) ? "" : Convert.ToDateTime(dc["STR_FECHAVENC"]).ToString("dd/MM/yyyy"),
                        STR_EMPLEADO_ASIGNADO = masDetalle ? sQ_Usuario.getUsuario(Convert.ToInt32(dc["STR_EMPLDASIG"])).Result[0] : null,
                        STR_ORDENVIAJE = dc["STR_ORDENVIAJE"],
                        STR_AREA = dc["STR_AREA"],
                        SOLICITUD_DET = masDetalle ? listDet : null,
                    };
                }, id.ToString()).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<SolicitudRD>(ex);
            }
        }

        public List<Aprobador> ObtieneListaAprobadores(string tipoUsuario, string idSolicitud, string estado)
        {
            List<Aprobador> listaAprobadores = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_infoAprobadores), dc =>
            {
                return new Aprobador
                {
                    idSolicitud = Convert.ToInt32(dc["ID_SR"]),
                    aprobadorId = Convert.ToInt32(dc["Aprobador Id"]),
                    aprobadorNombre = dc["Nombre Autorizador"],
                    emailAprobador = dc["Email Aprobador"],
                    finalizado = Convert.ToInt32(dc["Finalizado"]),
                    empleadoId = Convert.ToInt32(dc["Empleado Id"]),
                    nombreEmpleado = dc["Nombre Empleado"],
                    fechaRegistro = string.IsNullOrWhiteSpace(dc["STR_FECHAREGIS"]) ? null : Convert.ToDateTime(dc["STR_FECHAREGIS"]).ToString("dd/MM/yyyy")
                };
            }, tipoUsuario, idSolicitud, estado).ToList();

            return listaAprobadores;
        }

        public ConsultationResponse<AprobadorResponse> EnviarSolicitudAprobacion(string idSolicitud, int usuarioId, string tipord, string area, double monto, int estado, List<P_borrador> borradores)
        {
            // Obtiene Aprobadores
            List<AprobadorResponse> response = new List<AprobadorResponse>();
            string valor = hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.get_aprobadores), tipord, area, monto.ToString("F2"));
            List<Aprobador> aprobadors;
            try
            {
                if (valor == "-1")
                    throw new Exception("No se encontró Aprobadores con la solicitud enviada");
                // Determina si es 2 aprobadores o solo 1         
                List<string> aprobadores = valor.Split(',').Take(2).Where(aprobador => aprobador != "-1").ToList();

                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.post_insertAprobadores), idSolicitud, usuarioId.ToString(), null, null, 0, estado == 1 ? aprobadores[0] : aprobadores[1]);

                // Asegura presupuesto 
                if (estado == 1)
                {
                    for (int i = 0; i < borradores.Count; i++)
                    {
                        //string idTTp = hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.get_pendienteBorradorId));
                        hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.post_pendientesBorrador), borradores[i].STR_TOTAL.ToString("F2"), "Y", borradores[i].STR_CENTCOST, borradores[i].STR_POSFIN, idSolicitud, borradores[i].STR_FECHAREGIS);
                    }
                };   // Inserta en la tabla de presupuesto
                if (estado == 1) hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarEstadoSR), "2", "", idSolicitud);                                        // Actualiza el estado

                aprobadors = new List<Aprobador>();
                aprobadors = ObtieneListaAprobadores("2", idSolicitud, "0"); // Autorizadores, solicitud, estado

                if (aprobadors.Count < 1)
                    throw new Exception("No se encontró Aprobadores");

                aprobadors.ForEach(a =>
                {
                    EnviarEmail envio = new EnviarEmail();

                    envio.EnviarConfirmacion(a.emailAprobador,
                        a.aprobadorNombre, a.nombreEmpleado, true, a.idSolicitud.ToString(), "", a.fechaRegistro, a.estado.ToString(), "", "");
                });

                response = new List<AprobadorResponse> { new AprobadorResponse() {
                    aprobadores = aprobadores.Count()
                } };

                return Global.ReturnOk(response, "Ok");
            }
            catch (Exception ex)
            {

                return Global.ReturnError<AprobadorResponse>(ex);
            }

            /*
            string valor = hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.get_aprobadores), tipord, area.ToString(), monto.ToString("F2"), cargo);

            try
            {
                if (valor == "-1")
                    throw new Exception("No se encontró Aprobadores con la solicitud enviada");

                List<string> aprobadores = valor.Split(',').Take(2).ToList();

                aprobadores.ForEach(o =>
                {
                    if (o != "-1")
                    {
                        hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.post_insertAprobadores), id.ToString(), usuarioId.ToString(), o, null, "0"); 
                    }
                });

                hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarEstadoSR), "2", "", id.ToString());                                        // Actualiza el estado
                hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.post_pendientesBorrador), id.ToString(), id.ToString(), monto.ToString("F2"), "Y");   // Inserta en la tabla de presupuesto

                List<Aprobador> listaAprobadores = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_infoAprobadores), dc =>
                {
                    return new Aprobador
                    {
                        idSolicitud = Convert.ToInt32(dc["ID_SR"]),
                        aprobadorId = Convert.ToInt32(dc["Aprobador Id"]),
                        aprobadorNombre = dc["Nombre Autorizador"],
                        emailAprobador = dc["Email Aprobador"],
                        finalizado = Convert.ToInt32(dc["Finalizado"]),
                        empleadoId = Convert.ToInt32(dc["Empleado Id"]),
                        nombreEmpleado = dc["Nombre Empleado"],
                        fechaRegistro = Convert.ToDateTime(dc["STR_FECHAREGIS"])
                    };
                }, id.ToString()).ToList();

                if (listaAprobadores.Count < 1)
                    throw new Exception("No se encontró Aprobadores");

                listaAprobadores.ForEach(a =>
                {
                    EnviarEmail envio = new EnviarEmail();

                    envio.EnviarConfirmacion(a.emailAprobador,
                        a.aprobadorNombre, a.nombreEmpleado, true, a.idSolicitud.ToString(), "", a.fechaRegistro.ToString("dd/MM/yy"), a.estado.ToString(), "", "");
                });

                return Global.ReturnOk(aprobadores, "Ok");

            }
            catch (Exception ex)
            {

                return Global.ReturnError<string>(ex);
            }*/
        }
        public ConsultationResponse<CreateResponse> AceptarSolicitud(int solicitudId, string aprobadorId, string areaAprobador)
        {
            List<CreateResponse> lista = new List<CreateResponse>();
            List<Aprobador> listaAprobados = null;
            SolicitudRD solicitudRD = new SolicitudRD();
            string nuevoEstado = "0"; // Depende si va una solicitud o va ultima
                                      // Dependiendo de si ya es la segunda Aceptación de la solicitud o si solo tiene una migraría a
                                      // 3: "En Autorizacion SR"  o directamente 4: Autorizado SR
            try
            {
                solicitudRD = ObtenerSolicitud(solicitudId, "PWB").Result[0]; // Parametro Area y Id de la solicitud
                string valor = hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.get_aprobadores), solicitudRD.STR_TIPORENDICION, solicitudRD.STR_AREA, solicitudRD.STR_TOTALSOLICITADO.ToString("F2"));

                if (valor == "-1")
                    throw new Exception("No se encontró Aprobadores con la solicitud enviada");
                // Determina si es 2 aprobadores o solo 1         
                List<string> aprobadores = valor.Split(',').Take(2).Where(aprobador => aprobador != "-1").ToList();
                bool existeAprobador = aprobadores.Any(dat => dat.Equals(areaAprobador));

                // Valide que se encuentre en la lista de aprobadores pendientes
                listaAprobados = new List<Aprobador>();
                listaAprobados = ObtieneListaAprobadores("2", solicitudId.ToString(), "0");
                existeAprobador = listaAprobados.Any(dat => dat.aprobadorId == Convert.ToInt32(aprobadorId));

                if (existeAprobador)
                {
                    if (aprobadores.Count == 1 | solicitudRD.STR_ESTADO == 3)
                    {
                        EnviarEmail envio = new EnviarEmail();

                        hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_aprobadores), aprobadorId, DateTime.Now.ToString("yyyy-MM-dd"), 1, areaAprobador, solicitudId, 0);

                        var response = GeneraSolicitudRDenSAP(solicitudRD);

                        if (response.IsSuccessful)
                        {
                            CreateResponse createResponse = JsonConvert.DeserializeObject<CreateResponse>(response.Content);
                            createResponse.AprobacionFinalizada = 1;

                            // Inserts despues de crear la SR en SAP 
                            hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarEstadoSR), "6", "", solicitudId);                                       // Actualiza Estado
                            //hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.post_intermedia), createResponse.DocEntry);                                             // Inserta en la tabla intemedia de EAR para generar codigo
                            string codigoRendicion = hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.get_numeroRendicion), createResponse.DocEntry);   // Obtiene el número de Rendición con el DocEntry
                            hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarMigradaSR), createResponse.DocEntry, createResponse.DocNum, codigoRendicion, solicitudId);   // Actualiza en la tabla, DocEnty DocNum y Numero de Rendicón

                            // Obtiene Aprobadores para actualizar
                            //listaAprobados = ObtieneAprobadores(solicitudId.ToString()).Result;
                            //hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_migradaSRenSAP), solicitudId, solicitudRD.STR_EMPLEADO_ASIGNADO.empID, listaAprobados[0].aprobadorNombre, listaAprobados[1].aprobadorNombre, createResponse.DocEntry);    // Actualiza en SAP el codigo de SR y el de Empleado Asignado y los autorizadores                                                                                                                                                                                     // Quita de activos en la tabla de pendientes de Borrador
                            hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_pendientesBorrador), "N", solicitudId);

                            lista.Add(createResponse);

                            // Envio de Correo


                            envio.EnviarInformativo(solicitudRD.STR_EMPLEADO_ASIGNADO.email, FormatearNombreCompleto(solicitudRD.STR_EMPLEADO_ASIGNADO.Nombres), true, solicitudRD.ID.ToString(), "Número de Rendición: " + codigoRendicion, solicitudRD.STR_FECHAREGIS, true, "");
                            return Global.ReturnOk(lista, "");
                        }
                        else
                        {
                            string mensaje = JsonConvert.DeserializeObject<ErrorSL>(response.Content).error.message.value;
                            hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarEstadoSR), "7", mensaje.Replace("'", ""), solicitudId);
                            envio.EnviarError(true, null, solicitudRD.ID.ToString(), solicitudRD.STR_FECHAREGIS);
                            throw new Exception(mensaje);
                        }
                    }
                    else
                    {
                        CreateResponse createresponse = new CreateResponse()
                        {
                            AprobacionFinalizada = 0
                        };
                        //createresponse.aprobacionfinalizada = 0;
                        hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_aprobadores), aprobadorId, DateTime.Now.ToString("yyyy-MM-dd"), 1, areaAprobador, solicitudId, 0);
                        hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarEstadoSR), 3, null, solicitudId);

                        lista.Add(createresponse);

                        // Envia la solicitud al siguiente aprobador
                        EnviarSolicitudAprobacion(solicitudId.ToString(), (int)solicitudRD.STR_EMPLDASIG, solicitudRD.STR_TIPORENDICION, solicitudRD.STR_AREA, solicitudRD.STR_TOTALSOLICITADO, 2, null);

                        return Global.ReturnOk(lista, "");
                    }
                }
                else
                {
                    throw new Exception("No se encontraron solicitudes pendientes");
                }
            }
            catch (Exception ex)
            {

                return Global.ReturnError<CreateResponse>(ex);
            }
        }
        static string FormatearNombreCompleto(string nombreCompleto)
        {
            string[] partes = nombreCompleto.Split(' ');

            if (partes.Length >= 2)
            {
                for (int i = 0; i < partes.Length; i++)
                {
                    partes[i] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(partes[i].ToLower());
                }

                return string.Join(" ", partes);
            }

            return nombreCompleto;
        }
        public List<Aprobador> obtieneAprobadoresDet(string idArea, string aprobadorId, string fecha)
        {
            List<Aprobador> aprobadors = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_listaAprobadoresDet), dc =>
            {
                return new Aprobador()
                {
                    aprobadorId = Convert.ToInt32(dc["empID"]),
                    aprobadorNombre = dc["lastName"],
                    finalizado = dc["empID"] == aprobadorId ? 1 : 0,
                    fechaRegistro = dc["empID"] == aprobadorId ? fecha : null
                };
            }, idArea).ToList();
            return aprobadors;
        }
        public ConsultationResponse<Aprobador> ObtieneAprobadores(string idSolicitud)
        {
            try
            {
                List<Aprobador> aprobadors = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_listaAprobadoresCab), dc =>
                {
                    return new Aprobador()
                    {
                        aprobadorNombre = dc["Nombres"],
                        aprobadorId = string.IsNullOrEmpty(dc["STR_USUARIOAPROBADORID"]) ? 0 : Convert.ToInt32(dc["STR_USUARIOAPROBADORID"]),
                        finalizado = Convert.ToInt32(dc["APROBACIONFINALIZADA"]),
                        area = Convert.ToInt32(dc["STR_AREA"]),
                        fechaRegistro = string.IsNullOrWhiteSpace(dc["FECHAAPROBACION"]) ? "" : DateTime.Parse(dc["FECHAAPROBACION"]).ToString("dd/MM/yyyy"),
                        aprobadores = obtieneAprobadoresDet(dc["STR_AREA"], dc["STR_USUARIOAPROBADORID"], string.IsNullOrWhiteSpace(dc["FECHAAPROBACION"]) ? "" : DateTime.Parse(dc["FECHAAPROBACION"]).ToString("dd/MM/yyyy"))
                    };
                }, idSolicitud).ToList();


                return Global.ReturnOk(aprobadors, "");

            }
            catch (Exception ex)
            {
                return Global.ReturnError<Aprobador>(ex);
            }
        }
        public ConsultationResponse<string> RechazarSolicitud(string solicitudId, string aprobadorId, string comentarios, string areaAprobador)
        {
            string nuevoEstado = "5";
            // Si una solicitud es Rechazada volverá a ser editable
            // 5: "Rechazado SR"
            List<Aprobador> listaAprobados = null;
            SolicitudRD solicitudRD = new SolicitudRD();
            SQ_Usuario sQ_Usuario = new SQ_Usuario();
            Usuario usuario = null;

            try
            {
                solicitudRD = ObtenerSolicitud(Convert.ToInt32(solicitudId), "PWB").Result[0]; // Parametro Area y Id de la solicitud
                string valor = hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.get_aprobadores), solicitudRD.STR_TIPORENDICION, solicitudRD.STR_AREA, solicitudRD.STR_TOTALSOLICITADO.ToString("F2"));

                if (valor == "-1")
                    throw new Exception("No se encontró Aprobadores con la solicitud enviada");
                // Determina si es 2 aprobadores o solo 1         
                List<string> aprobadores = valor.Split(',').Take(2).Where(aprobador => aprobador != "-1").ToList();
                bool existeAprobador = aprobadores.Any(dat => dat.Equals(areaAprobador));

                // Valide que se encuentre en la lista de aprobadores pendientes
                listaAprobados = new List<Aprobador>();
                listaAprobados = ObtieneListaAprobadores("2", solicitudId.ToString(), "0");
                existeAprobador = listaAprobados.Any(dat => dat.aprobadorId == Convert.ToInt32(aprobadorId));

                if (existeAprobador)
                {
                    List<string> lista = new List<string>() {
                    "Rechazado con exito"
                    };

                    EnviarEmail envio = new EnviarEmail();

                    usuario = new Usuario();
                    usuario = sQ_Usuario.getUsuario(listaAprobados[0].empleadoId).Result[0];

                    if (usuario.email == null | usuario.email == "")
                    {
                        throw new Exception("No se encontró correo del empleado");
                    }
                    envio.EnviarInformativo(usuario.email, usuario.Nombres, true, listaAprobados[0].idSolicitud.ToString(),
                        "", listaAprobados[0].fechaRegistro, false, comentarios);

                    hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.dlt_aprobadoresSr), solicitudId);
                    hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarEstadoSR), nuevoEstado, "", solicitudId);
                    hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_pendientesBorrador), "N", solicitudId);
                    hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.dlt_pendientesBorrador), solicitudId);

                    return Global.ReturnOk(lista, "No se rechazo correctamente");
                }
                else
                {
                    throw new Exception("No se encontró solicitud a rechazar");
                }
            }
            catch (Exception ex)
            {
                return Global.ReturnError<string>(ex);
            }
        }
        public ConsultationResponse<Complemento> ValidacionSolicitud(int id)
        {
            List<Complemento> list = new List<Complemento>();
            SolicitudRD solicitud = new SolicitudRD();
            List<string> CamposVacios = new List<string>();
            PresupuestoRq preRq = new PresupuestoRq();
            SQ_Complemento sQ_Complemento = new SQ_Complemento();
            var respIncorrect = string.Empty;

            try
            {
                solicitud = ObtenerSolicitud(id, "PWB", true).Result[0];

                if (solicitud.STR_UBIGEO == null) CamposVacios.Add("Dirección");
                if (string.IsNullOrEmpty(solicitud.STR_RUTA)) CamposVacios.Add("Ruta");
                if (string.IsNullOrEmpty(solicitud.STR_RUTAANEXO)) CamposVacios.Add("Adjuntos");
                if (string.IsNullOrEmpty(solicitud.STR_FECHAFIN)) CamposVacios.Add("Fecha Fin");
                if (string.IsNullOrEmpty(solicitud.STR_FECHAINI)) CamposVacios.Add("Fecha de Inicio");
                if (solicitud.STR_TIPORENDICION != "ORV") if (string.IsNullOrEmpty(solicitud.STR_FECHAVENC)) CamposVacios.Add("Fecha de Vencimiento");
                if (string.IsNullOrEmpty(solicitud.STR_MONEDA)) CamposVacios.Add("Moneda");
                if (string.IsNullOrEmpty(solicitud.STR_TIPORENDICION)) CamposVacios.Add("Tipo de Rendición");

                // Valida Cent Costo



                if (solicitud.STR_TIPORENDICION != "ORV")
                {
                    if (solicitud.SOLICITUD_DET.Count == 0)
                    {
                        CamposVacios.Add("Lineas de Detalle");
                    }
                    else
                    {
                        solicitud.SOLICITUD_DET.ForEach((e) =>
                        {
                            if (e.articulo == null) { CamposVacios.Add("Nivel detalle"); return; }
                            if (e.STR_CANTIDAD == 0) { CamposVacios.Add("Nivel detalle"); return; }
                            // if (e.centCostos.Count == 0) { CamposVacios.Add("Nivel detalle"); return; }
                            if (e.ceco == null) { CamposVacios.Add("Nivel detlale"); return; }
                            if (e.cup == null) { CamposVacios.Add("Nivel detalle"); return; }
                            if (e.posFinanciera == null) { CamposVacios.Add("Nivel detalle"); return; }

                            bool exisCeco = hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.get_validaCECO), e.ceco) != "0";

                            if (!exisCeco) { CamposVacios.Add("Centro de Costo no existe"); return; }

                        });

                        if (!CamposVacios.Any((text) => text == "Nivel detalle"))
                        {
                            SolicitudRDdet det = solicitud.SOLICITUD_DET[0];

                            // Valida presupuesto solo si tiene contenido a nivel detalle - si no termine 
                            preRq = new PresupuestoRq()
                            {
                                //centCostos = det.centCostos[0]?.name,
                                centCostos = det.ceco,
                                posFinanciera = det.posFinanciera.name,
                                anio = DateTime.Now.Year.ToString(),
                                precio = -(decimal)solicitud.STR_TOTALSOLICITADO,
                            };
                            var lestPrep = sQ_Complemento.ObtienePresupuesto(preRq).Result;

                            if (lestPrep.Count == 0)
                            {
                                throw new Exception("No se tiene presupuesto");
                            }
                            else
                            {
                                var s = lestPrep[0];

                                if (s.name == "-1")
                                {
                                    throw new Exception("No hay presupuesto suficiente");
                                }
                            }
                        }
                    }
                };
                if (CamposVacios.Count > 0)
                {
                    string CamposErroneos = string.Join(", ", CamposVacios);
                    respIncorrect += $" Faltan completar campos de " + CamposErroneos;
                    throw new Exception(respIncorrect);
                };

                Complemento complemento = new Complemento()
                {
                    id = id.ToString(),
                    name = "Se valido exitosamente"
                };
                list.Add(complemento);

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }

        public ConsultationResponse<CreateResponse> ReintentarSolicitud(int solicitudId)
        {
            List<CreateResponse> lista = new List<CreateResponse>();
            SolicitudRD solicitudRD = new SolicitudRD();
            List<Aprobador> listaAprobados = null;
            string nuevoEstado = "0"; // Depende si va una solicitud o va ultima
                                      // Dependiendo de si ya es la segunda Aceptación de la solicitud o si solo tiene una migraría a
                                      // 3: "En Autorizacion SR"  o directamente 4: Autorizado SR

            try
            {
                // Envio de Correo
                EnviarEmail envio = new EnviarEmail();

                solicitudRD = ObtenerSolicitud(solicitudId, "PWB").Result[0];

                var response = GeneraSolicitudRDenSAP(solicitudRD);

                if (response.IsSuccessful)
                {

                    CreateResponse createResponse = JsonConvert.DeserializeObject<CreateResponse>(response.Content);
                    createResponse.AprobacionFinalizada = 1;
                    nuevoEstado = "6";

                    // Inserts despues de crear la SR en SAP 
                    hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarEstadoSR), nuevoEstado, "", solicitudId);                                       // Actualiza Estado
                    //hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.post_intermedia), createResponse.DocEntry);                                             // Inserta en la tabla intemedia de EAR para generar codigo
                    string codigoRendicion = hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.get_numeroRendicion), createResponse.DocEntry);   // Obtiene el número de Rendición con el DocEntry
                    hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarMigradaSR), createResponse.DocEntry, createResponse.DocNum, codigoRendicion, solicitudId);   // Actualiza en la tabla, DocEnty DocNum y Numero de Rendicón

                    // Obtiene Aprobadores para actualizar
                    //listaAprobados = new List<Aprobador>();
                    //listaAprobados = ObtieneAprobadores(solicitudId.ToString()).Result;
                    //hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_migradaSRenSAP), solicitudId, solicitudRD.STR_EMPLDREGI, listaAprobados[0].aprobadorNombre, listaAprobados[1].aprobadorNombre, createResponse.DocEntry);    // Actualiza en SAP el codigo de SR y el de Empleado Asignado
                    hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_pendientesBorrador), "N", solicitudId);

                    lista.Add(createResponse);



                    envio.EnviarInformativo(solicitudRD.STR_EMPLEADO_ASIGNADO.email, FormatearNombreCompleto(solicitudRD.STR_EMPLEADO_ASIGNADO.Nombres), true, solicitudRD.ID.ToString(), "Número de Rendición: " + codigoRendicion, solicitudRD.STR_FECHAREGIS, true, "");
                    return Global.ReturnOk(lista, "");
                }
                else
                {
                    nuevoEstado = "7";
                    string mensaje = JsonConvert.DeserializeObject<ErrorSL>(response.Content).error.message.value;
                    hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarEstadoSR), nuevoEstado, mensaje.Replace("'", ""), solicitudId);
                    //envio.EnviarError(true, null, solicitudRD.ID.ToString(), solicitudRD.STR_FECHAREGIS);
                    throw new Exception(mensaje);
                }
            }
            catch (Exception ex)
            {
                return Global.ReturnError<CreateResponse>(ex);

            }
        }
        public static int ObtieneCampoTipoRuta()
        {
            try
            {
                string valor;
                valor = ConfigurationManager.AppSettings["tipoRutafielID"].ToString();

                if (string.IsNullOrEmpty(valor))
                    return -99;
                else
                    return Convert.ToInt32(valor);
            }
            catch (Exception)
            {
                return -99;
            }
        }

        public IRestResponse GeneraSolicitudRDenSAP(SolicitudRD sr)
        {
            SQ_Usuario sQ_Usuario = new SQ_Usuario();
            List<DetalleSerializar> detalleSerializars = new List<DetalleSerializar>();

            //int series = Convert.ToInt32(ConfigurationManager.AppSettings["SerieCodeEAR"]);


            Usuario usuario = sQ_Usuario.getUsuario((int)sr.STR_EMPLDASIG).Result[0];
            AttatchmentSerializer adj = ObtenerAdjuntos(sr.STR_RUTAANEXO);
            List<Aprobador> aprobadores = new List<Aprobador>();
            aprobadores = ObtieneAprobadores(sr.ID.ToString()).Result;

            if (sr.STR_TIPORENDICION != "ORV")
            {
                sr.SOLICITUD_DET.ForEach((e) =>
                {
                    DetalleSerializar detalleSerializar = new DetalleSerializar
                    {
                        ItemCode = e.STR_CODARTICULO,
                        Quantity = e.STR_CANTIDAD,
                        Price = e.precioTotal / e.STR_CANTIDAD,
                        Currency = sr.STR_MONEDA,
                        //CostingCode = e.centCostos[0].name,
                        CostingCode = e.ceco,
                        CostingCode2 = e.posFinanciera.id,
                        U_CNCUP = e.cup.U_CUP,
                        TaxCode = "EXO"
                    };
                    detalleSerializars.Add(detalleSerializar);

                });
            }
            else
            {
                string conceptOrd = ConfigurationManager.AppSettings["concepto_orden_viaje"].ToString();

                DetalleSerializar detalleSerializar = new DetalleSerializar
                {
                    ItemCode = conceptOrd,
                    Quantity = 1,
                };
                detalleSerializars.Add(detalleSerializar);
            }

            SolicitudRDSerializer body = new SolicitudRDSerializer()
            {
                Series = ObtenerSerieOPRQ(),
                RequriedDate = DateTime.ParseExact(sr.STR_FECHAINI, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"), //DateTime.Parse(sr.STR_FECHAINI).ToString("yyyy-MM-dd"),
                RequesterEmail = usuario.email,
                AttachmentEntry = adj.AbsoluteEntry,
                U_STR_TIPOEAR = sr.STR_TIPORENDICION,
                U_DEPARTAMENTO = sr.STR_DIRECCION.Departamento,
                U_PROVINCIA = sr.STR_DIRECCION.Provincia,
                U_DISTRITO = sr.STR_DIRECCION.Distrito,
                U_STR_TIPORUTA = sr.STR_RUTA_INFO.Nombre,
                U_FECINI = DateTime.ParseExact(sr.STR_FECHAINI, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),//DateTime.Parse(sr.STR_FECHAINI).ToString("yyyy-MM-dd"),
                U_FECFIN = DateTime.ParseExact(sr.STR_FECHAFIN, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),//DateTime.Parse(sr.STR_FECHAFIN).ToString("yyyy-MM-dd"),
                Requester = sr.STR_EMPLDASIG.ToString(),
                RequesterName = sr.STR_EMPLEADO_ASIGNADO.Nombres.ToString(),
                RequesterBranch = sr.STR_EMPLEADO_ASIGNADO.SubGerencia,
                RequesterDepartment = sr.STR_EMPLEADO_ASIGNADO.dept,
                ReqType = 171,
                DocDate = DateTime.ParseExact(sr.STR_FECHAREGIS, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),//DateTime.Parse(sr.STR_FECHAREGIS).ToString("yyyy-MM-dd"),
                DocDueDate = string.IsNullOrEmpty(sr.STR_FECHAVENC) ? null : DateTime.ParseExact(sr.STR_FECHAVENC, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),//DateTime.Parse(sr.STR_FECHAVENC).ToString("yyyy-MM-dd"),
                Comments = sr.STR_MOTIVO,
                DocCurrency = sr.STR_MONEDA,
                DocumentLines = detalleSerializars,
                DocType = "dDocument_Items",
                DocRate = 1.0,
                U_CE_MNDA = sr.STR_MONEDA,
                U_STR_WEB_COD = (int)sr.ID,
                U_STR_WEB_AUTPRI = aprobadores[0].aprobadorNombre,
                U_STR_WEB_AUTSEG = aprobadores.Count > 1 ? aprobadores[1].aprobadorNombre : null,
                // U_ELE_Tipo_ER = solicitudRD.STR_TIPORENDICION_INFO.Nombre,
                Printed = "psYes",
                AuthorizationStatus = "dasGenerated",
                U_ELE_SEDE = sr.STR_EMPLEADO_ASIGNADO.fax,
                U_ELE_SUBGER = sr.STR_EMPLEADO_ASIGNADO.SubGerencia.ToString(),
                TaxCode = "EXO",
                TaxLiable = "tYES",
                // NUEVOS CAMPOS
                U_STR_WEB_ORDV = sr.STR_ORDENVIAJE,
                U_STR_WEB_EMPASIG = sr.STR_EMPLDREGI.ToString(),
                U_STR_WEB_PRIID = aprobadores[0].aprobadorId.ToString(),
                U_STR_WEB_SEGID = aprobadores.Count > 1 ? aprobadores[1].aprobadorId.ToString() : null,
            };

            B1SLEndpoint sl = new B1SLEndpoint();
            string json = JsonConvert.SerializeObject(body);
            IRestResponse response = sl.CreateOrdenSL(json);

            return response;
        }

        public AttatchmentSerializer ObtenerAdjuntos(string rutas)
        {
            B1SLEndpoint sl = new B1SLEndpoint();
            List<PathInfo> Paths = new List<PathInfo>();
            List<string> adjuntos = rutas.Split(',').ToList();

            adjuntos.ForEach((e) =>
            {
                PathInfo path = new PathInfo()
                {
                    SourcePath = Path.GetDirectoryName(e),
                    FileName = Path.GetFileNameWithoutExtension(e),
                    FileExtension = Path.GetExtension(e),
                };

                Paths.Add(path);
            });

            AttatchmentSerializer attatchmentSerializer = new AttatchmentSerializer()
            {
                Attachments2_Lines = Paths
            };
            string json = JsonConvert.SerializeObject(attatchmentSerializer);
            IRestResponse response = sl.CreateAttachments(json);

            if (response.IsSuccessful)
            {
                AttatchmentSerializer body = JsonConvert.DeserializeObject<AttatchmentSerializer>(response.Content);

                return body;
            }
            else
            {
                throw new Exception("Hubo un error al subir los documentos adjuntos, revisar a mayor detalle con el administrador");
            }
        }

        public int ObtenerSerieOPRQ()
        {
            try
            {
                string serie = ConfigurationManager.AppSettings["SerieCodeEAR"];
                string anio = DateTime.Now.Year.ToString();
                int codeEAR = 0;

                codeEAR = Convert.ToInt32(hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.get_SerieOPRQ), anio, serie));

                return codeEAR;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static int ObtieneCampoTipoEar()
        {
            try
            {
                string valor;
                valor = ConfigurationManager.AppSettings["tipoEARfielID"].ToString();

                if (string.IsNullOrEmpty(valor))
                    return -99;
                else
                    return Convert.ToInt32(valor);
            }
            catch (Exception)
            {
                return -99;
            }
        }



    }
}
