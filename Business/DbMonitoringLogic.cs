using System.Data;
using MySql.Data.MySqlClient;
using ProjektMonitoringNuget.DBAccess;

namespace ProjektMonitoringNuget.Business
{
    public static class DbMonitoringLogic
    {
        public static DataTable Select()
        {
            DbConnect.Open();
            var dataAdapter = new MySqlDataAdapter("Select id AS Id,pod,location,hostname,severity,timestamp,message  FROM v_logentries", DbConnect.Connection);
            var dt = new DataTable();
            dataAdapter.Fill(dt);
            DbConnect.Close();
            return dt;
        }

        public static DataTable LogClear(int index, DataTable dt)
        {
            var bOk = int.TryParse(dt.Rows[index]["Id"].ToString(), out var logId);
            if (bOk)
            {
                IDbCommand cmd = DbConnect.Connection.CreateCommand();
                cmd.CommandText = $"call LogClear({logId});";
                DbConnect.Open();
                cmd.ExecuteNonQuery();
                DbConnect.Close();
            }

            return Select();
        }
    }
}