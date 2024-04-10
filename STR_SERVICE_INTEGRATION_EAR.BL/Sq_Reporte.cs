using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using STR_SERVICE_INTEGRATION_EAR.SQ;
using Sap.Data.Hana;
using STR_SERVICE_INTEGRATION_EAR.EL.Commons;
using STR_SERVICE_INTEGRATION_EAR.EL.Responses;
using RestSharp.Extensions;
using System.Net.Http;
using System.Net;

namespace STR_SERVICE_INTEGRATION_EAR.BL
{
    public class Sq_Reporte
    {
        public static string UserName { get; set; }
        public static string Password { get; set; }
        public static string DbName { get; set; }
        public static string Server { get; set; }

        public Sq_Reporte()
        {
            /*
            HanaConnectionManager hanaConnection = new HanaConnectionManager();
            HanaConnection hana = hanaConnection.GetConnection();
            
            UserName = hana.Credential.UserId;
            Password = hana.Credential.Password.ToString();
            DbName = hana.Database;
            Server = hana.DataSource;
            */
            UserName = ConfigurationManager.AppSettings["db_username"].ToString();
            Password = ConfigurationManager.AppSettings["db_password"].ToString();
            DbName = ConfigurationManager.AppSettings["CompanyDB"].ToString();
            Server = ConfigurationManager.AppSettings["server_crystal"].ToString();
        }
        public static byte[] GenerarReporteSLB(string docEntry, string numRendicion, string tipo)
        {


            switch (tipo)
            {
                case "VIA":
                    tipo = "format_solicitud_dinero";
                    break;
                case "GAU":
                    tipo = "format_gastos_urgentes";
                    break;
                case "REC":
                    tipo = "format_recibo_provicional";
                    break;
                case "ORD":
                    tipo = "format_orden_viaje";
                    break;
                default:
                    throw new NotImplementedException();
            }


            string rutaSL = ConfigurationManager.AppSettings[tipo].ToString();

            ReportDocument rptDoc = new ReportDocument();

            rptDoc.Load(rutaSL);
            rptDoc.PrintOptions.DissociatePageSizeAndPrinterPaperSize = true;
            rptDoc.PrintOptions.PaperSize = PaperSize.PaperA4;

            rptDoc.SetParameterValue("DocKey@", docEntry); // 6195

            // Conectar HANA
            rptDoc.SetParameterValue("Schema@", DbName);

            string driver = Environment.Is64BitProcess ? "{HDBODBC}" : "{HDBODBC32}";
            string strConnection = string.Format("DRIVER={0};UID={1};PWD={2};SERVERNODE={3};DATABASE={4};", driver, UserName, Password, Server, DbName);

            // Configura
            NameValuePairs2 logonProps2 = rptDoc.DataSourceConnections[0].LogonProperties;
            logonProps2.Set("Provider", Environment.Is64BitProcess ? "{HDBODBC}" : "{HDBODBC32}");
            logonProps2.Set("Server Type", Environment.Is64BitProcess ? "{HDBODBC}" : "{HDBODBC32}");
            logonProps2.Set("Connection String", strConnection);
            logonProps2.Set("Locale Identifier", "1033");

            rptDoc.DataSourceConnections[0].SetLogonProperties(logonProps2);
            rptDoc.DataSourceConnections[0].SetConnection(Server, DbName, UserName, Password);

            byte[] pdfByteArray;
            pdfByteArray = rptDoc.ExportToStream(ExportFormatType.PortableDocFormat).ReadAsBytes();
            //memoryStream.Seek(0, SeekOrigin.Begin);
            rptDoc.Close();
            rptDoc.Dispose();
            return pdfByteArray;

        }

        public static byte[] GenerarReporteEAR(string numRendicion)
        {

            string rutaSL = ConfigurationManager.AppSettings["format_liquidacion_ear"].ToString();

            ReportDocument rptDoc = new ReportDocument();

            rptDoc.Load(rutaSL);
            rptDoc.PrintOptions.DissociatePageSizeAndPrinterPaperSize = true;
            rptDoc.PrintOptions.PaperSize = PaperSize.PaperA4;

            rptDoc.SetParameterValue("DocKey@", numRendicion); // 6195

            // Conectar HANA
            rptDoc.SetParameterValue("Schema@", DbName);

            string driver = Environment.Is64BitProcess ? "{HDBODBC}" : "{HDBODBC32}";
            string strConnection = string.Format("DRIVER={0};UID={1};PWD={2};SERVERNODE={3};DATABASE={4};", driver, UserName, Password, Server, DbName);

            // Configura
            NameValuePairs2 logonProps2 = rptDoc.DataSourceConnections[0].LogonProperties;
            logonProps2.Set("Provider", Environment.Is64BitProcess ? "{HDBODBC}" : "{HDBODBC32}");
            logonProps2.Set("Server Type", Environment.Is64BitProcess ? "{HDBODBC}" : "{HDBODBC32}");
            logonProps2.Set("Connection String", strConnection);
            logonProps2.Set("Locale Identifier", "1033");

            rptDoc.DataSourceConnections[0].SetLogonProperties(logonProps2);
            rptDoc.DataSourceConnections[0].SetConnection(Server, DbName, UserName, Password);

            byte[] pdfByteArray;
            pdfByteArray = rptDoc.ExportToStream(ExportFormatType.PortableDocFormat).ReadAsBytes();
            //memoryStream.Seek(0, SeekOrigin.Begin);
            rptDoc.Close();
            rptDoc.Dispose();
            return pdfByteArray;
        }
        public HttpResponseMessage CreaReporte(string docEntry, string numRendicion, string tipo)
        {
            var respOk = "OK";
            var respIncorrect = "No se generó reporte";
            SqlADOHelper hash = new SqlADOHelper();

            try
            {
                byte[] pdfByteArray = GenerarReporteSLB(docEntry, numRendicion, tipo);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(pdfByteArray)
                };

                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = "PlantillaPortalEAR.pdf"
                };

                return response;
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public HttpResponseMessage CrearReporteEAR(string numRendicion)
        {
            var respOk = "OK";
            var respIncorrect = "No se generó reporte";
            SqlADOHelper hash = new SqlADOHelper();

            try
            {
                byte[] pdfByteArray = GenerarReporteEAR(numRendicion);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(pdfByteArray)
                };

                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = "PlantillaPortalEAR.pdf"
                };

                return response;
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

        }

    }
}
