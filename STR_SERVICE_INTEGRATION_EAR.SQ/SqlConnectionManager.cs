using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using Sap.Data.Hana;


namespace STR_SERVICE_INTEGRATION_EAR.SQ
{
    public class HanaConnectionManager
    {
        private HanaConnection hanaConnection = null;
        //private HanaConnection hanaConnection = null;

        public HanaConnection GetConnection()
        {
            hanaConnection = new HanaConnection(ConfigurationManager.ConnectionStrings["hanaIntermedia"].ConnectionString);
            return hanaConnection; 
        }

        public HanaConnection GetConnectionDirecta()
        {
            hanaConnection = new HanaConnection(ConfigurationManager.ConnectionStrings["hanaDirecta"].ConnectionString);
            return hanaConnection;
        }

        public void OpenConnection()
        {
            hanaConnection.Open();
        }

        public void CloseConnection()
        {
            hanaConnection.Close();
            hanaConnection = null;
        }
    }
}
