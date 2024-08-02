using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;


namespace STR_SERVICE_INTEGRATION_EAR.SQ
{
    public class SqlConnectionManager
    {
        private SqlConnection sqlConnection = null;
        //private HanaConnection hanaConnection = null;

        public SqlConnection GetConnection()
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlIntermedia"].ConnectionString);
            return sqlConnection; 
        }

        public SqlConnection GetConnectionDirecta()
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlDirecta"].ConnectionString);
            return sqlConnection;
        }

        public void OpenConnection()
        {
            sqlConnection.Open();
        }

        public void CloseConnection()
        {
            sqlConnection.Close();
            sqlConnection = null;
        }
    }
}
