using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace STR_SERVICE_INTEGRATION_EAR
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class TokenAuthorizationAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext) 
        {
            if (actionContext.Request.Headers.Authorization != null)
            {
                // Obtener el token de la cabecera de autorización
                string token = actionContext.Request.Headers.Authorization.Parameter;

                // Validar el token (implementa la lógica según tu aplicación)
                return true;
            }

            return false;
        }
    }
}