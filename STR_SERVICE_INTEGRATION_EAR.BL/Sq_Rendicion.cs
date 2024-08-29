using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Newtonsoft.Json;
using RestSharp;
using STR_SERVICE_INTEGRATION_EAR.EL;
using STR_SERVICE_INTEGRATION_EAR.EL.Commons;
using STR_SERVICE_INTEGRATION_EAR.EL.Requests;
using STR_SERVICE_INTEGRATION_EAR.EL.Responses;
using STR_SERVICE_INTEGRATION_EAR.SL;
using STR_SERVICE_INTEGRATION_EAR.SQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.IO;
using System.Globalization;
using System.Web;
using Path = System.IO.Path;

namespace STR_SERVICE_INTEGRATION_EAR.BL
{
    public class Sq_Rendicion
    {
        SqlADOHelper hash = new SqlADOHelper();

        public ConsultationResponse<Rendicion> ListarSolicitudesS(string usrCreate, string usrAsig, int perfil, string fecIni, string fecFin, string nrRendi, string estado, string area)
        {
            var respIncorrect = "No trajo la lista de solicitudes de rendición";
            SQ_Complemento sQ = new SQ_Complemento();

            try
            {
                List<Rendicion> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_listaRendiciones), dc =>
                {
                    return new Rendicion()
                    {
                        ID = Convert.ToInt32(dc["ID"]),
                        RML_SOLICITUD = Convert.ToInt32(dc["RML_SOLICITUD"]),
                        RML_NRRENDICION = dc["RML_NRRENDICION"],
                        RML_NRAPERTURA = dc["RML_NRAPERTURA"],
                        RML_NRCARGA = string.IsNullOrWhiteSpace(Convert.ToString(dc["RML_NRCARGA"])) ? (int?)null : Convert.ToInt32(dc["RML_NRCARGA"]),
                        RML_ESTADO = Convert.ToInt32(dc["RML_ESTADO"]),
                        RML_ESTADO_INFO = sQ.ObtenerEstado(Convert.ToInt32(dc["RML_ESTADO"])),//dc["RML_ESTADO_INFO"] ,
                        RML_EMPLDASIG = Convert.ToInt32(dc["RML_EMPLDASIG"]),
                        RML_EMPLDREGI = Convert.ToInt32(dc["RML_EMPLDREGI"]),
                        RML_TOTALRENDIDO = Convert.ToDouble(dc["RML_TOTALRENDIDO"]),
                        //RML_TOTALAPERTURA = Convert.ToDouble(dc["RML_TOTALAPERTURA"]),
                        RML_FECHAREGIS = string.IsNullOrWhiteSpace(dc["RML_FECHAREGIS"]) ? "" : Convert.ToDateTime(dc["RML_FECHAREGIS"]).ToString("dd/MM/yyyy"),
                        RML_DOCENTRY = string.IsNullOrWhiteSpace(Convert.ToString(dc["RML_DOCENTRY"])) ? (int?)null : Convert.ToInt32(dc["RML_DOCENTRY"]),
                        RML_MOTIVOMIGR = dc["RML_MOTIVOMIGR"]
                    };
                }, usrCreate, usrAsig, perfil.ToString(), fecIni, fecFin, nrRendi, estado, area).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Rendicion>(ex);
            }
        }

        public ConsultationResponse<Rendicion> ObtenerRendicion(string id)
        {
            var respIncorrect = "Obtener Rendicion";
            SQ_Complemento sQ = new SQ_Complemento();
            SQ_Usuario sQ_Usuario = new SQ_Usuario();
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();

            try
            {
                List<Rendicion> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_rendicion), dc =>
                {
                    return new Rendicion()
                    {
                        ID = Convert.ToInt32(dc["ID"]),
                        RML_SOLICITUD = Convert.ToInt32(dc["RML_SOLICITUD"]),
                        RML_NRRENDICION = dc["RML_NRRENDICION"],
                        RML_NRAPERTURA = dc["RML_NRAPERTURA"],
                        RML_NRCARGA = string.IsNullOrWhiteSpace(Convert.ToString(dc["RML_NRCARGA"])) ? (int?)null : Convert.ToInt32(dc["RML_NRCARGA"]),
                        RML_ESTADO = Convert.ToInt32(dc["RML_ESTADO"]),
                        RML_ESTADO_INFO = sQ.ObtenerEstado(Convert.ToInt32(dc["RML_ESTADO"])),//dc["RML_ESTADO_INFO"] ,
                        RML_EMPLDASIG = Convert.ToInt32(dc["RML_EMPLDASIG"]),
                        RML_EMPLDREGI = Convert.ToInt32(dc["RML_EMPLDREGI"]),
                        RML_TOTALRENDIDO = Convert.ToDouble(dc["RML_TOTALRENDIDO"]),
                        RML_TOTALAPERTURA = dc["RML_TOTALAPERTURA"] != null ? Convert.ToDouble(dc["RML_TOTALAPERTURA"]) : 0.0,
                        RML_FECHAREGIS = string.IsNullOrWhiteSpace(dc["RML_FECHAREGIS"]) ? "" : Convert.ToDateTime(dc["RML_FECHAREGIS"]).ToString("dd/MM/yyyy"),
                        RML_DOCENTRY = string.IsNullOrWhiteSpace(Convert.ToString(dc["RML_DOCENTRY"])) ? (int?)null : Convert.ToInt32(dc["RML_DOCENTRY"]),
                        RML_MOTIVOMIGR = dc["RML_MOTIVOMIGR"],
                        RML_EMPLEADO_ASIGNADO = sQ_Usuario.getUsuario(Convert.ToInt32(dc["RML_EMPLDASIG"])),
                        SOLICITUDRD = sq_SolicitudRd.ObtenerSolicitud(Convert.ToInt32(dc["RML_SOLICITUD"]), "PWB",false).Result[0],
                        documentos = ObtenerDocumentos(dc["ID"]).Result
                    };
                }, id).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Rendicion>(ex);
            }
        }

        public ConsultationResponse<Documento> ObtenerDocumentos(string id)
        {
            SQ_Proveedor sQ_Proveedor = new SQ_Proveedor();
            SQ_Complemento sQ_Complemento = new SQ_Complemento();
            var respIncorrect = "No se obtuvo Documentos";

            try
            {
                List<Documento> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_obtenerDocumentos), dc =>
                {
                    return new Documento()
                    {
                        ID = Convert.ToInt32(dc["ID"]),
                        RML_COMENTARIOS = dc["RML_COMENTARIOS"],
                        RML_ANEXO_ADJUNTO = dc["RML_ANEXO_ADJUNTO"],
                        RML_CORR_DOC = dc["RML_CORR_DOC"],
                        RML_FECHA_CONTABILIZA = string.IsNullOrWhiteSpace(dc["RML_FECHA_CONTABILIZA"]) ? "" : Convert.ToDateTime(dc["RML_FECHA_CONTABILIZA"]).ToString("dd/MM/yyyy"),
                        RML_FECHA_DOC = string.IsNullOrWhiteSpace(dc["RML_FECHA_DOC"]) ? "" : Convert.ToDateTime(dc["RML_FECHA_DOC"]).ToString("dd/MM/yyyy"),
                        RML_FECHA_VENCIMIENTO = string.IsNullOrWhiteSpace(dc["RML_FECHA_VENCIMIENTO"]) ? "" : Convert.ToDateTime(dc["RML_FECHA_VENCIMIENTO"]).ToString("dd/MM/yyyy"),
                        RML_MONEDA = new Complemento { id = dc["RML_MONEDA"], name = dc["RML_MONEDA"] },
                        //RML_OPERACION = dc["RML_OPERACION"],
                       // RML_PARTIDAFLUJO = string.IsNullOrWhiteSpace(Convert.ToString(dc["RML_PARTIDAFLUJO"])) ? (int?)null : Convert.ToInt32(dc["RML_PARTIDAFLUJO"]),
                        RML_PROVEEDOR = dc["RML_PROVEEDOR"] == "" ? null : ObtieneProveedorPrev(dc["RML_PROVEEDOR"], dc["RML_RUC"], dc["RML_RAZONSOCIAL"]),
                        RML_SERIE_DOC = dc["RML_SERIE_DOC"],
                        RML_VALIDA_SUNAT = Convert.ToUInt16(dc["RML_VALIDA_SUNAT"]) == 1,
                        //RML_TIPO_AGENTE = new Complemento { id = dc["RML_TIPO_AGENTE"], name = dc["RML_TIPO_AGENTE"] },
                        RML_TIPO_DOC = dc["RML_TIPO_DOC"] == "" ? null : sQ_Complemento.ObtenerTpoDocumento(dc["RML_TIPO_DOC"]).Result[0],
                        // RML_TIPO_DOC = sQ_Complemento.ObtenerTpoDocumento(dc["RML_TIPO_DOC"]).Result[0],
                        RML_RD_ID = Convert.ToInt32(dc["RML_RD_ID"]),
                        RML_TOTALDOC = Convert.ToDouble(dc["RML_TOTALDOC"])
                        //detalles = listDet
                    };
                }, id).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Documento>(ex);
            }
        }

        public ConsultationResponse<Documento> ObtenerDocumento(string id)
        {
            var respIncorrect = "No se obtuvo Documento";
            SQ_Complemento sQ_Complemento = new SQ_Complemento();
            Sq_Item sq_item = new Sq_Item();

            try
            {
                List<DocumentoDet> listDet = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_listaDocumentoDet), dc =>
                {
                    return new DocumentoDet()
                    {
                        ID = dc["ID"],
                        RML_CODARTICULO = sq_item.ObtenerItem(dc["RML_CODARTICULO"]).Result.FirstOrDefault() ?? null,
                        RML_DOC_ID = Convert.ToInt32(dc["RML_DOC_ID"]),
                        RML_CUENTA_CNTBL = sq_item.ObtenerCuentasContable(dc["RML_CUENTA_CNTBL"]),
                        RML_DIM1 = sQ_Complemento.ObtenerCECOId(dc["RML_DIM1"]),
                        RML_DIM3 = sQ_Complemento.ObtenerCECOId(dc["RML_DIM3"]),
                        
                        RML_INDIC_IMPUESTO = sq_item.ObtenerIndicador(dc["RML_INDIC_IMPUESTO"]),
                        RML_PRECIO = Convert.ToDecimal(dc["RML_PRECIO"]),
                        RML_SUBTOTAL = Convert.ToDecimal(dc["RML_SUBTOTAL"]),
                        RML_ALMACEN = sq_item.ObtenerAlmacen(dc["RML_ALMACEN"]),
                        RML_CANTIDAD = Convert.ToInt32(dc["RML_CANTIDAD"]),
                        //  RML_TPO_OPERACION = dc["RML_TPO_OPERACION"]
                    };
                }, id).ToList();

                List<Documento> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_obtenerDocumento), dc =>
                {
                    return new Documento()
                    {
                        ID = Convert.ToInt32(dc["ID"]),
                        RML_COMENTARIOS = dc["RML_COMENTARIOS"],
                        RML_ANEXO_ADJUNTO = dc["RML_ANEXO_ADJUNTO"],
                        RML_CORR_DOC = dc["RML_CORR_DOC"],
                        RML_FECHA_CONTABILIZA = string.IsNullOrWhiteSpace(dc["RML_FECHA_CONTABILIZA"]) ? "" : Convert.ToDateTime(dc["RML_FECHA_CONTABILIZA"]).ToString("dd/MM/yyyy"),
                        RML_FECHA_DOC = string.IsNullOrWhiteSpace(dc["RML_FECHA_DOC"]) ? "" : Convert.ToDateTime(dc["RML_FECHA_DOC"]).ToString("dd/MM/yyyy"),
                        RML_FECHA_VENCIMIENTO = string.IsNullOrWhiteSpace(dc["RML_FECHA_VENCIMIENTO"]) ? "" : Convert.ToDateTime(dc["RML_FECHA_VENCIMIENTO"]).ToString("dd/MM/yyyy"),
                        RML_MONEDA = sQ_Complemento.ObtenerMoneda(dc["RML_MONEDA"]),//new Complemento { id = dc["RML_MONEDA"], name = dc["RML_MONEDA"] },
                        RML_PROVEEDOR = dc["RML_PROVEEDOR"] == "" ? null : ObtieneProveedorPrev(dc["RML_PROVEEDOR"], dc["RML_RUC"], dc["RML_RAZONSOCIAL"]),
                        RML_SERIE_DOC = dc["RML_SERIE_DOC"],
                        RML_VALIDA_SUNAT = Convert.ToUInt16(dc["RML_VALIDA_SUNAT"]) == 1,
                        RML_TIPO_DOC = dc["RML_TIPO_DOC"] == "" ? null : sQ_Complemento.ObtenerTpoDocumento(dc["RML_TIPO_DOC"]).Result[0],
                        RML_RETE_IMPST = sQ_Complemento.ObtieneDataTablaIR(dc["RML_RETE_IMPST"]),
                        RML_WLIABLE = sQ_Complemento.ObtenerWtliableId(dc["RML_WLIABLE"]),
                        RML_RD_ID = Convert.ToInt32(dc["RML_RD_ID"]),
                        RML_TOTALDOC = Convert.ToDouble(dc["RML_TOTALDOC"]),
                        detalles = listDet
                    };
                }, id).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Documento>(ex);
            }

        }

        public Complemento ObtenTipoAgente(string agente)
        {

            string val = agente == "1" ? "Retención" : agente == "0" ? "Ninguno" : "Detracción";

            Complemento comp = new Complemento { id = agente, name = val };
            return comp;
        }

        public Proveedor ObtieneProveedorPrev(string proveedor, string ruc, string razonSocial)
        {
            Proveedor _proveedor = new Proveedor();
            SQ_Proveedor sQ_Proveedor = new SQ_Proveedor();
            if (proveedor == "P99999999999")
            {
                _proveedor.CardCode = proveedor;
                _proveedor.CardName = razonSocial;
                _proveedor.LicTradNum = ruc;

                return _proveedor;
            }
            else
            {
                return sQ_Proveedor.ObtenerProveedor(proveedor).Result[0];
            }
        }

        public ConsultationResponse<Complemento> CrearDocumento(Documento doc)
        {
            var respIncorrect = "No se pudo registrar Documento";
            Sq_Item sq = new Sq_Item();

            List<Complemento> list = new List<Complemento>();

            try
            {
                string idDoc = hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.post_insertDOC), doc.RML_RD_ID,doc.RML_FECHA_CONTABILIZA,doc.RML_FECHA_DOC,doc.RML_FECHA_VENCIMIENTO,
                    doc.RML_PROVEEDOR?.CardCode,doc.RML_PROVEEDOR?.CardName,doc.RML_PROVEEDOR?.LicTradNum,doc.RML_MONEDA?.id,doc.RML_COMENTARIOS,doc.RML_TIPO_DOC?.id,
                    doc.RML_SERIE_DOC,doc.RML_CORR_DOC,doc.RML_VALIDA_SUNAT ? 1 : 0,doc.RML_TOTALDOC,doc.RML_ANEXO_ADJUNTO,doc.RML_RETE_IMPST?.id,doc.RML_WLIABLE?.code);

                if (string.IsNullOrEmpty(idDoc))
                    throw new Exception("No se pudo crear el documento");

                doc.detalles.ForEach((e) =>
                {
                    hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.post_insertDOCDt), idDoc,e.RML_CODARTICULO?.ItemCode,e.RML_CANTIDAD,e.RML_PRECIO,
                        e.RML_SUBTOTAL,e.RML_INDIC_IMPUESTO?.code,e.RML_DIM1?.id,e.RML_DIM3?.id,e.RML_ALMACEN?.id,e.RML_CUENTA_CNTBL.AcctCode);
                });

                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_RDTotal), doc.RML_RD_ID, doc.RML_RD_ID);

                Complemento complemento = new Complemento()
                {
                    id = idDoc,
                    name = "Se creó documento exitosamente"
                };
                list.Add(complemento);

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }
        public ConsultationResponse<Complemento> ActualizarRendicion(Rendicion rendicion)
        {
            var respIncorrect = "No se pudo Actualizar Rendición";

            List<Complemento> list = new List<Complemento>();

            try
            {
                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_Rendicon), rendicion.RML_NRAPERTURA, rendicion.RML_NRCARGA, rendicion.RML_ESTADO, rendicion.RML_TOTALRENDIDO, rendicion.RML_DOCENTRY, rendicion.RML_MOTIVOMIGR, rendicion.ID);

                Complemento complemento = new Complemento()
                {
                    id = rendicion.ID.ToString(),
                    name = "Se Actualizo Rendición exitosamente"
                };
                list.Add(complemento);

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {

                return Global.ReturnError<Complemento>(ex);
            }

        }
        public ConsultationResponse<Complemento> ActualizarDocumento(Documento doc)
         {
            var respIncorrect = "No se pudo Actualizar Documento";

            List<Complemento> list = new List<Complemento>();

            try
            {
                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_idDOC), doc.RML_FECHA_CONTABILIZA, doc.RML_FECHA_DOC, doc.RML_FECHA_VENCIMIENTO,
                    doc.RML_PROVEEDOR?.CardCode, doc.RML_PROVEEDOR?.CardName, doc.RML_PROVEEDOR?.LicTradNum, doc.RML_MONEDA?.id, doc.RML_COMENTARIOS, doc.RML_TIPO_DOC?.id,
                    doc.RML_SERIE_DOC, doc.RML_CORR_DOC, doc.RML_VALIDA_SUNAT ? 1 : 0, doc.RML_TOTALDOC, doc.RML_ANEXO_ADJUNTO, doc.RML_RETE_IMPST?.id, doc.RML_WLIABLE?.code, doc.ID);

                //string idDoc = hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.get_idDOC), doc.RML_RD_ID.ToString());
                doc.detalles.ForEach((e) =>
                {
                    // Si el detalle fue creado solo actualiza en la tabla 
                    if (e.ID != null) 
                    {
                        hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_idDOCDet), e.ID, e.RML_CODARTICULO?.ItemCode, e.RML_CANTIDAD, e.RML_PRECIO,
                        e.RML_SUBTOTAL, e.RML_INDIC_IMPUESTO?.code, e.RML_DIM1?.id, e.RML_DIM3?.id, e.RML_ALMACEN?.id, e.RML_CUENTA_CNTBL.AcctCode);
                    }
                    else
                    {
                        hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.post_insertDOCDt), doc.ID, e.RML_CODARTICULO?.ItemCode, e.RML_CANTIDAD, e.RML_PRECIO,
                        e.RML_SUBTOTAL, e.RML_INDIC_IMPUESTO?.code, e.RML_DIM1?.id, e.RML_DIM3?.id, e.RML_ALMACEN?.id, e.RML_CUENTA_CNTBL.AcctCode);
                    }
                    // Valida si ya tiene creado ID, si es así                     
                });

                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_RDTotal), doc.RML_RD_ID, doc.RML_RD_ID);

                Complemento complemento = new Complemento()
                {
                    id = doc.ID.ToString(),
                    name = "Se Actualizo Documento exitosamente"
                };
                list.Add(complemento);

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }
        //public ConsultationResponse<Complemento> ActualizarDocumentoDet()
        public ConsultationResponse<Complemento> BorrarDocumento(int id, int rdId)
        {
            var respIncorrect = "No se pudo eliminar DOcumento";

            List<Complemento> list = new List<Complemento>();
            try
            {
                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.dlt_documentoDet), id);

                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.dlt_documento), id);

                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_RDTotal), rdId, rdId);

                Complemento complemento = new Complemento()
                {
                    id = id.ToString(),
                    name = "Se elimino documento exitosamente"
                };
                list.Add(complemento);

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }

        public ConsultationResponse<Complemento> ValidacionDocumento(int id)
        {

            var respIncorrect = "Validación Erronea";
            List<Complemento> list = new List<Complemento>();
            Documento doc = new Documento();
            List<string> CamposVacios = new List<string>();

            try
            {
                doc = ObtenerDocumento(id.ToString()).Result[0];

                if (doc.RML_PROVEEDOR == null) CamposVacios.Add("Proveedor");
                if (doc.RML_MONEDA == null) CamposVacios.Add("Moneda");
                // if (doc.RML_COMENTARIOS == null) CamposVacios.Add("Moneda");
                if (doc.RML_TIPO_DOC == null) CamposVacios.Add("Tipo de Documento");
                if (string.IsNullOrEmpty(doc.RML_SERIE_DOC)) CamposVacios.Add("Serie de Documento");
                if (string.IsNullOrEmpty(doc.RML_CORR_DOC)) CamposVacios.Add("Correlativo de Documento");
                //if (doc.RML_VALIDA_SUNAT == false) CamposVacios.Add("Validación SUNAT");
                if (string.IsNullOrEmpty(doc.RML_ANEXO_ADJUNTO)) CamposVacios.Add("Adjunto");

                doc.detalles.ForEach((e) =>
                {
                    if (e.RML_CODARTICULO == null) { CamposVacios.Add("Nivel detalle"); return; }
                    if (e.RML_INDIC_IMPUESTO == null) { CamposVacios.Add("Nivel detalle"); return; }
                    // if (e.RML_PROYECTO == null) { CamposVacios.Add("Nivel detalle"); return; }
                   // if (e.RML_CUENTA_CNTBL == null) { CamposVacios.Add("Nivel detalle"); return; }
                   // if (e.RML_DIM1 == null) { CamposVacios.Add("Nivel detalle"); return; }
                   // if (e.RML_CUENTA_CNTBL.AcctCode == null) { CamposVacios.Add("Nivel detalle"); return; }
                });

                if (doc.detalles.Count == 0) CamposVacios.Add("Lineas de Detalle");

                if (CamposVacios.Count > 0)
                {
                    string CamposErroneos = string.Join(", ", CamposVacios);
                    respIncorrect += $" {doc.RML_SERIE_DOC + "-" + doc.RML_CORR_DOC} | Faltan completar campos de " + CamposErroneos;
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
        public ConsultationResponse<Complemento> BorrarDetalleDcoumento(int id, int docId)
        {
            var respIncorrect = "No se pudo eliminar Detalle de Documento";

            List<Complemento> list = new List<Complemento>();
            try
            {
                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.dlt_documentoDetId), id, docId);

                Complemento complemento = new Complemento()
                {
                    id = id.ToString(),
                    name = "Se elimino documento exitosamente"
                };
                list.Add(complemento);

                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_DOCTotal), docId, docId);

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }

        public ConsultationResponse<Complemento> ActualizarSntDocumento(int id, string estado)
        {

            var rspIncorect = "No se pudo actualizar";
            List<Complemento> list = new List<Complemento>();
            try
            {
                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_DOCsunt), estado, id);

                Complemento complemento = new Complemento()
                {
                    id = id.ToString(),
                    name = "Se actualizo documento exitosamente"
                };
                list.Add(complemento);


                return Global.ReturnOk(list, rspIncorect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }
        public List<Aprobador> ObtieneListaAprobadores(string tipoUsuario, string idSolicitud, string estado)
        {
            List<Aprobador> listaAprobadores = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_infoAprobadoresRD), dc =>
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
                    fechaRegistro = string.IsNullOrWhiteSpace(dc["RML_FECHAREGIS"]) ? "" : Convert.ToDateTime(dc["RML_FECHAREGIS"]).ToString("dd/MM/yyyy")
                };
            }, tipoUsuario, idSolicitud, estado).ToList();

            return listaAprobadores;
        }
        public ConsultationResponse<AprobadorResponse> EnviarAprobacion(string idRendicion, string idSolicitud, int usuarioId, int estado, string areAprobador) // Id de la solicitud / Usuario y Estado 
        {

            // Obtiene Aprobadores
            List<AprobadorResponse> response = new List<AprobadorResponse>();
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();
            SolicitudRD solicitudRD = new SolicitudRD();
            List<Aprobador> aprobadors;
            try
            {
                // Solo actualizar a Cargado cuando se envie por primera vez la solicitud
                if (estado == 9 | estado == 12) hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarEstadoRD), "10", "", idRendicion);

                solicitudRD = sq_SolicitudRd.ObtenerSolicitud(Convert.ToInt32(idSolicitud),"PWB").Result[0]; // Parametro Area y Id de la solicitud
                string valor = hash.GetValueSqlDrct(SQ_QueryManager.Generar(SQ_Query.get_aprobadores), solicitudRD.RML_TIPORENDICION.id, solicitudRD.RML_AREA, solicitudRD.RML_TOTALSOLICITADO.ToString("F2"));

                if (valor == "-1")
                    throw new Exception("No se encontró Aprobadores con la solicitud enviada");

                List<string> aprobadores = valor.Split(',').Take(3).Where(aprobador => aprobador != "-1").ToList();

                // Se encarga de insertar los aprobadores en la TABLA RML_WEB_APR_RD 
                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.post_insertAprobadoresRD), idRendicion, usuarioId.ToString(), null, null, 0, estado == 9 ? aprobadores[aprobadores.Count == 2 ? 1 : 2] : estado == 11 ? aprobadores[0] : aprobadores[1]);

                aprobadors = new List<Aprobador>();
                aprobadors = ObtieneListaAprobadores(estado == 9 ? "3" : "2", idRendicion, "0"); // Autorizadores, solicitud, estado

                if (aprobadors.Count < 1)
                    throw new Exception("No se encontró Aprobadores");

                aprobadors.ForEach(a =>
                {
                    EnviarEmail envio = new EnviarEmail();

                    envio.EnviarConfirmacion(a.emailAprobador,
                        a.aprobadorNombre, a.nombreEmpleado, false, a.idSolicitud.ToString(), "", a.fechaRegistro, a.estado.ToString(), "", "");
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

        }
        public ConsultationResponse<CreateResponse> AceptarAprobacion(int solicitudId, string aprobadorId, string areaAprobador, int estado, int rendicionId, int area)
        {
            var respIncorrect = "No se realizo la aprobación";
            // Identificar si la aprobación es de Contabilidad o de otros aprobadores
            Rendicion rendicion = new Rendicion();
            SolicitudRD solicitudRD = new SolicitudRD();
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();

            List<CreateResponse> list = new List<CreateResponse>();
            List<Aprobador> listaAprobados = new List<Aprobador>();
            try
            {

                solicitudRD = sq_SolicitudRd.ObtenerSolicitud(solicitudId,"PWB").Result[0]; // Parametro Area y Id de la solicitud
                string valor = hash.GetValueSqlDrct(SQ_QueryManager.Generar(SQ_Query.get_aprobadores), solicitudRD.RML_TIPORENDICION.id, solicitudRD.RML_AREA, solicitudRD.RML_TOTALSOLICITADO.ToString("F2"));

                if (valor == "-1")
                    throw new Exception("No se encontró Aprobadores con la solicitud enviada");

                // Determina si es 2 aprobadores o solo 1         
                List<string> aprobadores = valor.Split(',').Take(3).Where(aprobador => aprobador != "-1").ToList();
                bool existeAprobador = aprobadores.Any(dat => dat.Equals(areaAprobador));

                // Valide que se encuentre en la lista de aprobadores pendientes
                listaAprobados = new List<Aprobador>();
                listaAprobados = ObtieneListaAprobadores(estado == 10 ? "3" : "2", rendicionId.ToString(), "0");
                existeAprobador = listaAprobados.Any(dat => dat.aprobadorId == Convert.ToInt32(aprobadorId));

                if (existeAprobador)
                {
                    if (estado == 10)
                    {
                        hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarEstadoRD), "11", "", rendicionId);
                        // Cambiar upd_aprobadoresRD que permita autorizar por área
                        /*
                        hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_aprobadores), aprobadorId, DateTime.Now.ToString("dd/MM/yy"), 1, areaAprobador, solicitudId, 0);
                        */
                        hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_aprobadoresRD), aprobadorId, DateTime.Now.ToString("yyyy-MM-dd"), 1, areaAprobador, rendicionId, 0);
                        CreateResponse complemento = new CreateResponse()
                        {
                            DocEntry = 0,
                            DocNum = 0,
                            RML_APROBACIONFINALIZADA = 0,
                        };
                        list.Add(complemento);
                        //  ActualizarAprobacion(string idSolicitud, int usuarioId, int estado) 
                        EnviarAprobacion(rendicionId.ToString(), solicitudId.ToString(), Convert.ToInt32(solicitudRD.RML_EMPLDASIG.empleadoID), 11, area.ToString());
                    }
                    else if (estado == 11 & aprobadores.Count == 2 | estado == 13) // Valida estado final
                    {
                        hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_aprobadoresRD), aprobadorId, DateTime.Now.ToString("yyyy-MM-dd"), 1, areaAprobador, rendicionId, 0);
                        hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarEstadoRD), "14", "", rendicionId);


                        // Envio de correo
                        EnviarEmail envio = new EnviarEmail();

                        hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_aprobadoresRD), aprobadorId, DateTime.Now.ToString("yyyy-MM-dd"), 1, areaAprobador, rendicionId, 0);

                        rendicion = ObtenerRendicion(rendicionId.ToString()).Result[0];
                        var response = GenerarRegistroDeRendicion(rendicion);
                        listaAprobados = ObtieneAprobadores(rendicion.ID.ToString()).Result;

                        try
                        {
                            if (response.IsSuccessful)
                            {

                                CreateResponse createResponse = JsonConvert.DeserializeObject<CreateResponse>(response.Content);
                                createResponse.RML_APROBACIONFINALIZADA = 1;

                                // Inserts despues de crear EAR en SAP
                                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarEstadoRD), "16", "", rendicionId);
                                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarMigradaRD), createResponse.DocEntry, createResponse.DocNum, rendicion.ID);
                                // hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_migradaRdenSAP), listaAprobados[1].aprobadorNombre, listaAprobados.Count > 2 ? listaAprobados[2].aprobadorNombre : null, listaAprobados[0].aprobadorNombre, createResponse.DocEntry);

                                list.Add(createResponse);


                                envio.EnviarInformativo(rendicion.RML_EMPLEADO_ASIGNADO.email, rendicion.RML_EMPLEADO_ASIGNADO.Nombres, false,
                                    rendicion.ID.ToString(), rendicion.RML_NRRENDICION, DateTime.Now.ToString("dd/MM/yyyy"), true, "");
                                return Global.ReturnOk(list, respIncorrect);
                            }
                            else
                            {
                                string mensaje = JsonConvert.DeserializeObject<ErrorSL>(response.Content).error.message.value;
                                hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarEstadoRD), "17", mensaje.Replace("'", ""), rendicionId);
                                envio.EnviarError(false, rendicion.RML_NRRENDICION, rendicion.ID.ToString(), rendicion.RML_FECHAREGIS);
                                throw new Exception(mensaje);
                            }
                        }
                        catch (Exception ex)
                        {
                            string mensaje = JsonConvert.DeserializeObject<ErrorSL>(response.Content).error.message.value;
                            hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarEstadoRD), "17", mensaje.Replace("'", ""), rendicionId);
                            envio.EnviarError(false, rendicion.RML_NRRENDICION, rendicion.ID.ToString(), rendicion.RML_FECHAREGIS);
                        }

                    }
                    else if (estado == 11 & aprobadores.Count == 3)
                    {
                        hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarEstadoRD), "13", "", rendicionId);
                        // Cambiar upd_aprobadoresRD que permita autorizar por área
                        /*
                        hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_aprobadores), aprobadorId, DateTime.Now.ToString("dd/MM/yy"), 1, areaAprobador, solicitudId, 0);
                        */
                        hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_aprobadoresRD), aprobadorId, DateTime.Now.ToString("yyyy-MM-dd"), 1, areaAprobador, rendicionId, 0);
                        CreateResponse complemento = new CreateResponse()
                        {
                            DocEntry = 0,
                            DocNum = 0,
                            RML_APROBACIONFINALIZADA = 0,
                        };
                        list.Add(complemento);
                        //  ActualizarAprobacion(string idSolicitud, int usuarioId, int estado) 
                        EnviarAprobacion(rendicionId.ToString(), solicitudId.ToString(), Convert.ToInt32(solicitudRD.RML_EMPLDASIG.empleadoID), 13, area.ToString());
                    }
                    return Global.ReturnOk(list, respIncorrect);
                }
                else
                {
                    throw new Exception("No se tiene rendición a aprobar");
                }

            }
            catch (Exception ex)
            {
                return Global.ReturnError<CreateResponse>(ex);
            }
        }


        public ConsultationResponse<string> RechazarRendicion(int solicitudId, string aprobadorId, string areaAprobador, int estado, int rendicionId, int area)
        {

            var respIncorrect = "No se pudo concretar el Rechazo";
            // Identificar si la aprobación es de Contabilidad o de otros aprobadores
            Rendicion rendicion = new Rendicion();
            SolicitudRD solicitudRD = new SolicitudRD();
            Sq_SolicitudRd sq_SolicitudRd = new Sq_SolicitudRd();

            List<Aprobador> listaAprobados = new List<Aprobador>();
            List<string> list = new List<string>();
            SQ_Usuario sQ_Usuario = new SQ_Usuario();
            Usuario usuario = null;

            try
            {
                solicitudRD = sq_SolicitudRd.ObtenerSolicitud(solicitudId, "PWB").Result[0]; // Parametro Area y Id de la solicitud
                string valor = hash.GetValueSqlDrct(SQ_QueryManager.Generar(SQ_Query.get_aprobadores), solicitudRD.RML_TIPORENDICION.id, solicitudRD.RML_AREA, solicitudRD.RML_TOTALSOLICITADO.ToString("F2"));

                if (valor == "-1")
                    throw new Exception("No se encontró Aprobadores con la solicitud enviada");

                // Determina si es 2 aprobadores o solo 1         
                List<string> aprobadores = valor.Split(',').Take(3).Where(aprobador => aprobador != "-1").ToList();
                bool existeAprobador = aprobadores.Any(dat => dat.Equals(areaAprobador));

                // Valide que se encuentre en la lista de aprobadores pendientes
                listaAprobados = new List<Aprobador>();
                listaAprobados = ObtieneListaAprobadores(estado == 10 ? "3" : "2", rendicionId.ToString(), "0");
                existeAprobador = listaAprobados.Any(dat => dat.aprobadorId == Convert.ToInt32(aprobadorId));

                if (existeAprobador)
                {
                    if (estado == 10) // Contable 
                    {
                        List<string> lista = new List<string>() {
                        "Rechazado con exito"
                        };

                        EnviarEmail envio = new EnviarEmail();

                        usuario = new Usuario();
                        usuario = sQ_Usuario.getUsuario(listaAprobados[0].empleadoId);

                        if (usuario.email == null | usuario.email == "")
                        {
                            throw new Exception("No se encontró correo del empleado");
                        }
                        envio.EnviarInformativo(usuario.email, usuario.Nombres, false, listaAprobados[0].idSolicitud.ToString(),
                            "", listaAprobados[0].fechaRegistro, false, "");

                        hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.dlt_aprobadoresRd), rendicionId);
                        hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarEstadoRD), "12", "", rendicionId);

                        return Global.ReturnOk(lista, "No se rechazo correctamente");
                    }
                    else // Valida los que no está rechazando el usuario Contable
                    {
                        List<string> lista = new List<string>() {
                        "Rechazado con exito"
                        };

                        EnviarEmail envio = new EnviarEmail();

                        usuario = new Usuario();
                        usuario = sQ_Usuario.getUsuario(listaAprobados[0].empleadoId);

                        if (usuario.email == null | usuario.email == "")
                        {
                            throw new Exception("No se encontró correo del empleado");
                        }
                        envio.EnviarInformativo(usuario.email, usuario.Nombres, false, listaAprobados[0].idSolicitud.ToString(),
                            "", listaAprobados[0].fechaRegistro, false, "");

                        hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.dlt_aprobadoresRd), rendicionId);
                        hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarEstadoRD), "15", "", rendicionId);

                        return Global.ReturnOk(lista, "No se rechazo correctamente");
                    }
                }
                else
                {
                    throw new Exception("No se tiene rendición a rechazar");
                }
            }
            catch (Exception ex)
            {
                return Global.ReturnError<string>(ex);
            }
        }

        public List<Aprobador> obtieneAprobadoresDet(string idArea, string aprobadorId, string fecha)
        {
            List<Aprobador> aprobadors = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_listaAprobadoresDetRd), dc =>
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

        public ConsultationResponse<Aprobador> ObtieneAprobadores(string idRendicion)
        {
            try
            {
                List<Aprobador> aprobadors = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_listaAprobadoresCabRd), dc =>
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
                }, idRendicion).ToList();


                return Global.ReturnOk(aprobadors, "");

            }
            catch (Exception ex)
            {
                return Global.ReturnError<Aprobador>(ex);
            }
        }
        public ConsultationResponse<FileRS> ObtieneAdjuntos(string idRendicion)
        {
            var respIncorrect = "No tiene adjuntos";
            List<FileRS> files = null;
            List<string> adjuntos = null;
            try
            {
                adjuntos = new List<string>();

                adjuntos = hash.GetValueSql(SQ_QueryManager.Generar(SQ_Query.get_adjuntosDoc), idRendicion).ToString().Split(',').ToList();

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
        public ConsultationResponse<CreateResponse> ReintentarRendicion(string rendiId)
        {
            List<CreateResponse> list = new List<CreateResponse>();
            Rendicion rendicion = new Rendicion();
            string respIncorrect = "No se concreto la migración";
            List<Aprobador> aprobadores = new List<Aprobador>();


            try
            {
                EnviarEmail envio = new EnviarEmail();
                rendicion = ObtenerRendicion(rendiId).Result[0];

                var response = GenerarRegistroDeRendicion(rendicion);

                if (response.IsSuccessful)
                {

                    CreateResponse createResponse = JsonConvert.DeserializeObject<CreateResponse>(response.Content);
                    createResponse.RML_APROBACIONFINALIZADA = 1;

                    // Inserts despues de crear EAR en SAP
                    hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarEstadoRD), "16", "", rendiId);
                    hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarMigradaRD), createResponse.DocEntry, createResponse.DocNum, rendicion.ID);

                    //  aprobadores = ObtieneAprobadores(rendicion.ID.ToString()).Result;
                    //aprobadores = ObtieneAprobadores(rendicion.ID.ToString()).Result;
                    //hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_migradaRdenSAP), aprobadores[1].aprobadorNombre, aprobadores.Count > 2 ? aprobadores[2].aprobadorNombre : null, aprobadores[0].aprobadorNombre, createResponse.DocEntry);

                    list.Add(createResponse);
                    // Envio de correo


                    envio.EnviarInformativo(rendicion.RML_EMPLEADO_ASIGNADO.email, rendicion.RML_EMPLEADO_ASIGNADO.Nombres, false,
                        rendicion.ID.ToString(), rendicion.RML_NRRENDICION, DateTime.Now.ToString("dd/MM/yyyy"), true, "");

                    return Global.ReturnOk(list, respIncorrect);
                }
                else
                {
                    string mensaje = JsonConvert.DeserializeObject<ErrorSL>(response.Content).error.message.value;
                    hash.insertValueSql(SQ_QueryManager.Generar(SQ_Query.upd_cambiarEstadoRD), "17", mensaje.Replace("'", ""), rendiId);
                    //envio.EnviarError(false, rendicion.RML_NRRENDICION, rendicion.ID.ToString(), rendicion.RML_FECHAREGIS);
                    throw new Exception(mensaje);
                }
            }
            catch (Exception ex)
            {
                return Global.ReturnError<CreateResponse>(ex);

            }

        }
        public IRestResponse GenerarRegistroDeRendicion(Rendicion rendicion)
        {
            List<RendicionDetSerializer> detalles = new List<RendicionDetSerializer>();

            // Obtiene Aprobadores
            List<Aprobador> aprobadores = new List<Aprobador>();
            aprobadores = ObtieneAprobadores(rendicion.ID.ToString()).Result;

            rendicion.documentos.ForEach((e) =>
            {
                Documento doc = new Documento();
                doc = ObtenerDocumento(e.ID.ToString()).Result[0];
                doc.detalles?.ForEach((d) =>
                {
                    detalles.Add(
                        new RendicionDetSerializer()
                        {
                            // doc = Cabecera
                            // d = detalle 
                            CardCode = doc.RML_PROVEEDOR.CardCode,
                            CardName = doc.RML_PROVEEDOR.CardName,
                            //RUC = doc.RML_PROVEEDOR.LicTradNum,
                            Comentarios = doc.RML_COMENTARIOS,
                            TipoDocumento = doc.RML_TIPO_DOC.id,
                            SerieDocumento = doc.RML_SERIE_DOC,
                            CorrelativoDocumento = doc.RML_CORR_DOC.PadLeft(8, '0'),
                            //FechaEmision = DateTime.ParseExact(doc.RML_FECHA_DOC, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),//DateTime.Parse(doc.RML_FECHA_DOC).ToString("yyyy-MM-dd"),
                            //FechaVencimiento = DateTime.ParseExact(doc.RML_FECHA_VENCIMIENTO, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),//DateTime.Parse(doc.RML_FECHA_VENCIMIENTO).ToString("yyyy-MM-dd"),
                            Moneda = doc.RML_MONEDA.name,
                            ItemCode = d.RML_CODARTICULO?.ItemCode,
                            Descripcion = d.RML_CODARTICULO.ItemName.Length > 50 ? d.RML_CODARTICULO.ItemName.Substring(0, 50) : d.RML_CODARTICULO.ItemName,
                            Almacen = d.RML_ALMACEN?.id,
                            FechaConta = DateTime.ParseExact(doc.RML_FECHA_DOC, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),
                            FechaDocumento =  DateTime.ParseExact(doc.RML_FECHA_DOC, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),
                            Dimension1 = d.RML_DIM1?.id,
                            Dimension3 = d.RML_DIM3?.id,
                            PrecioUnidad = d.RML_PRECIO,
                            PrecioTotal = d.RML_SUBTOTAL,
                            IndicadorImpuesto = d.RML_INDIC_IMPUESTO.code,
                            //Impuesto = d.RML_INDIC_IMPUESTO.id,
                            //PrecioUnidad = d.RML_SUBTOTAL / d.RML_CANTIDAD,
                            //TotalLinea = d.RML_SUBTOTAL,
                            //CentroDCosto = d.RML_CENTCOSTO.CostCenter,
                            //PosFinanciera = d.RML_POS_FINANCIERA.name,
                            //CUP = d.RML_CUP.U_CUP,
                            //Proyecto = d.RML_PROYECTO?.id,
                            //TipoOperacion = doc.RML_OPERACION == "1" ? "01" : doc.RML_OPERACION,
                            //Almacen = d.RML_ALMACEN,
                            Cantidad = d.RML_CANTIDAD,
                           // Retencion = doc.RML_TIPO_AGENTE.id == "0" ? "N" : doc.RML_TIPO_AGENTE.id == "1" ? "Y" : "N",
                            // Valores de la migración 
                            RutaAdjunto = doc.RML_ANEXO_ADJUNTO,
                            // Valores por Defecto
                            U_ER_ESTD = "CRE",
                            U_ER_SLCC = "Y",
                           // U_ObjType = "18",
                            Inventario = "I",
                            U_ER_PW_TPDC = doc.RML_WLIABLE?.code,
                            // = doc.RML_ANEXO_ADJUNTO
                        }
                        );
                });
            });

            RendicionSerializer body = new RendicionSerializer()
            {
                NumRendicion = rendicion.RML_NRRENDICION,
                Moneda = rendicion.documentos[0].RML_MONEDA.id,
                FechaCargaDocs = DateTime.Now.ToString("yyyy-MM-dd"),   // Por Defecto se asigna del día de hoy
                DocsTotal = rendicion.RML_TOTALRENDIDO,

                SaldoApertura = rendicion.RML_TOTALAPERTURA,
                UsuarioEARCod = rendicion.RML_EMPLEADO_ASIGNADO.numeroEAR,

                // Valores de la migración
                //PrimerAutorizador = aprobadores[1].aprobadorNombre,
                //SegundoAutorizador = aprobadores.Count > 2 ? aprobadores[2].aprobadorNombre : null,
                //ContableAutorizador = aprobadores[0].aprobadorNombre,
                // valores en defecto
                //Estado = "G",
                //TipoActividad = "G",                                    // por defecto se asigna G
                SaldoFinal = 0.0,
                //TipoRendicion = "EAR",

                //U_STR_WEB_EMPASIG = rendicion.RML_EMPLDREGI.ToString(),
                PrimerAutorizador = aprobadores[1].aprobadorId.ToString(),
                SegundoAutorizador = aprobadores.Count > 2 ? aprobadores[2].aprobadorId.ToString() : null,
                ContableAutorizador = aprobadores[0].aprobadorId.ToString(),
                //U_RML_WEB_EMPASIG = rendicion.RML_EMPLDREGI.ToString(),

                STR_EARCRGDETCollection = detalles
            };

            B1SLEndpoint sl = new B1SLEndpoint();
            string json = JsonConvert.SerializeObject(body);
            IRestResponse response = sl.CreateCargaDocumentos(json);

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
                    SourcePath = System.IO.Path.GetDirectoryName(e),
                    FileName = System.IO.Path.GetFileNameWithoutExtension(e),
                    FileExtension = System.IO.Path.GetExtension(e),
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
    }
}
