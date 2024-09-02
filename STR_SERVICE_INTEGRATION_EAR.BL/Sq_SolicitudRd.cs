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
        SqlADOHelper hash = new SqlADOHelper();
        public ConsultationResponse<Complemento> CreaSolicitudRd(SolicitudRD solicitudRD)
        {
            var respIncorrect = "Solicitud de Detalle";
            try
            {
                // Obtener Usuario para completar Nombres
                SQ_Usuario sQ = new SQ_Usuario();
                Usuario usuarioNuevo = sQ.getUsuario(solicitudRD.RML_EMPLDASIG.empleadoID);

                string id = hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.post_insertSR),solicitudRD.RML_EMPLDREGI.empleadoID.ToString(),
                    solicitudRD.RML_EMPLDASIG.empleadoID.ToString(), solicitudRD.RML_ESTADO.id,solicitudRD.RML_FECHAREGIS,solicitudRD.RML_RUTAANEXO,solicitudRD.RML_MONEDA?.id,
                    solicitudRD.RML_TIPORENDICION?.id,solicitudRD.RML_DESCRIPCION,solicitudRD.RML_COMENTARIOS,solicitudRD.RML_TOTALSOLICITADO.ToString("F2"),
                    usuarioNuevo.SubGerencia, usuarioNuevo.Nombres);

                if (string.IsNullOrEmpty(id))
                    throw new Exception("No se pudo crear correctamente la Solicitud de Rendición");
                List<Complemento> list = new List<Complemento>();
                Complemento com = new Complemento() { id = id, name = id };
                list.Add(com);
                return Global.ReturnOk(list, "No se pudo crear correctamente la Solicitud de Rendición");
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

                adjuntos = hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.get_adjuntos), idSolicitud).ToString().Split(',').ToList();

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

                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.post_insertSrDet), detalle.articulo?.ItemCode, detalle.articulo?.ItemName, detalle.precioTotal, detalle.cantidad, detalle.posFinanciera?.name, detalle.cup?.U_CUP, detalle.SR_ID, detalle.ceco, detalle.ctc);

                string id = hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.get_idSrDet), string.Empty).ToString();

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
                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_idSRDet), detalle.articulo.ItemCode, detalle.articulo.ItemName, detalle.precioTotal, detalle.cantidad, detalle.posFinanciera.name, detalle.cup?.U_CUP, detalle.ceco, id);

                List<Complemento> list = new List<Complemento>();
                Complemento complemento = new Complemento()
                {
                    //id = id.ToString(),
                    //Descripcion = "Actualizado exitosamente",
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
                // Obtener Usuario para completar Nombres
                SQ_Usuario sQ = new SQ_Usuario();
                Usuario usuarioNuevo = sQ.getUsuario(solicitudRD.RML_EMPLDASIG.empleadoID);

                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_idSR), solicitudRD.RML_EMPLDASIG?.empleadoID,solicitudRD.RML_NRSOLICITUD,
                    solicitudRD.RML_NRRENDICION,solicitudRD.RML_ESTADO?.id,solicitudRD.RML_RUTAANEXO,solicitudRD.RML_MONEDA?.id,
                    solicitudRD.RML_TIPORENDICION?.id,solicitudRD.RML_DESCRIPCION,solicitudRD.RML_COMENTARIOS,solicitudRD.RML_TOTALSOLICITADO,
                    usuarioNuevo.SubGerencia, usuarioNuevo.Nombres, solicitudRD.RML_DOCENTRY,solicitudRD.RML_MOTIVOMIGR, id);

                List<Complemento> list = new List<Complemento>();
                Complemento com = new Complemento() { 
                    id = solicitudRD.ID.ToString(), name = "Creado Exitosamente"
                };

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
                    //id = id.ToString(),
                    //Descripcion = "Actualizado exitosamente",
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
            SQ_Complemento sQ_Complemento = new SQ_Complemento();
            SQ_Usuario sQ_Usuario = new SQ_Usuario();

            string campo = ConfigurationManager.AppSettings["tipoEARfielID"];

            try
            {
                List<SolicitudRD> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_listaSolicitudes), dc =>
                {
                    return new SolicitudRD()
                    {
                        ID = string.IsNullOrWhiteSpace(Convert.ToString(dc["ID"])) ? (int?)null : Convert.ToInt32(dc["ID"]),
                        RML_DOCENTRY = string.IsNullOrWhiteSpace(Convert.ToString(dc["RML_DOCENTRY"])) ? (int?)null : Convert.ToInt32(dc["RML_DOCENTRY"]),
                        RML_NRSOLICITUD = string.IsNullOrWhiteSpace(Convert.ToString(dc["RML_NRSOLICITUD"])) ? (int?)null : Convert.ToInt32(dc["RML_NRSOLICITUD"]),
                        RML_ESTADO = sQ_Complemento.ObtenerEstado(Convert.ToInt32(dc["RML_ESTADO"])),
                        RML_ESTADO_INFO = dc["RML_ESTADO_INFO"],
                        //RML_MOTIVO = dc["RML_MOTIVO"],
                        //RML_RUTAANEXO = dc["RML_RUTAANEXO"],
                        // RML_MONEDA = sQ_Complemento.ObtenerMonedas(dc["RML_MONEDA"]),
                        RML_TIPORENDICION = sQ_Complemento.ObtenerDesplegableId(campo, dc["RML_TIPORENDICION"]),
                        RML_DESCRIPCION = dc["RML_DESCRIPCION"],
                        RML_TOTALSOLICITADO = Convert.ToDecimal(dc["RML_TOTALSOLICITADO"]),
                        RML_EMPLDASIG = sQ_Usuario.getEmpleado(dc["RML_EMPLDASIG"]),
                        RML_EMPLDREGI = sQ_Usuario.getEmpleado(dc["RML_EMPLDREGI"]),
                        RML_FECHAREGIS = string.IsNullOrWhiteSpace(dc["RML_FECHAREGIS"]) ? "" : Convert.ToDateTime(dc["RML_FECHAREGIS"]).ToString("dd/MM/yyyy"),
                        RML_NRRENDICION = dc["RML_NRRENDICION"],
                        RML_MOTIVOMIGR = dc["RML_MOTIVOMIGR"],
                        RML_NOMBRES = dc["RML_NOMBRES"],
                        RML_APROBACIONFINALIZADA = dc["RML_APROBACIONFINALIZADA"],
                        CREATE = dc["RML_CREATE"]
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
                        //Id = Convert.ToInt32(dc["RML_CENT_COSTO"]),
                        //Nombre = dc["RML_CENT_COSTO"],
                        //Descripcion = dc["RML_DET_ID"]
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
                            name = sc["RML_CENTCOSTO"],
                            Descripcion = sc["RML_DET_ID"]
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
                    }, dc["RML_CUP"]).ToList();
                    */
                    return new SolicitudRDdet()
                    {
                        id = Convert.ToInt32(dc["ID"]),
                        ID = Convert.ToInt32(dc["ID"]),
                        RML_CANTIDAD = Convert.ToInt32(dc["RML_CANTIDAD"]),
                        cantidad = Convert.ToInt32(dc["RML_CANTIDAD"]),
                        RML_CONCEPTO = dc["RML_CONCEPTO"],
                        RML_CODARTICULO = dc["RML_CODARTICULO"],
                        RML_CUP = dc["RML_CUP"],
                        RML_POSFINAN = dc["RML_POSFINAN"],
                        RML_TOTAL = Convert.ToDouble(dc["RML_TOTAL"]),
                        SR_ID = Convert.ToInt32(dc["SR_ID"]),
                        articulo = sq_item.ObtenerItem(dc["RML_CODARTICULO"]).Result[0],
                        ceco = dc["RML_CECO"],
                        cup = string.IsNullOrEmpty(dc["RML_CUP"]) ? null : obtenerCup(dc["RML_CUP"]),
                        posFinanciera = new Complemento { id = dc["RML_POSFINAN"], name = dc["RML_POSFINAN"] },
                        precioUnitario = Convert.ToDouble(dc["RML_TOTAL"]) / Convert.ToDouble(dc["RML_CANTIDAD"]),
                        precioTotal = Convert.ToDouble(dc["RML_TOTAL"]),

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

                // Obtiene Ruta
                SQ_Complemento sQ_Complemento = new SQ_Complemento();

                // Obitiene EAR
                int campEar = 0;
                campEar = ObtieneCampoTipoEar();

                // Obtiene usuario
                SQ_Usuario sQ_Usuario = new SQ_Usuario();

                List<SolicitudRD> list = hash.GetResultAsType(SQ_QueryManager.Generar(create == "PWB" ? SQ_Query.get_solicitudRendicion : SQ_Query.get_solicitudRendicionSAP), dc =>
                {
                    return new SolicitudRD()
                    {
                        ID = string.IsNullOrWhiteSpace(Convert.ToString(dc["ID"])) ? (int?)null : Convert.ToInt32(dc["ID"]),
                        RML_DOCENTRY = string.IsNullOrWhiteSpace(Convert.ToString(dc["RML_DOCENTRY"])) ? (int?)null : Convert.ToInt32(dc["RML_DOCENTRY"]),
                        RML_NRSOLICITUD = string.IsNullOrWhiteSpace(Convert.ToString(dc["RML_NRSOLICITUD"])) ? (int?)null : Convert.ToInt32(dc["RML_NRSOLICITUD"]),
                        RML_ESTADO = sQ_Complemento.ObtenerEstado(Convert.ToInt32(dc["RML_ESTADO"])),
                       // RML_ESTADO_INFO = dc["RML_ESTADO_INFO"],
                        RML_MOTIVO = dc["RML_MOTIVO"],
                        RML_RUTAANEXO = dc["RML_RUTAANEXO"],
                        RML_MONEDA = sQ_Complemento.ObtenerMoneda(dc["RML_MONEDA"]),
                        RML_TIPORENDICION = sQ_Complemento.ObtenerDesplegableId(campEar.ToString(), dc["RML_TIPORENDICION"]),
                        RML_DESCRIPCION = dc["RML_DESCRIPCION"],
                        RML_TOTALSOLICITADO = Convert.ToDecimal(dc["RML_TOTALSOLICITADO"]),
                        RML_EMPLDASIG = sQ_Usuario.getEmpleado(dc["RML_EMPLDASIG"]),
                        RML_EMPLDREGI = sQ_Usuario.getEmpleado(dc["RML_EMPLDREGI"]),
                        RML_FECHAREGIS = string.IsNullOrWhiteSpace(dc["RML_FECHAREGIS"]) ? "" : Convert.ToDateTime(dc["RML_FECHAREGIS"]).ToString("dd/MM/yyyy"),
                        RML_NRRENDICION = dc["RML_NRRENDICION"],
                        RML_MOTIVOMIGR = dc["RML_MOTIVOMIGR"],
                        RML_COMENTARIOS = dc["RML_COMENTARIOS"],
                        RML_AREA = dc["RML_AREA"]
                        //CREATE = dc["RML_CREATE"]
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
                    fechaRegistro = string.IsNullOrWhiteSpace(dc["RML_FECHAREGIS"]) ? null : Convert.ToDateTime(dc["RML_FECHAREGIS"]).ToString("dd/MM/yyyy")
                };
            }, tipoUsuario, idSolicitud, estado).ToList();

            return listaAprobadores;
        }

        public ConsultationResponse<AprobadorResponse> EnviarSolicitudAprobacion(string idSolicitud, int usuarioId, string tipord, string area, double monto, int estado)
        {
            // Obtiene Aprobadores
            List<AprobadorResponse> response = new List<AprobadorResponse>();
            string valor = hash.GetValueSqlDrct(SQ_QueryManager.Generar(SQ_Query.get_aprobadores), tipord, area, monto.ToString("F2"));
            List<Aprobador> aprobadors;
            try
            {
                if (valor == "-1")
                    throw new Exception("No se encontró Aprobadores con la solicitud enviada");
                // Determina si es 2 aprobadores o solo 1         
                List<string> aprobadores = valor.Split(',').Take(2).Where(aprobador => aprobador != "-1").ToList();

                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.post_insertAprobadores), idSolicitud, usuarioId.ToString(), null, null, 0, estado == 1 ? aprobadores[0] : aprobadores[1]);

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
                        fechaRegistro = Convert.ToDateTime(dc["RML_FECHAREGIS"])
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
            SQ_Usuario sQ_Usuario = new SQ_Usuario();
            string nuevoEstado = "0"; // Depende si va una solicitud o va ultima
                                      // Dependiendo de si ya es la segunda Aceptación de la solicitud o si solo tiene una migraría a
                                      // 3: "En Autorizacion SR"  o directamente 4: Autorizado SR
            try
            {
                solicitudRD = ObtenerSolicitud(solicitudId, "PWB").Result[0]; // Parametro Area y Id de la solicitud
                string valor = hash.GetValueSqlDrct(SQ_QueryManager.Generar(SQ_Query.get_aprobadores), solicitudRD.RML_TIPORENDICION.id, solicitudRD.RML_AREA, solicitudRD.RML_TOTALSOLICITADO.ToString("F2"));

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
                    if (aprobadores.Count == 1 | solicitudRD.RML_ESTADO.id == "3")
                    {

                        EnviarEmail envio = new EnviarEmail();

                        hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_aprobadores), aprobadorId, DateTime.Now.ToString("yyyy-MM-dd"), 1, areaAprobador, solicitudId, 0);

                        var response = GeneraSolicitudRDenSAP(solicitudRD);

                        if (response.IsSuccessful)
                        {
                            CreateResponse createResponse = JsonConvert.DeserializeObject<CreateResponse>(response.Content);
                            createResponse.RML_APROBACIONFINALIZADA = 1;

                            // Inserts despues de crear la SR en SAP 
                            hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarEstadoSR), "6", "", solicitudId);                                       // Actualiza Estado
                            string codigoRendicion = "";

                            Usuario solicitante = sQ_Usuario.getUsuario(solicitudRD.RML_EMPLDASIG.empleadoID);
                            //codigoRendicion = hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.get_numeroRendicion), createResponse.DocEntry);   // Obtiene el número de Rendición con el DocEntry
                            hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarMigradaSR), createResponse.DocEntry, createResponse.DocNum, codigoRendicion, solicitudId);   // Actualiza en la tabla, DocEnty DocNum y Numero de Rendicón
                            lista.Add(createResponse);

                            // Envio de Correo
                            envio.EnviarInformativo(solicitante.email, FormatearNombreCompleto(solicitante.Nombres), true, solicitudRD.ID.ToString(), "Número de Rendición: " + codigoRendicion, solicitudRD.RML_FECHAREGIS, true, "");
                            return Global.ReturnOk(lista, "");
                        }
                        else
                        {
                            string mensaje = JsonConvert.DeserializeObject<ErrorSL>(response.Content).error.message.value;
                            hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarEstadoSR), "7", mensaje.Replace("'", ""), solicitudId);
                            envio.EnviarError(true, null, solicitudRD.ID.ToString(), solicitudRD.RML_FECHAREGIS);
                            throw new Exception(mensaje);
                        }

                   
                    }
                    else
                    {
                        CreateResponse createresponse = new CreateResponse()
                        {
                            RML_APROBACIONFINALIZADA = 0
                        };
                        //createresponse.RML_APROBACIONFINALIZADA = 0;
                        hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_aprobadores), aprobadorId, DateTime.Now.ToString("yyyy-MM-dd"), 1, areaAprobador, solicitudId, 0);
                        hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarEstadoSR), 3, null, solicitudId);

                        lista.Add(createresponse);

                        // Envia la solicitud al siguiente aprobador
                      //  EnviarSolicitudAprobacion(solicitudId.ToString(), (int)solicitudRD.RML_EMPLDASIG, solicitudRD.RML_TIPORENDICION, solicitudRD.RML_AREA, solicitudRD.RML_TOTALSOLICITADO, 2, null);

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
                        aprobadorId = string.IsNullOrEmpty(dc["RML_USUARIOAPROBADORID"]) ? 0 : Convert.ToInt32(dc["RML_USUARIOAPROBADORID"]),
                        finalizado = Convert.ToInt32(dc["RML_APROBACIONFINALIZADA"]),
                        area = Convert.ToInt32(dc["RML_AREA"]),
                        fechaRegistro = string.IsNullOrWhiteSpace(dc["RML_FECHAAPROBACION"]) ? "" : DateTime.Parse(dc["RML_FECHAAPROBACION"]).ToString("dd/MM/yyyy"),
                        aprobadores = obtieneAprobadoresDet(dc["RML_AREA"], dc["RML_USUARIOAPROBADORID"], string.IsNullOrWhiteSpace(dc["RML_FECHAAPROBACION"]) ? "" : DateTime.Parse(dc["RML_FECHAAPROBACION"]).ToString("dd/MM/yyyy"))
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
                string valor = hash.GetValueSqlDrct(SQ_QueryManager.Generar(SQ_Query.get_aprobadores), solicitudRD.RML_TIPORENDICION.id, solicitudRD.RML_AREA, solicitudRD.RML_TOTALSOLICITADO.ToString("F2"));

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
                    usuario = sQ_Usuario.getUsuario(listaAprobados[0].empleadoId);

                    hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.dlt_aprobadoresSr), solicitudId);
                    hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarEstadoSR), nuevoEstado, "", solicitudId);

                    if (!string.IsNullOrEmpty(usuario.email))
                    {
                        envio.EnviarInformativo(usuario.email, usuario.Nombres, true, listaAprobados[0].idSolicitud.ToString(),
                                             "", listaAprobados[0].fechaRegistro, false, comentarios);
                    }
                
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

                if (solicitud.RML_TIPORENDICION == null) throw new Exception("NO se seleccionó el tipo de operación");
                if (solicitud.RML_COMENTARIOS == null) throw new Exception("Falta Agregar comentarios");
                if (solicitud.RML_DESCRIPCION == null) throw new Exception("Falta Agregar Descripción");
                if (solicitud.RML_TOTALSOLICITADO == null || solicitud.RML_TOTALSOLICITADO == 0) throw new Exception("Falta completar el monto Solicitado");
                // Valida Cent Costo

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
                    createResponse.RML_APROBACIONFINALIZADA = 1;
                    nuevoEstado = "6";

                    // Inserts despues de crear la SR en SAP 
                    hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarEstadoSR), nuevoEstado, "", solicitudId);                                       // Actualiza Estado
                    string codigoRendicion = hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.get_numeroRendicion), createResponse.DocEntry);   // Obtiene el número de Rendición con el DocEntry
                    hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarMigradaSR), createResponse.DocEntry, createResponse.DocNum, codigoRendicion, solicitudId);   // Actualiza en la tabla, DocEnty DocNum y Numero de Rendicón

                    // Obtiene Aprobadores para actualizar
                    //listaAprobados = new List<Aprobador>();
                    //listaAprobados = ObtieneAprobadores(solicitudId.ToString()).Result;
                    //hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_migradaSRenSAP), solicitudId, solicitudRD.RML_EMPLDREGI, listaAprobados[0].aprobadorNombre, listaAprobados[1].aprobadorNombre, createResponse.DocEntry);    // Actualiza en SAP el codigo de SR y el de Empleado Asignado
                    hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_pendientesBorrador), "N", solicitudId);

                    lista.Add(createResponse);



                    envio.EnviarInformativo(solicitudRD.RML_EMPLEADO_ASIGNADO.email, FormatearNombreCompleto(solicitudRD.RML_EMPLEADO_ASIGNADO.Nombres), true, solicitudRD.ID.ToString(), "Número de Rendición: " + codigoRendicion, solicitudRD.RML_FECHAREGIS, true, "");
                    return Global.ReturnOk(lista, "");
                }
                else
                {
                    nuevoEstado = "7";
                    string mensaje = JsonConvert.DeserializeObject<ErrorSL>(response.Content).error.message.value;
                    hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarEstadoSR), nuevoEstado, mensaje.Replace("'", ""), solicitudId);
                    //envio.EnviarError(true, null, solicitudRD.ID.ToString(), solicitudRD.RML_FECHAREGIS);
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
            // oBTENER A LOS USUARIOS DE APROBADOR y empleado
            Usuario creador = sQ_Usuario.getUsuario(sr.RML_EMPLDREGI.empleadoID);
            Usuario solicitante = sQ_Usuario.getUsuario(sr.RML_EMPLDASIG.empleadoID);
           // Usuario aprobador = sQ_Usuario.getUsuario(sr.RML_EMPLDASIG.empleadoID);

            //AttatchmentSerializer adj = ObtenerAdjuntos(sr.RML_RUTAANEXO);
            List<Aprobador> aprobadores = new List<Aprobador>();
            aprobadores = ObtieneAprobadores(sr.ID.ToString()).Result;

            DetalleSerializar detalleSerializar = new DetalleSerializar
            {
                ItemDescription = sr.RML_DESCRIPCION,
               // AccountCode = "cuenta contable",    // Obtener Cuenta Contable
                RequiredDate = DateTime.ParseExact(sr.RML_FECHAREGIS, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),
                LineVendor = solicitante.provAsociado,  // Obtener Proveedor,
                U_CE_IMSL =  sr.RML_TOTALSOLICITADO
            };
            detalleSerializars.Add(detalleSerializar);

            SolicitudRDSerializer body = new SolicitudRDSerializer()
            {
               // Series = ObtenerSerieOPRQ(),
                RequriedDate = DateTime.ParseExact(sr.RML_FECHAREGIS, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"), //DateTime.Parse(sr.RML_FECHAINI).ToString("yyyy-MM-dd"),
                RequesterEmail = solicitante.email,
                //AttachmentEntry = adj.AbsoluteEntry,
                //U_RML_TIPOEAR = sr.RML_TIPORENDICION,
                //U_DEPARTAMENTO = sr.RML_DIRECCION.Departamento,
                //U_PROVINCIA = sr.RML_DIRECCION.Provincia,
                //U_DISTRITO = sr.RML_DIRECCION.Distrito,
                //U_RML_TIPORUTA = sr.RML_RUTA_INFO.Nombre,
                //U_FECINI = DateTime.ParseExact(sr.RML_FECHAINI, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),//DateTime.Parse(sr.RML_FECHAINI).ToString("yyyy-MM-dd"),
                //U_FECFIN = DateTime.ParseExact(sr.RML_FECHAFIN, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),//DateTime.Parse(sr.RML_FECHAFIN).ToString("yyyy-MM-dd"),
                Requester = solicitante.empID.ToString(),
                RequesterName = sr.RML_EMPLDASIG.nombre,
                RequesterBranch = solicitante.SubGerencia,
                RequesterDepartment = solicitante.SubGerencia,
                ReqType = 171,
                DocDate = DateTime.ParseExact(sr.RML_FECHAREGIS, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),//DateTime.Parse(sr.RML_FECHAREGIS).ToString("yyyy-MM-dd"),
                DocDueDate = string.IsNullOrEmpty(sr.RML_FECHAREGIS) ? null : DateTime.ParseExact(sr.RML_FECHAREGIS, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),//DateTime.Parse(sr.RML_FECHAVENC).ToString("yyyy-MM-dd"),
                Comments = sr.RML_COMENTARIOS,
                DocCurrency = sr.RML_MONEDA.id,
                DocumentLines = detalleSerializars,
                // DocType = "dDocument_Items",
                DocType = "dDocument_Service",
                DocRate = 1.0,
                U_CE_MNDA = sr.RML_MONEDA.id, 
                U_CE_TPRN = sr.RML_TIPORENDICION.id,
                U_STR_WEB_COD = (int)sr.ID,
                U_STR_WEB_AUTPRI = aprobadores[0].aprobadorNombre,
                U_STR_WEB_AUTSEG = aprobadores.Count > 1 ? aprobadores[1].aprobadorNombre : null,
                // U_ELE_Tipo_ER = solicitudRD.RML_TIPORENDICION_INFO.Nombre,
                Printed = "psYes",
                AuthorizationStatus = "dasGenerated",
               // U_ELE_SEDE = sr.RML_EMPLEADO_ASIGNADO.fax,
               // U_ELE_SUBGER = sr.RML_EMPLEADO_ASIGNADO.SubGerencia.ToString(),
                TaxCode = "EXO",
                TaxLiable = "tYES",
                // NUEVOS CAMPOS
                //U_RML_WEB_ORDV = sr.RML_ORDENVIAJE,
                U_STR_WEB_EMPASIG = creador.empID.ToString(),
                //U_RML_WEB_PRIID = aprobadores[0].aprobadorId.ToString(),
                //U_RML_WEB_SEGID = aprobadores.Count > 1 ? aprobadores[1].aprobadorId.ToString() : null,
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
