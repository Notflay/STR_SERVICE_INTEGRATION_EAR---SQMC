using Microsoft.Owin;
using System;
using System.IO;
using System.Threading.Tasks;
using STR_SERVICE_INTEGRATION_EAR.BL;
using System.Security.Claims;
public class LoggingMiddleware : OwinMiddleware
{
    public LoggingMiddleware(OwinMiddleware next) : base(next) { }

    public async override Task Invoke(IOwinContext context)
    {
        // Registrar la solicitud
        LogRequest(context);

        // Continuar con la siguiente fase del pipeline
        await Next.Invoke(context);
    }

    private void LogRequest(IOwinContext context)
    {
        // Obtener la información del usuario desde el token JWT
        var identity = context.Authentication.User.Identity as ClaimsIdentity;
        var userId = identity?.FindFirst("id")?.Value;
        var userName = identity?.Name;
        var userRole = identity?.FindFirst("rol")?.Value;

        // Obtener la dirección IP del usuario
        var ipAddress = context.Request.RemoteIpAddress;

        // Obtener la información de la solicitud
        var request = context.Request;
        var message = $"User: {userId} - {userName} (Role: {userRole}) " +
                      $"IP: {ipAddress} Request: {request.Method} {request.Uri}";

        // Escribir el log en un archivo (puedes cambiar la ubicación y el formato según sea necesario)
        Global.WriteToFile(message);
    }
}