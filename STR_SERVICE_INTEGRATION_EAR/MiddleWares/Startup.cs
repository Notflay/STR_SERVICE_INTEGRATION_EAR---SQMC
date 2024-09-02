using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;

[assembly: OwinStartup(typeof(STR_SERVICE_INTEGRATION_EAR.Startup))]

namespace STR_SERVICE_INTEGRATION_EAR
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var key = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["secret"]); // Clave secreta del JWT

           
            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
               
                TokenValidationParameters = new TokenValidationParameters
                {
                    
                    ValidateIssuer = false, // Ajustar según sea necesario
                    ValidateAudience = false, // Ajustar según sea necesario
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // Sin margen de tolerancia para la expiración
                }
            });

            app.Use<LoggingMiddleware>();

            app.Use<TipoDeCambioMiddleware>();
        }
    }
}
