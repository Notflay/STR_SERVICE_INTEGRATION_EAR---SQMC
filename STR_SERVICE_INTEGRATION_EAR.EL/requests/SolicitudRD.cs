using STR_SERVICE_INTEGRATION_EAR.EL.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.EL.Requests
{
    public class SolicitudRD
    {
        public int? ID { get; set; }
        //public int? RML_EMPLDREGI { get; set; }
        //public int? RML_NRSOLICITUD { get; set; }
        public EmpleadoSAP RML_EMPLDREGI { get; set; }
        public EmpleadoSAP RML_EMPLDASIG { get; set; }
        public int? RML_NRSOLICITUD { get; set; }
        public string RML_NRRENDICION { get; set; }
        public string RML_ESTADO_INFO { get; set; }
        public Complemento RML_ESTADO { get; set; }
        //public int? RML_EMPLDASIG { get; set; }
        public string RML_FECHAREGIS { get; set; }
        public int? RML_UBIGEO { get; set; }
        public string RML_RUTA { get; set; }
        public string RML_RUTAANEXO { get; set; }
        public string RML_MOTIVO { get; set; }
        public string RML_FECHAINI { get; set; }
        public string RML_FECHAFIN { get; set; }
        public string RML_FECHAVENC { get; set; }
        public string RML_DESCRIPCION { get; set; }
        public string RML_COMENTARIOS { get; set; }
        public Complemento RML_MONEDA { get; set; }
        public Complemento RML_TIPORENDICION { get; set; }
        public int? RML_IDAPROBACION { get; set; }
        public decimal RML_TOTALSOLICITADO { get; set; }
        public string RML_MOTIVOMIGR { get; set; }
        public string RML_ORDENVIAJE { get; set; }
        public string RML_AREA { get; set; }
        public int? RML_DOCENTRY { get; set; }
        public string RML_NOMBRES { get; set; }
        public string RML_APROBACIONFINALIZADA { get; set; }
        public string CREATE { get; set; }
        public List<SolicitudRDdet> SOLICITUD_DET { get; set; }
        public Direccion RML_DIRECCION { get; set; }
        public Complemento RML_RUTA_INFO { get; set; }
        public Complemento RML_TIPORENDICION_INFO { get; set; }
        public Usuario RML_EMPLEADO_ASIGNADO { get; set; }

    }
}
