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
        public int RML_SOLICITUD { get; set; }
        public string RML_NRRENDICION { get; set; }
        public string RML_NRAPERTURA { get; set; }
        public int? RML_NRCARGA { get; set; }
        public int RML_ESTADO { get; set; }
        public int RML_EMPLDASIG { get; set; }
        public int RML_EMPLDREGI { get; set; }
        //public string RML_ESTADO_INFO { get; set; }
        public double RML_TOTALRENDIDO { get; set; }
        public string RML_FECHAREGIS { get; set; }
        public double RML_TOTALAPERTURA { get; set; }
       // public double U_CE_SLDI { get; set; }
        public int? RML_DOCENTRY { get; set; }
        public string RML_MOTIVOMIGR { get; set; }
        public Usuario RML_EMPLEADO_ASIGNADO { get; set; }
        public Complemento RML_ESTADO_INFO { get; set; }
        public SolicitudRD SOLICITUDRD { get; set; }
        public List<Documento> documentos { get; set; }

    }
}
