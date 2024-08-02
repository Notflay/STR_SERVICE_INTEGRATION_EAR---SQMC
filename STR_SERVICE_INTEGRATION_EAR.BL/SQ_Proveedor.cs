using STR_SERVICE_INTEGRATION_EAR.EL.Commons;
using STR_SERVICE_INTEGRATION_EAR.EL.Responses;
using STR_SERVICE_INTEGRATION_EAR.SQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.BL
{
    public class SQ_Proveedor
    {
        SqlADOHelper hash = new SqlADOHelper();
        public ConsultationResponse<Proveedor> ObtenerProveedores()
        {
            var respIncorrect = "No Hay Proveedores";

            try
            {
                List<Proveedor> list = hash.GetResultAsTypeDirecta(SQ_QueryManager.Generar(SQ_Query.get_proveedores), dc =>
                {
                    return new Proveedor()
                    {
                        CardCode = dc["CardCode"],
                        CardName = dc["CardName"],
                        LicTradNum = dc["LicTradNum"]
                    };
                }, string.Empty).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Proveedor>(ex);
            }
        }

        public ConsultationResponse<Proveedor> ObtenerProveedor(string id)
        {
            var respIncorrect = "No Hay Proveedores";

            try
            {
                List<Proveedor> list = hash.GetResultAsTypeDirecta(SQ_QueryManager.Generar(SQ_Query.get_proveedor), dc =>
                {
                    return new Proveedor()
                    {
                        CardCode = dc["CardCode"],
                        CardName = dc["CardName"],
                        LicTradNum = dc["LicTradNum"]
                    };
                }, id).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Proveedor>(ex);
            }
        }

        public async Task<ConsultationResponse<InfoRuc>> ObtenerInfoPorRuc(string ruc)
        {
            var respIncorrect = "No Hay Proveedores";
            ConsultRuc consulta = new ConsultRuc();
            List<InfoRuc> infoRucs = new List<InfoRuc>();
            try
            {
                var response = await consulta.ObtieneInfoPorRucAsync(ruc);

                infoRucs.Add(response);

                return Global.ReturnOk(infoRucs, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<InfoRuc>(ex);
            }
        }
    }
}
