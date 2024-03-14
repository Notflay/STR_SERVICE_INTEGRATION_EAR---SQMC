using STR_SERVICE_INTEGRATION_EAR.EL.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.EL.Requests
{
    public class Rendicion
    {
        public int ID { get; set; }
        public int STR_SOLICITUD { get; set; }
        public string STR_NRRENDICION { get; set; }
        public string STR_NRAPERTURA { get; set; }
        public int? STR_NRCARGA { get; set; }
        public int STR_ESTADO { get; set; }
        public int STR_EMPLDASIG { get; set; }
        public int STR_EMPLDREGI { get; set; }
        //public string STR_ESTADO_INFO { get; set; }
        public double STR_TOTALRENDIDO { get; set; }
        public string STR_FECHAREGIS { get; set; }
        public double STR_TOTALAPERTURA { get; set; }
       // public double U_CE_SLDI { get; set; }
        public int? STR_DOCENTRY { get; set; }
        public string STR_MOTIVOMIGR { get; set; }
        public Usuario STR_EMPLEADO_ASIGNADO { get; set; }
        public Complemento STR_ESTADO_INFO { get; set; }
        public SolicitudRD SOLICITUDRD { get; set; }
        public List<Documento> documentos { get; set; }

    }
}
