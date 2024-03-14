using RestSharp;
using STR_SERVICE_INTEGRATION_EAR.EL.Commons;
using STR_SERVICE_INTEGRATION_EAR.SL.B1SL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.SL
{
    public class B1SLEndpoint : B1SLTransactions
    {
        public override void Get()
        {
            throw new NotImplementedException();
        }

        public dynamic Get<T>(string resource)
        {
            var rslt = default(RestSharp.IRestResponse);
            rslt = sLInteract.httpGET(getBasePath() + resource, SessionId);

            var objeto = System.Text.Json.JsonSerializer.Deserialize<T>(rslt.Content);

            return objeto;
        }

        public override U POST<U>(string strJSON)
        {
            throw new NotImplementedException();
        }

        public IRestResponse CreateAttachments(string strJSON)
        {

            var rslt = default(RestSharp.IRestResponse);
            rslt = sLInteract.httpPOST("Attachments2", SessionId, strJSON);

            return rslt;

        }

        public IRestResponse CreateOrdenSL(string strJSON) {

            var rslt = default(RestSharp.IRestResponse);
            rslt = sLInteract.httpPOST("PurchaseRequests", SessionId,strJSON);

            return rslt;

        }

        public IRestResponse CreateCargaDocumentos(string strJSON)
        {

            var rslt = default(RestSharp.IRestResponse);
            rslt = sLInteract.httpPOST("STR_EARCRG", SessionId, strJSON);

            return rslt;
        }



    }
}
