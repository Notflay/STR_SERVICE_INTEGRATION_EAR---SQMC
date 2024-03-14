using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.EL.Responses
{
    public class Aprobador
    {
        public int idSolicitud { get; set; }
        public int? aprobadorId { get; set; }
        public string aprobadorNombre { get; set; }
        public string emailAprobador { get; set; }
        public int finalizado { get; set; }
        public int empleadoId { get; set; }
        public string nombreEmpleado { get; set; }
        public string fechaRegistro { get; set; }
        public int estado { get; set; }
        public int TipoPerfil { get; set; }
        public string Rendicion { get; set; }
        public int area { get; set; }
        public List<Aprobador> aprobadores { get; set; }
    }
}
