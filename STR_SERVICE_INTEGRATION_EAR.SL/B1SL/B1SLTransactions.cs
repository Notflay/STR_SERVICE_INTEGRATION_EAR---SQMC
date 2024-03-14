using RestSharp.Serialization.Json;
using STR_SERVICE_INTEGRATION_EAR.EL;
using STR_SERVICE_INTEGRATION_EAR.EL.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace STR_SERVICE_INTEGRATION_EAR.SL.B1SL
{
    public abstract class B1SLTransactions
    {
        private B1SLLoginDAO loginDAO = null;
        protected string _sessionId = null;
        protected B1SLInteract sLInteract = null;

        public B1SLTransactions()
        {
            loginDAO = new B1SLLoginDAO();
            sLInteract = new B1SLInteract(getBasePath());
        }
        public abstract void Get();
        public abstract U POST<U>(string strJSON);
        protected string getBasePath()
        {
            return new UriBuilder()
            {
                Scheme = WebConfigurationManager.AppSettings["Scheme"],
                Host = WebConfigurationManager.AppSettings["Host"],
                Port = Convert.ToInt32(WebConfigurationManager.AppSettings["Port"]),
                Path = WebConfigurationManager.AppSettings["BasePath"]
            }.ToString();
        }

        protected string SessionId
        {
            get
            {
                return _sessionId = _sessionId ?? getSessionID();
            }
        }

        private string getSessionID()
        {
            //var loginResponse = JsonConvert.DeserializeObject<B1SLLoginResponse>
            //    (sLInteract.httpPOST("Login", string.Empty, (B1SLLogin)loginDAO.read("1")).Content);
            var loginResponse = sLInteract.httpPOST("Login", string.Empty, (B1SLLogin)loginDAO.read("1")).Content;

            B1SLLoginResponse reponse = System.Text.Json.JsonSerializer.Deserialize<B1SLLoginResponse>(loginResponse);
            return reponse.SessionId;
        }

        public void Logout()
        {
            var response = sLInteract.httpPOST("Logout", SessionId, null);
            if (!response.IsSuccessful) throw new InvalidOperationException(response.StatusDescription);
            _sessionId = null;
        }
    }
}
