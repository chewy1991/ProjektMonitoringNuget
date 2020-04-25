using System;
using System.Data;
using System.Data.SqlClient;
using ProjektMonitoringNuget.Properties;

namespace ProjektMonitoringNuget.DBAccess
{
    public class DbConnect
    {
        private static SqlConnectionStringBuilder Builder = new SqlConnectionStringBuilder()
                                                                     {
                                                                         DataSource = Settings.Default.Datasource
                                                                       , InitialCatalog = Settings.Default.InitialCatalog
                                                                       , UserID = Settings.Default.UserId
                                                                       , Password = Settings.Default.Password
                                                                     };
        public SqlConnection Connection { get; } = new SqlConnection(Builder.ConnectionString);

        public void Open(){
            
            try { Connection.Open(); }
            catch (Exception) { Connection.Close(); }
        }

        public void Close()
        {
            if (Connection.State == ConnectionState.Open) Connection.Close();
        }
    }
}