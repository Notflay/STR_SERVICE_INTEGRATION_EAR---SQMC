using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace STR_SERVICE_INTEGRATION_EAR
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var allowedServers = ConfigurationManager.AppSettings["AllowedCorsOrigins"];
            // Configuración y servicios de Web API
            var cors = new EnableCorsAttribute(allowedServers, "*", "*");
            config.EnableCors(cors);
            // Rutas de Web API
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
