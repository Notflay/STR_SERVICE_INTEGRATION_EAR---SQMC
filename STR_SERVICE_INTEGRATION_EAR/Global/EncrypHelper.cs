using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace STR_SERVICE_INTEGRATION_EAR
{
    public static class EncrypHelper
    {
        public static string GenerarToken(string username)
        {
            // Configuración del ticket de autenticación
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,                           // Número de versión del ticket
                username,                    // Nombre de usuario asociado al ticket
                DateTime.Now,                // Fecha y hora de emisión del ticket
                DateTime.Now.AddMinutes(30), // Duración del ticket (aquí puedes ajustar la duración)
                false,                       // Si se persiste el ticket en una cookie
                "cookie_pw");           // Ruta de la cookie (puedes ajustarla según tu configuración)

            // Encriptar el ticket y convertirlo en un string
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            return encryptedTicket;
        }
    }
}