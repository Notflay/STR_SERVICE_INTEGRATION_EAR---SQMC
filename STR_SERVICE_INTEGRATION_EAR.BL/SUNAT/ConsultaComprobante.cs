using STR_SERVICE_INTEGRATION_EAR.EL.Requests;
using STR_SERVICE_INTEGRATION_EAR.EL.Responses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.BL
{
    public class ConsultaComprobante
    {
        public string grant_type { get; set; }
        public string scope { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string ruc { get; set; }
        public ConsultaComprobante()
        {
            grant_type = ConfigurationManager.AppSettings["grant_type"].ToString();
            scope = ConfigurationManager.AppSettings["scope"].ToString();
            client_id = ConfigurationManager.AppSettings["client_id"].ToString();
            client_secret = ConfigurationManager.AppSettings["client_secret"].ToString();
            ruc = ConfigurationManager.AppSettings["ruc"].ToString();
        }
        public TokenResponses GeneraTokenSUNAT()
        {
            TokenResponses resp = new TokenResponses();
            try
            {
                var param = new Dictionary<string, string>();
                param.Add("grant_type", grant_type);
                param.Add("scope", scope);
                param.Add("client_id", client_id);
                param.Add("client_secret", client_secret);

                string url = $"https://api-seguridad.sunat.gob.pe/v1/clientesextranet/{client_id}/oauth2/token/";

                using (var httpClientHandler = new HttpClientHandler())
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                    using (var httpClient = new HttpClient(httpClientHandler))
                    {
                        httpClient.Timeout = TimeSpan.FromSeconds(60000);
                        var result = httpClient.PostAsync(new Uri(url), new FormUrlEncodedContent(param)).Result;
                        var resultContent2 = result.Content.ReadAsStringAsync().Result;
                        resp = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponses>(resultContent2);
                    }
                }
                return resp;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ComprobanteResponses ValidarDocumento(ComprobanteRequest comprobante)
        {
            // 
            bool prod = ConfigurationManager.AppSettings["Prod"] == "1";

            string url = $"https://api.sunat.gob.pe/v1/contribuyente/contribuyentes/{ruc}/validarcomprobante";
            ComprobanteResponses resultContent = new ComprobanteResponses();
            TokenResponses token = GeneraTokenSUNAT();

            try
            {
                using (var httpClientHandler = new HttpClientHandler())
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                    using (var httpClient = new HttpClient(httpClientHandler))
                    {
                        httpClient.Timeout = TimeSpan.FromSeconds(60000);

                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.access_token);
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(comprobante);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        var result = httpClient.PostAsync(new Uri(url), content).Result;
                        var resultContent2 = result.Content.ReadAsStringAsync().Result;

                        resultContent = Newtonsoft.Json.JsonConvert.DeserializeObject<ComprobanteResponses>(resultContent2);

                    }
                }

                if (!prod) {
                    resultContent.data = new Data();
                    resultContent.data.estadoCp = "1";
                }

                return resultContent;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
