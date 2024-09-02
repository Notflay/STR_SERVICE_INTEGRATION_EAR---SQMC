using STR_SERVICE_INTEGRATION_EAR.EL.Responses;
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
        public string Nombres { get; set; }
        public int activo { get; set; }
        public Complemento TipoUsuario { get; set; }
        public string sex { get; set; }
        public int SubGerencia { get; set; }
        public string email { get; set; }
        public string fax { get; set; }
        public string numeroEAR { get; set; }
        public string provAsociado { get; set; }
        public string CostCenter { get; set; }
        public int dept { get; set; }
        public int ID { get; set; }

        /*
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
        public string CostCenter { get; set; }*/

    }
    public class UsuarioInfo
    {
        public UsuarioSAP usuarioSAP { get; set; }
        public UsuarioPortal usuarioPortal { get; set; }
    }
    public class UsuarioSAP
    { 
        public int EmpleadoId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cargo { get; set; }
        public string Email { get; set; }
        public string CodEar { get; set; }
        public Proveedor ProveedorAsoc { get; set; }
        public string RendicionesMaxima { get; set; }
    }
    public class UsuarioPortal
    {
        public int ID { get; set; }
        public string Nombres { get; set; }
        public string Username { get; set; }
        public string FechaRegistro { get; set; }
        public Complemento Rol { get; set; }
        public string Estado { get; set; }
    }
}
