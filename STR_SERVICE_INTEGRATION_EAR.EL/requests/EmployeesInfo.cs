using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.EL.Requests
{
    public class EmployeesInfo
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string eMail {get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string U_CE_PVAS { get; set; } = string.Empty;
        public string U_CE_PVNM { get; set; } = string.Empty;
        public string U_CE_PVMN { get; set; }
        public string U_CE_CTAS { get; set; }
        public string U_CE_CEAR { get; set; }
        public int U_CE_RNDC { get; set; }
    }
}
