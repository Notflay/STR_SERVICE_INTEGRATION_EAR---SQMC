using STR_SERVICE_INTEGRATION_EAR.EL.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using STR_SERVICE_INTEGRATION_EAR.EL.Responses;

namespace STR_SERVICE_INTEGRATION_EAR.BL
{
    public class ConsultRuc
    {
        private SUNAT ObtenerDatos(string contenidoHTML)
        {
            Codigo oCuTexto = new Codigo();
            SUNAT oEnSUNAT = new SUNAT();
            string nombreInicio = "<HEAD><TITLE>";
            string nombreFin = "</TITLE></HEAD>";
            string contenidoBusqueda = oCuTexto.ExtraerContenidoEntreTagString(contenidoHTML, 0, nombreInicio, nombreFin);
            if (contenidoBusqueda == ".:: Pagina de Mensajes ::.")
            {
                nombreInicio = "<p class=\"error\">";
                nombreFin = "</p>";
                oEnSUNAT.TipoRespuesta = 2;
                oEnSUNAT.MensajeRespuesta = oCuTexto.ExtraerContenidoEntreTagString(contenidoHTML, 0, nombreInicio, nombreFin);
            }
            else if (contenidoBusqueda == ".:: Pagina de Error ::.")
            {
                nombreInicio = "<p class=\"error\">";
                nombreFin = "</p>";
                oEnSUNAT.TipoRespuesta = 3;
                oEnSUNAT.MensajeRespuesta = oCuTexto.ExtraerContenidoEntreTagString(contenidoHTML, 0, nombreInicio, nombreFin);
            }
            else
            {
                oEnSUNAT.TipoRespuesta = 2;
                nombreInicio = "<div class=\"list-group\">";
                nombreFin = "<div class=\"panel-footer text-center\">";
                contenidoBusqueda = oCuTexto.ExtraerContenidoEntreTagString(contenidoHTML, 0, nombreInicio, nombreFin);
                if (contenidoBusqueda == "")
                {
                    nombreInicio = "<strong>";
                    nombreFin = "</strong>";
                    oEnSUNAT.MensajeRespuesta = oCuTexto.ExtraerContenidoEntreTagString(contenidoHTML, 0, nombreInicio, nombreFin);
                    if (oEnSUNAT.MensajeRespuesta == "")
                        oEnSUNAT.MensajeRespuesta = "No se encuentra las cabeceras principales del contenido HTML";
                }
                else
                {
                    contenidoHTML = contenidoBusqueda;
                    oEnSUNAT.MensajeRespuesta = "Mensaje del inconveniente no especificado";
                    nombreInicio = "<h4 class=\"list-group-item-heading\">";
                    nombreFin = "</h4>";
                    int resultadoBusqueda = contenidoHTML.IndexOf(nombreInicio, 0, StringComparison.OrdinalIgnoreCase);
                    if (resultadoBusqueda > -1)
                    {
                        // Modificar cuando el estado del Contribuyente es "BAJA DE OFICIO", porque se agrega un elemento con clase "list-group-item"
                        resultadoBusqueda += nombreInicio.Length;
                        string[] arrResultado = oCuTexto.ExtraerContenidoEntreTag(contenidoHTML, resultadoBusqueda,
                            nombreInicio, nombreFin);
                        if (arrResultado != null)
                        {
                            oEnSUNAT.RazonSocial = arrResultado[1];

                            // Tipo Contribuyente
                            nombreInicio = "<p class=\"list-group-item-text\">";
                            nombreFin = "</p>";
                            arrResultado = oCuTexto.ExtraerContenidoEntreTag(contenidoHTML, Convert.ToInt32(arrResultado[0]),
                                nombreInicio, nombreFin);
                            if (arrResultado != null)
                            {
                                oEnSUNAT.TipoContribuyente = arrResultado[1];

                                // Nombre Comercial
                                arrResultado = oCuTexto.ExtraerContenidoEntreTag(contenidoHTML, Convert.ToInt32(arrResultado[0]),
                                    nombreInicio, nombreFin);
                                if (arrResultado != null)
                                {
                                    oEnSUNAT.NombreComercial = arrResultado[1].Replace("\r\n", "").Replace("\t", "").Trim();

                                    // Fecha de Inscripción
                                    arrResultado = oCuTexto.ExtraerContenidoEntreTag(contenidoHTML, Convert.ToInt32(arrResultado[0]),
                                        nombreInicio, nombreFin);
                                    if (arrResultado != null)
                                    {
                                        oEnSUNAT.FechaInscripcion = arrResultado[1];

                                        // Fecha de Inicio de Actividades: 
                                        arrResultado = oCuTexto.ExtraerContenidoEntreTag(contenidoHTML, Convert.ToInt32(arrResultado[0]),
                                            nombreInicio, nombreFin);
                                        if (arrResultado != null)
                                        {
                                            oEnSUNAT.FechaInicioActividades = arrResultado[1];

                                            // Estado del Contribuyente
                                            arrResultado = oCuTexto.ExtraerContenidoEntreTag(contenidoHTML, Convert.ToInt32(arrResultado[0]),
                                            nombreInicio, nombreFin);
                                            if (arrResultado != null)
                                            {
                                                oEnSUNAT.EstadoContribuyente = arrResultado[1].Trim();

                                                // Condición del Contribuyente
                                                arrResultado = oCuTexto.ExtraerContenidoEntreTag(contenidoHTML, Convert.ToInt32(arrResultado[0]),
                                                    nombreInicio, nombreFin);
                                                if (arrResultado != null)
                                                {
                                                    oEnSUNAT.CondicionContribuyente = arrResultado[1].Trim();

                                                    // Domicilio Fiscal
                                                    arrResultado = oCuTexto.ExtraerContenidoEntreTag(contenidoHTML, Convert.ToInt32(arrResultado[0]),
                                                        nombreInicio, nombreFin);
                                                    if (arrResultado != null)
                                                    {
                                                        oEnSUNAT.DomicilioFiscal = arrResultado[1].Trim();

                                                        // Actividad(es) Económica(s)
                                                        nombreInicio = "<tbody>";
                                                        nombreFin = "</tbody>";
                                                        arrResultado = oCuTexto.ExtraerContenidoEntreTag(contenidoHTML, Convert.ToInt32(arrResultado[0]),
                                                            nombreInicio, nombreFin);
                                                        if (arrResultado != null)
                                                        {
                                                            oEnSUNAT.ActividadesEconomicas = arrResultado[1].Replace("\r\n", "").Replace("\t", "").Trim();

                                                            // Comprobantes de Pago c/aut. de impresión (F. 806 u 816)
                                                            arrResultado = oCuTexto.ExtraerContenidoEntreTag(contenidoHTML, Convert.ToInt32(arrResultado[0]),
                                                                nombreInicio, nombreFin);
                                                            if (arrResultado != null)
                                                            {
                                                                oEnSUNAT.ComprobantesPago = arrResultado[1].Replace("\r\n", "").Replace("\t", "").Trim();

                                                                // Sistema de Emisión Electrónica
                                                                arrResultado = oCuTexto.ExtraerContenidoEntreTag(contenidoHTML, Convert.ToInt32(arrResultado[0]),
                                                                    nombreInicio, nombreFin);
                                                                if (arrResultado != null)
                                                                {
                                                                    oEnSUNAT.SistemaEmisionComprobante = arrResultado[1].Replace("\r\n", "").Replace("\t", "").Trim();

                                                                    // Afiliado al PLE desde
                                                                    nombreInicio = "<p class=\"list-group-item-text\">";
                                                                    nombreFin = "</p>";
                                                                    arrResultado = oCuTexto.ExtraerContenidoEntreTag(contenidoHTML, Convert.ToInt32(arrResultado[0]),
                                                                        nombreInicio, nombreFin);
                                                                    if (arrResultado != null)
                                                                    {
                                                                        oEnSUNAT.AfiliadoPLEDesde = arrResultado[1];

                                                                        // Padrones 
                                                                        nombreInicio = "<tbody>";
                                                                        nombreFin = "</tbody>";
                                                                        arrResultado = oCuTexto.ExtraerContenidoEntreTag(contenidoHTML, Convert.ToInt32(arrResultado[0]),
                                                                            nombreInicio, nombreFin);
                                                                        if (arrResultado != null)
                                                                        {
                                                                            oEnSUNAT.Padrones = arrResultado[1].Replace("\r\n", "").Replace("\t", "").Trim();
                                                                        }
                                                                    }

                                                                    oEnSUNAT.TipoRespuesta = 1;
                                                                    oEnSUNAT.MensajeRespuesta = "Ok";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return oEnSUNAT;
        }

        public async Task<InfoRuc> ObtieneInfoPorRucAsync(string RUC)
        {
            string ruc = RUC;

            Codigo codigo = new Codigo();

            CookieContainer cookies = new CookieContainer();
            HttpClientHandler controladorMensaje = new HttpClientHandler();
            controladorMensaje.CookieContainer = cookies;
            controladorMensaje.UseCookies = true;

            HttpClient httpClient = new HttpClient();

            using (HttpClient cliente = new HttpClient(controladorMensaje))
            {
                cliente.DefaultRequestHeaders.Add("Host", "e-consultaruc.sunat.gob.pe");
                cliente.DefaultRequestHeaders.Add("sec-ch-ua",
                    " \" Not A;Brand\";v=\"99\", \"Chromium\";v=\"90\", \"Google Chrome\";v=\"90\"");
                cliente.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
                cliente.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
                cliente.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
                cliente.DefaultRequestHeaders.Add("Sec-Fetch-Site", "none");
                cliente.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1");
                cliente.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
                cliente.DefaultRequestHeaders.Add("User-Agent",
                    "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.150 Safari/537.36");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 |
                                                       SecurityProtocolType.Tls12;
                //await Task.Delay(100);

                string url =
                    "https://e-consultaruc.sunat.gob.pe/cl-ti-itmrconsruc/jcrS00Alias";
                using (HttpResponseMessage resultadoConsulta = await cliente.GetAsync(new Uri(url)))
                {
                    if (resultadoConsulta.IsSuccessStatusCode)
                    {
                        await Task.Delay(100);
                        cliente.DefaultRequestHeaders.Remove("Sec-Fetch-Site");

                        cliente.DefaultRequestHeaders.Add("Origin", "https://e-consultaruc.sunat.gob.pe");
                        cliente.DefaultRequestHeaders.Add("Referer", url);
                        cliente.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");

                        string numeroDNI = "12345678";
                        var lClaveValor = new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>("accion", "consPorTipdoc"),
                            new KeyValuePair<string, string>("razSoc", ""),
                            new KeyValuePair<string, string>("nroRuc", ""),
                            new KeyValuePair<string, string>("nrodoc", numeroDNI),
                            new KeyValuePair<string, string>("contexto", "ti-it"),
                            new KeyValuePair<string, string>("modo", "1"),
                            new KeyValuePair<string, string>("search1", ""),
                            new KeyValuePair<string, string>("rbtnTipo", "2"),
                            new KeyValuePair<string, string>("tipdoc", "1"),
                            new KeyValuePair<string, string>("search2", numeroDNI),
                            new KeyValuePair<string, string>("search3", ""),
                            new KeyValuePair<string, string>("codigo", ""),
                        };
                        FormUrlEncodedContent contenido = new FormUrlEncodedContent(lClaveValor);

                        url = "https://e-consultaruc.sunat.gob.pe/cl-ti-itmrconsruc/jcrS00Alias";
                        using (HttpResponseMessage resultadoConsultaRandom = await cliente.PostAsync(url, contenido))
                        {
                            if (resultadoConsultaRandom.IsSuccessStatusCode)
                            {
                                await Task.Delay(100);
                                string contenidoHTML = await resultadoConsultaRandom.Content.ReadAsStringAsync();
                                string numeroRandom = codigo.ExtraerContenidoEntreTagString(contenidoHTML, 0, "name=\"numRnd\" value=\"", "\">");

                                lClaveValor = new List<KeyValuePair<string, string>>
                                {
                                    new KeyValuePair<string, string>("accion", "consPorRuc"),
                                    new KeyValuePair<string, string>("actReturn", "1"),
                                    new KeyValuePair<string, string>("nroRuc", ruc),
                                    new KeyValuePair<string, string>("numRnd", numeroRandom),
                                    new KeyValuePair<string, string>("modo", "1")
                                };
                                // Por si cae en el primer intento por el código "Unauthorized", en el buble se va a intentar hasta 3 veces "nConsulta"
                                int cConsulta = 0;
                                int nConsulta = 3;
                                HttpStatusCode codigoEstado = HttpStatusCode.Unauthorized;
                                while (cConsulta < nConsulta && codigoEstado == HttpStatusCode.Unauthorized)
                                {
                                    contenido = new FormUrlEncodedContent(lClaveValor);
                                    using (HttpResponseMessage resultadoConsultaDatos =
                                    await cliente.PostAsync(url, contenido))
                                    {
                                        codigoEstado = resultadoConsultaDatos.StatusCode;
                                        if (resultadoConsultaDatos.IsSuccessStatusCode)
                                        {
                                            contenidoHTML = await resultadoConsultaDatos.Content.ReadAsStringAsync();
                                            contenidoHTML = WebUtility.HtmlDecode(contenidoHTML);

                                            #region Obtener los datos del RUC
                                            SUNAT oEnSUNAT = ObtenerDatos(contenidoHTML);
                                            if (oEnSUNAT.TipoRespuesta == 1)
                                            {
                                                InfoRuc infoRuc = new InfoRuc();

                                                infoRuc.RUC = oEnSUNAT.NumeroRUC;
                                                infoRuc.RazonSocial = oEnSUNAT.RazonSocial.Substring(14);
                                                // oEnSUNAT.NombreComercial
                                                infoRuc.TipoContribuyente = oEnSUNAT.TipoContribuyente;
                                                infoRuc.EstadoContribuyente = oEnSUNAT.EstadoContribuyente;

                                                return infoRuc;
                                            }
                                            else
                                            {
                                                throw new Exception(string.Format(
                                                     "No se pudo realizar la consulta del número de RUC {0}.\r\nDetalle: {1}",
                                                     ruc,
                                                     oEnSUNAT.MensajeRespuesta));
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            throw new Exception(string.Format(
                                                    "Ocurrió un inconveniente al consultar los datos del RUC {0}.\r\nDetalle:{1}",
                                                    ruc));

                                        }
                                    }
                                }
                            }
                            else
                            {
                                throw new Exception(string.Format(
                                                    "Ocurrió un inconveniente al consultar los datos del RUC {0}.\r\nDetalle:{1}",
                                                    ruc));
                            }
                        }
                    }
                    else
                    {
                        throw new Exception(string.Format(
                                                    "Ocurrió un inconveniente al consultar los datos del RUC {0}.\r\nDetalle:{1}",
                                                    ruc));
                    }
                }
            }


            throw new Exception(string.Format(
                             "Ocurrió un inconveniente al consultar los datos del RUC {0}.\r\nDetalle:{1}",
                             ruc));
        }

    }
}
