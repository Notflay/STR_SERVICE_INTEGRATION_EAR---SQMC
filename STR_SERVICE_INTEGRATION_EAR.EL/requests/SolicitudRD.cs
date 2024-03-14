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
        public int? STR_EMPLDREGI { get; set; }
        public int? STR_NRSOLICITUD { get; set; }
        public string STR_NRRENDICION { get; set; }
        public string STR_ESTADO_INFO { get; set; }
        public int? STR_ESTADO { get; set; }
        public int? STR_EMPLDASIG { get; set; }
        public string STR_FECHAREGIS { get; set; }
        public int? STR_UBIGEO { get; set; }
        public string STR_RUTA { get; set; }
        public string STR_RUTAANEXO { get; set; }
        public string STR_MOTIVO { get; set; }
        public string STR_FECHAINI { get; set; }
        public string STR_FECHAFIN { get; set; }
        public string STR_FECHAVENC { get; set; }
        public string STR_MONEDA { get; set; }
        public string STR_TIPORENDICION { get; set; }
        public int? STR_IDAPROBACION { get; set; }
        public double STR_TOTALSOLICITADO { get; set; }
        public string STR_MOTIVOMIGR { get; set; }
        public string STR_ORDENVIAJE { get; set; }
        public string STR_AREA { get; set; }
        public int? STR_DOCENTRY { get; set; }
        public string STR_NOMBRES { get; set; }
        public string CREATE { get; set; }
        public List<SolicitudRDdet> SOLICITUD_DET { get; set; }
        public Direccion STR_DIRECCION { get; set; }
        public Complemento STR_RUTA_INFO { get; set; }
        public Complemento STR_TIPORENDICION_INFO { get; set; }
        public Usuario STR_EMPLEADO_ASIGNADO { get; set; }

    }
}
