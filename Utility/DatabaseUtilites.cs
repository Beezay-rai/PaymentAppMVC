using Microsoft.Data.SqlClient;
using System.Data;

namespace PaymentApp.Areas.Admin
{
    public class DatabaseUtilities
    {
        private readonly SqlConnection _connection;
        private readonly IConfiguration _config;


        public DatabaseUtilities(IConfiguration configuration)
        {
            _config = configuration;
            _connection = InitializeConnection();
        }



        public SqlConnection InitializeConnection()
        {
            var conncection = new SqlConnection(GetConnectionString());
            return conncection;
        }

        public SqlConnection GetConnection()
        {
            return _connection;
        }


        public void OpenConnection()
        {
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();

        }

        public void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
                _connection.Close();

        }

        public string GetConnectionString()
        {
            return _config.GetConnectionString("DefaultConnection");
        }
    }
}
