using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektMonitoringNuget.DBAccess
{
    public static class DBConnect
    {
        private static MySqlBaseConnectionStringBuilder builder = new MySqlConnectionStringBuilder()
        {
            Server = "localhost",
            Database = "testat",
            UserID = "Monitoring",
            Password = "secret"
        };
        private static MySqlConnection _connection = new MySqlConnection(builder.ConnectionString);
        public static MySqlConnection Connection { get { return _connection; } }

        public static void Open()
        {
            try
            {
                _connection.Open();
            }
            catch (Exception e)
            {
                _connection.Close();
            }
        }
        public static void Close()
        {
            if (_connection.State == ConnectionState.Open)
                _connection.Close();
        }
    }
}
