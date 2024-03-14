using STR_SERVICE_INTEGRATION_EAR.EL.Commons;
using STR_SERVICE_INTEGRATION_EAR.EL.Responses;
using STR_SERVICE_INTEGRATION_EAR.SQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STR_SERVICE_INTEGRATION_EAR.BL
{
    public class Sq_Item
    {
        HanaADOHelper hash = new HanaADOHelper();
        public ConsultationResponse<Item> ObtenerItems(string ear, string sede)
        {
            var respIncorrect = "No se encuentra Items";

            try
            {
                List<Item> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_items), dc =>
                {
                    return new Item()
                    {
                        ItemCode = dc["ItemCode"],
                        ItemName = dc["ItemName"],
                        id = dc["ItemCode"],
                        name = dc["ItemName"],
                        posFinanciera = dc["POSFINANCIERA"],
                        CTA = dc["CTA"]
                    };
                }, ear, sede).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Item>(ex);
            }
        }

        public ConsultationResponse<Item> ObtenerItem(string itemCode)
        {
            var respIncorrect = "No se encuentra Items";

            try
            {
                List<Item> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_item), dc =>
                {
                    return new Item()
                    {
                        ItemCode = dc["id"],
                        ItemName = dc["name"],
                        id = dc["id"],
                        name = dc["name"],
                       // POSFINANCIERA = dc["posFinanciera"],
                        posFinanciera = dc["posFinanciera"],
                        CTA = dc["CTA"]

                    };
                }, itemCode).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Item>(ex);
            }
        }

        public ConsultationResponse<Cup> ObtenerCUP(int? ceco, int? posFinanciera, int anio)
        {
            var respIncorrect = "No se encuentra Cup del item";
            List<Cup> list = new List<Cup>();
            try
            {
                //if (ceco )

                list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_obtenCup), dc =>
                {
                    return new Cup()
                    {
                        CRP = Convert.ToInt32(dc["CRP"]),
                        Partida = Convert.ToInt32(dc["Partida"]),
                        U_CUP = dc["U_CUP"],
                        U_DESCRIPTION = dc["U_DESCRIPTION"]
                    };
                }, ceco.ToString(), posFinanciera.ToString(), DateTime.Now.Year.ToString()).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Cup>(ex);
            }
        }

        public ConsultationResponse<Precio> ObtenerPrecio(string provincia, string distrito, string itemCode)
        {
            var respIncorrect = "No se encuentró precio en la tabla con este Articulo";

            try
            {
                List<Precio> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_precioUnitario), dc =>
                {
                    return new Precio()
                    {
                        Id = 01,
                        precio = Convert.ToDouble(string.IsNullOrEmpty(dc["precio"]) ? 0.00 : Convert.ToDouble(dc["precio"]))
                    };
                }, provincia, distrito, itemCode).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Precio>(ex);
            }
        }

        public ConsultationResponse<Complemento> ObtenerProyectos()
        {
            var respIncorrect = "No se encuentró la lista de proyectos";

            try
            {
                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_proyectos), dc =>
                {
                    return new Complemento()
                    {
                        id = dc["id"],
                        name = dc["name"]
                    };
                }, string.Empty).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }
        public ConsultationResponse<Complemento> ObtenerProyecto(string id)
        {
            var respIncorrect = "No se encuentró el proyecto Asignado";
            List<Complemento> list = new List<Complemento>();
            try
            {
                list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_proyecto), dc =>
                {
                    return new Complemento()
                    {
                        id = dc["id"],
                        name = dc["name"]
                    };
                }, id).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }

        public ConsultationResponse<Complemento> ObtenerIndicadores()
        {
            var respIncorrect = "No se encontró indicadores";

            try
            {
                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_indicadores), dc =>
                {
                    return new Complemento()
                    {
                        id = dc["id"],
                        name = dc["name"]
                    };
                }, string.Empty).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }

        public ConsultationResponse<Complemento> ObtenerIndicador(string id)
        {
            var respIncorrect = "No se encontró indicador";

            try
            {
                List<Complemento> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_indicador), dc =>
                {
                    return new Complemento()
                    {
                        id = dc["id"],
                        name = dc["name"]
                    };
                }, id).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Complemento>(ex);
            }
        }

    }
}
