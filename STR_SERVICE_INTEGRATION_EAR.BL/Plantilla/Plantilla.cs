using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml;
using STR_SERVICE_INTEGRATION_EAR.EL.Requests;
using STR_SERVICE_INTEGRATION_EAR.EL.Responses;

namespace STR_SERVICE_INTEGRATION_EAR.BL
{
    public class Plantilla
    {

        public static readonly string ruta = "D:\\Chamba Backend\\Electro Peru\\Plantillas\\PlantillaPortalEAR - Importacion 1.xlsx";
        public ExcelPackage package = null;

        public List<Documento> ObtienePlantilla(ExcelPackage excel, int id)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            package = new ExcelPackage();
            package = excel;


            ExcelWorksheet facturaSheet = package.Workbook.Worksheets["Cabecera"];

            int rowCount = facturaSheet.Dimension.Rows;
            int colCount = facturaSheet.Dimension.Columns;

            List<Documento> documentos = new List<Documento>();

            // Valida si hay un ID Rendicion diferente
            for (int row = 0; row < rowCount; row++)
            {
                if (row > 1)
                {
                    if (facturaSheet.Cells[row, 13].Text != "" )
                        if (facturaSheet.Cells[row, 13].Text != id.ToString())
                            throw new Exception("Se está intentando agregar documentos a una rendición diferente");
                }
            }

            for (int row = 1; row <= rowCount; row++)
            {
                if (row > 1)
                {
                    ObtieneCuerpo(ref documentos, facturaSheet, row);
                }
            }

            return documentos;
        }
        public void ObtieneCuerpo(ref List<Documento> documentos, ExcelWorksheet cab, int row)
        {
            Documento documento = new Documento();

            try
            {
                ObtieneCabecera(ref documentos, ref documento, ref cab, ref row);
            }
            catch
            {

            }
        }

        public void ObtieneCabecera(ref List<Documento> documentos, ref Documento documento, ref ExcelWorksheet cab, ref int row)
        {

            string idRendicion = cab.Cells[row, 13].Text;
            if (string.IsNullOrEmpty(idRendicion))
                return;

            string id = cab.Cells[row, 1].Text;     // Id de la factura que estás buscando
            string almacen = string.Empty;     // Almacen al que pertenece
            string tpoRendicion = string.Empty;      // Tipo de Rendicion
            int cantidad = 1;

            ValoresPorDefecto(ref almacen, ref tpoRendicion, idRendicion);

            DateTime fechaContabiliza = DateTime.Parse(cab.Cells[row, 2].Text);
            DateTime fechaDocumento = DateTime.Parse(cab.Cells[row, 3].Text);
            DateTime fechaVencimiento = DateTime.Parse(cab.Cells[row, 4].Text);

            documento.STR_FECHA_CONTABILIZA = fechaContabiliza.ToString("yyyy-MM-dd");
            documento.STR_FECHA_DOC = fechaDocumento.ToString("yyyy-MM-dd");
            documento.STR_FECHA_VENCIMIENTO = fechaVencimiento.ToString("yyyy-MM-dd");
            documento.STR_PROVEEDOR = cab.Cells[row, 5].Text == "" ? null : new Proveedor { CardCode = cab.Cells[row, 5].Text };
            documento.STR_TIPO_AGENTE = cab.Cells[row, 6].Text == "" ? null : new Complemento { id = cab.Cells[row, 6].Text.Split('-')[0].Trim() };
            documento.STR_MONEDA = cab.Cells[row, 7].Text == "" ? null : new Complemento { name = cab.Cells[row, 7].Text };
            documento.STR_COMENTARIOS = cab.Cells[row, 8].Text;
            documento.STR_TIPO_DOC = cab.Cells[row, 9].Text == "" ? null : new Complemento { id = cab.Cells[row, 9].Text.Split('-')[0].Trim() };
            documento.STR_SERIE_DOC = cab.Cells[row, 10].Text;
            documento.STR_CORR_DOC = cab.Cells[row, 11].Text;
            documento.STR_TOTALDOC = Convert.ToDouble(cab.Cells[row, 12].Text);
            documento.STR_RD_ID = Convert.ToInt32(cab.Cells[row, 13].Text);
            documento.STR_OPERACION = "1";

            /* Valores por Defecto */

            documento.detalles = ObtieneDetalle(id, almacen, tpoRendicion, cantidad);

            documentos.Add(documento);
        }

        public List<DocumentoDet> ObtieneDetalle(string cabId, string almacen, string tpoOpe, int cantidad)
        {
            List<DocumentoDet> detalles = new List<DocumentoDet>();

            ExcelWorksheet det = package.Workbook.Worksheets["Detalle"];

            int rowCount = det.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                if (det.Cells[row, 9].Text == cabId)
                {
                    DocumentoDet documentoDet = new DocumentoDet()
                    {
                        STR_CODARTICULO = new EL.Responses.Item { id = det.Cells[row, 2].Text, posFinanciera = det.Cells[row, 7].Text },
                        STR_SUBTOTAL = Convert.ToDouble(det.Cells[row, 3].Text),
                        STR_INDIC_IMPUESTO = new Complemento { id = det.Cells[row, 4].Text },
                        STR_PROYECTO = new Complemento { name = det.Cells[row, 5].Text },
                        STR_CENTCOSTO = new CentroCosto { CostCenter = det.Cells[row, 6].Text },
                        //STR_POS_FINANCIERA = new Complemento { id = det.Cells[row, 7].Text },
                        STR_CUP = new Cup { U_CUP = det.Cells[row, 8].Text },
                        STR_ALMACEN = almacen,
                        STR_TPO_OPERACION = tpoOpe,
                        STR_CANTIDAD = cantidad
                    };
                    detalles.Add(documentoDet);
                }
            }
            return detalles;
        }

        public void ValoresPorDefecto(ref string almacen, ref string tpoRendicion, string id) // Id de Rendicion
        {
            Sq_Rendicion sq = new Sq_Rendicion();

            var data = sq.ObtenerRendicion(id.ToString()).Result[0];
            almacen = data.STR_EMPLEADO_ASIGNADO.fax;
            tpoRendicion = "1";//data.SOLICITUDRD.STR_TIPORENDICION;

        }
    }
}
