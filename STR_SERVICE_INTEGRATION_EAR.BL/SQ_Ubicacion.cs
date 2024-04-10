using STR_SERVICE_INTEGRATION_EAR.EL.Commons;
using STR_SERVICE_INTEGRATION_EAR.EL.Responses;
using STR_SERVICE_INTEGRATION_EAR.SQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace STR_SERVICE_INTEGRATION_EAR.BL
{
    public class SQ_Ubicacion
    {

        SqlADOHelper hash = new SqlADOHelper();
        public ConsultationResponse<Departamento> ObtenerDepartamentos()
        {
            string pais = "PE";
            var respIncorrect = "No Hay Departamentos";

            try
            {
                List<Departamento> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_departamentos), dc =>
                {
                    return new Departamento()
                    {
                        IdDepartamento = dc["IdDepartamento"],
                        Nombre = dc["Nombre"]
                    };
                }, pais).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Departamento>(ex);
            }
        }
        public ConsultationResponse<Departamento> ObtenerDepartamento(string id)
        {
            string pais = "PE";
            var respIncorrect = "No Hay Departamento";

            try
            {
                List<Departamento> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_departamento), dc =>
                {
                    return new Departamento()
                    {
                        IdDepartamento = dc["IdDepartamento"],
                        Nombre = dc["Nombre"]
                    };
                }, pais, id).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Departamento>(ex);
            }
        }
        public ConsultationResponse<Provincia> ObtenerProvincias(string idDepartamentio)
        {
            var respIncorrect = "No Hay Provincias";

            try
            {
                List<Provincia> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_provincias), dc =>
                {
                    return new Provincia()
                    {
                        IdProvincia = dc["IdProvincia"],
                        Nombre = dc["Nombre"],
                        IdDepartamento = dc["IdDepartamento"]
                    };
                }, idDepartamentio).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Provincia>(ex);
            }
        }
        public ConsultationResponse<Distrito> ObtenerDistritos(string idProvincia)
        {
            var respIncorrect = "No Hay Distrito";

            try
            {
                List<Distrito> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_distritos), dc =>
                {
                    return new Distrito()
                    {
                        Ubigeo = dc["Ubigeo"],
                        Nombre = dc["Nombre"],
                        IdProvincia = dc["IdProvincia"]
                    };
                }, idProvincia).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Distrito>(ex);
            }
        }
        public ConsultationResponse<Direccion> ObtenerDireccion(string idDireccion)
        {
            var respIncorrect = "No se encuentra dirección con el Ubigeo ingresado";

            try
            {
                List<Direccion> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_direccion), dc =>
                {
                    return new Direccion()
                    {
                        IdDepartamento = dc["IdDepartamento"],
                        Departamento = dc["Departamento"],
                        IdProvincia = dc["IdProvincia"],
                        Provincia = dc["Provincia"],
                        Ubigeo = dc["Ubigeo"],
                        Distrito = dc["Distrito"]
                    };
                }, idDireccion).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Direccion>(ex);
            }
        }

        public ConsultationResponse<Direccion> ObtenerUbigeoPorLetra(string filtro, string letra)
        {
            var respIncorrect = "No se encuentra distrito según codigo";

            try
            {
                List<Direccion> list = hash.GetResultAsType(SQ_QueryManager.Generar(SQ_Query.get_direccionPorLetra), dc =>
                {
                    return new Direccion()
                    {
                        IdDepartamento = dc["IdDepartamento"],
                        Departamento = dc["Departamento"],
                        IdProvincia = dc["IdProvincia"],
                        Provincia = dc["Provincia"],
                        Ubigeo = dc["Ubigeo"],
                        Distrito = dc["Distrito"]
                    };
                }, filtro, letra).ToList();

                return Global.ReturnOk(list, respIncorrect);
            }
            catch (Exception ex)
            {
                return Global.ReturnError<Direccion>(ex);
            }
        }
    }
}
