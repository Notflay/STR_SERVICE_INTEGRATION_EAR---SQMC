using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.EL.Requests
{
    public class Usuario
    {
        public int empID { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string sex { get; set; }
        public string jobTitle { get; set; }
        public string campo { get; set; }
        public string Nombres { get; set; }
        public int TipoUsuario { get; set; }
        public int SubGerencia { get; set; }
        public int dept { get; set; }
        public string email { get; set; }
        public string fax { get; set; }
        public string numeroEAR { get; set; }
        public string CostCenter { get; set; }
    }
}
