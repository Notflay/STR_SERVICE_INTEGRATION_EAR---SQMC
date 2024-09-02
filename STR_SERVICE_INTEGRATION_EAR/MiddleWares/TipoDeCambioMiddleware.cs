using Microsoft.Owin;
using STR_SERVICE_INTEGRATION_EAR.BL;
using STR_SERVICE_INTEGRATION_EAR.SQ;
using System;
using System.Threading.Tasks;

public class TipoDeCambioMiddleware : OwinMiddleware
{
    public TipoDeCambioMiddleware(OwinMiddleware next) : base(next)
    {
    }

    public override async Task Invoke(IOwinContext context)
    {
        try
        {
            // Realiza la validaci�n del tipo de cambio
            SqlADOHelper sqlADOHelper = new SqlADOHelper();
            object val = sqlADOHelper.GetValueSql(SQ_QueryManager.Generar(SQ_Query.get_tipoDeCambio));

            if (val == null || Convert.ToInt32(val) == 0)
            {
                context.Response.StatusCode = 400; // Bad Request
                await context.Response.WriteAsync("No se registr� el tipo de cambio de hoy en SAP");
                return;
            }

            // Si la validaci�n es correcta, contin�a con la solicitud
            await Next.Invoke(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500; // Internal Server Error
            await context.Response.WriteAsync("Error en la validaci�n del tipo de cambio: " + ex.Message);
        }
    }
}
