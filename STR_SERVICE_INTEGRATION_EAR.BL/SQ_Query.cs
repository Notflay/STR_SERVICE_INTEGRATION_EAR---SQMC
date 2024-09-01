using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.BL
{
    public class SQ_Query
    {
        public static readonly string get_tokenPass = "ObtenerContraseniaUsuario";
        public static readonly string get_passForId = "ObtenerContraseniaUsuarioId";
        public static readonly string get_infoUser = "ObtenerInformacionUsuario";
        public static readonly string get_monedas = "ObtenerMonedas";
        public static readonly string get_moneda = "ObtenerMoneda";
        public static readonly string get_wtliable = "ObtenerWliable";
        public static readonly string get_wtliableId = "ObtenerWliableID";
        public static readonly string get_tablaIR = "ObtenerTablaIR";
        public static readonly string get_tablaIRId = "ObtenerTablaIRId";
        public static readonly string get_centroCosto = "ObtenerCeCo";
        public static readonly string get_centroCostoId = "ObtenerCeCoId";
        public static readonly string get_tpoperacion = "ObtenerTipoOperacion";
        public static readonly string get_tpoperacionId = "ObtenerTipoOperacionID";
        public static readonly string get_cuentacontable = "ObtieneCuentaContable";
        public static readonly string get_cuentacontableId = "ObtieneCuentaContableId";
        public static readonly string get_almacenes = "ObtieneAlmacenes";
        public static readonly string get_almacenesId = "ObtieneAlmacenesId";
        public static readonly string get_empleado = "obtenerEmpleado";

        public static readonly string get_infUser = "InformacionUsuario";
        public static readonly string get_infUser2 = "InformacionUsuario2";
        public static readonly string get_cfGeneral = "ObtenerConfGeneral";
        public static readonly string get_usuarios = "ObtenerUsuarios";
        public static readonly string get_departamentos = "ObtenerDepartamentos";
        public static readonly string get_departamento = "ObtenerDepartamento";
        public static readonly string get_provincias = "ObtenerProvincias";
        public static readonly string get_distritos = "ObtenerDistritos";
        public static readonly string get_direccion = "ObtenerDireccion";
        public static readonly string get_direccionPorLetra = "ObtenerDistritosPorLetra";
        public static readonly string get_estados = "ObtenerEstados";
        public static readonly string get_estado = "ObtenerEstado";
        public static readonly string get_comboTipos = "ObtenerComboTipos";
        public static readonly string get_comboTiposPorId = "ObtenerComboTiposPorId";
        public static readonly string get_proveedores = "ObtenerProveedores";
        public static readonly string get_proveedor = "ObtenerProveedor";
        public static readonly string get_items = "ListardItems";
        public static readonly string get_item = "ObtenerItem";
        
        public static readonly string get_obtenCup = "ListardCup";
        public static readonly string get_obtieneCup = "ObtenCUP";
        public static readonly string get_precioUnitario = "ListarPrecioUnitario";
        public static readonly string get_rutaAdjSap = "ObtenerRutaArchivosSAP";
        public static readonly string post_insertSR = "InsertaSoliRendicion";
        public static readonly string post_insertRD = "InsertaSoliRendicion";
        public static readonly string post_insertDOC = "InsertRegiRMLoDoc";
        public static readonly string post_insertDOCDt = "InsertRegiRMLoDocDt";
        public static readonly string post_insertRendicion = "InsertRendicion";
        public static readonly string post_pendientesBorrador = "InsertTotalPendientesBorr";
        public static readonly string get_pendienteBorradorId = "ObtieneTtPendienteBorr";
        public static readonly string upd_pendientesBorrador = "UpdateTotalPendientesBorr";
        public static readonly string dlt_pendientesBorrador = "DelteTotalPendientesBorr";
        public static readonly string get_listaSolicitudes = "ListarSolicitudRendicion";
        public static readonly string get_listaRendiciones = "ListardRendicion";
        public static readonly string get_obtenerDocumento = "ObtenerDocumento";
        public static readonly string get_obtenerDocumentos = "ObtenerDocumentos";
        public static readonly string get_listaDocumentoDet = "ObtenerDocumentoDetalles";
        public static readonly string get_idSR = "ObtieneIdSR";
        public static readonly string get_idDOC = "ObtieneIdDOC";
        public static readonly string upd_idSR = "ActualizarSoliRendicion";
        public static readonly string upd_idSRDet = "ActualizarSoliRendicionDet";
        public static readonly string upd_idDOC = "ActualizarDocumento";
        public static readonly string upd_idDOCDet = "ActualizarDocumentoDet";
        public static readonly string upd_RDTotal = "ActualizarRDTotal";
        public static readonly string upd_DOCTotal = "ActualizarDOCTotal";
        public static readonly string upd_DOCsunt = "ActualizarDOCValidaSunat";
        public static readonly string upd_Rendicon = "ActualizarRendicion";
        public static readonly string post_insertSrDet = "InsertaSoliRendicionDet";
        public static readonly string get_idSrDet = "ObtenerIdSoliRendiDet";
        public static readonly string get_aprobadores = "ObtieneAprobadores";
        public static readonly string get_proyectos = "ObtenerProyectos";
        public static readonly string get_proyecto = "ObtenerProyecto";
        public static readonly string get_indicadores = "ObtenerIndicadores";
        public static readonly string get_indicador = "ObtenerIndicador";
        public static readonly string get_tpoDocumentos = "ObtenerTpoDocumentos";
        public static readonly string get_tpoDocumento = "ObtenerTpoDocumento";
        public static readonly string get_SerieOPRQ = "ObtenerSerieOPRQ";
        public static readonly string post_insertSrDetCentC = "InsertaSoliRendicionDetCentC";
        public static readonly string get_solicitudRendicion = "ObtenerSolicitudRendicion";
        public static readonly string get_solicitudRendicionPorDocn = "ObtenerSRPorDocNum";
        public static readonly string get_solicitudRendicionDet = "ObtenerSolicitudRendicionDet";

        public static readonly string get_rendicion = "ObtenerRendicion";
        public static readonly string get_solicitudRendicionSAP = "ObtenerSolicitudRendicionSAP";
        public static readonly string get_solicitudRendicionDetSAP = "ObtenerSolicitudRendicionDetSAP";

        public static readonly string get_centrodeCostoPorItem = "ObtenerCentrosdeCostoPorItem";
        public static readonly string get_presupuesto = "ObtenerPresupuesto";
        public static readonly string get_presupuestoPrd = "ObtenerPresupuestoPrd";
        public static readonly string dlt_centrodeCostoPorItem = "BorrarSRDetCent";
        public static readonly string dlt_solicitudRdDetalle = "BorrarSRDetalle";
        public static readonly string post_insertAprobadores = "InsertaTablaAprobadoresSR";
        public static readonly string post_insertAprobadoresRD = "InsertaTablaAprobadoresRD";
        public static readonly string upd_cambiarEstadoSR = "CambiaEstadoSR";
        public static readonly string upd_cambiarEstadoRD = "CambiaEstadoRD";
        public static readonly string upd_cambiarMigradaSR = "CambioMigrada";
        public static readonly string upd_cambiarMigradaRD = "ActualizaRDMigrado";
        public static readonly string upd_migradaSRenSAP = "UpdateMigraSrSAP";
        public static readonly string upd_migradaRdenSAP = "UpdateMigraRdSAP";
        public static readonly string get_infoAprobadores = "ObtieneInfoAprobadores";
        public static readonly string get_infoAprobadoresRD = "ObtieneInfoAprobadoresRD";
        public static readonly string dlt_aprobadoresSr = "EliminaAprobadoresDeSolicitud";
        public static readonly string dlt_aprobadoresRd = "EliminarAprobadoresRendicion";
        public static readonly string upd_aprobadores = "ActualizaablaAprobadoresSR";
        public static readonly string upd_aprobadoresRD = "ActualizaablaAprobadoresRD";
        public static readonly string post_intermedia = "InsertaIntermadiaEAR";
        public static readonly string get_numeroRendicion = "ObtieneNumeroRendicion";
        public static readonly string dlt_documento = "BorrarDocumento";
        public static readonly string dlt_documentoDet = "BorrarDocumentoDet";
        public static readonly string dlt_documentoDetId = "BorrarDocumentoSubDet";
        public static readonly string get_adjuntos = "ObtieneAdjuntos";
        public static readonly string get_adjuntosDoc = "ObtieneAdjuntosDoc";

        public static readonly string get_listaAprobadoresCab = "ListarAprobadoresCabecera";
        public static readonly string get_listaAprobadoresDet = "ListarAprobadoresDetalle";

        public static readonly string get_listaAprobadoresCabRd = "ListarAprobadoresCabeceraRd";        // 1 Parametro
        public static readonly string get_listaAprobadoresDetRd = "ListarAprobadoresDetalleRd";

        public static readonly string get_validaCECO = "ValidarCentroCosto";

        // NUEVOS QUERYS
        public static readonly string get_usuariosPortal = "ObtieneUsuariosPortal";
        public static readonly string get_usuarioPortal = "ObtieneUsuarioPortal";
        public static readonly string get_valorUsrActivo = "ObtieneValorActivo";
        public static readonly string get_rolesPortal = "ObtenerRoles";
        public static readonly string get_usuarioExiste = "ValidarExistenciaUsername";
        public static readonly string get_usuarioExisteID = "ValidarExistenciaPorID";
        public static readonly string upd_resetUsuario = "ReseteaContrasenia";
        public static readonly string upd_actualizUsuario = "ActualizaEmpleado";
        public static readonly string upd_actualizarContrasenia = "ActualizarContrasenia";
        public static readonly string post_creaUsuario = "CrearEmpleado";
        public static readonly string get_empleadosPendientes = "ObtieneEmpleadosPendientes";
    }
}
