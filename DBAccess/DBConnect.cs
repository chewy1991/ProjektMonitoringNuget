using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace ProjektMonitoringNuget.DBAccess
{
    public static class DbConnect
    {
        private static readonly MySqlBaseConnectionStringBuilder Builder = new MySqlConnectionStringBuilder {Server = "localhost", Database = "testat", UserID = "Monitoring", Password = "secret"};
        public static MySqlConnection Connection { get; } = new MySqlConnection(Builder.ConnectionString);

        public static void Open()
        {
            try { Connection.Open(); }
            catch (Exception) { Connection.Close(); }
        }

        public static void Close()
        {
            if (Connection.State == ConnectionState.Open) Connection.Close();
        }
    }
}