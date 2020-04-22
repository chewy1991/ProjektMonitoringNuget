using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjektMonitoringNuget.DBAccess;

namespace ProjektMonitoringNuget.Business
{
    public static class DbMonitoringLogic
    {
        public static DataTable Select()
        {
            DBConnect.Open();
            var dataAdapter = new MySqlDataAdapter("Select id AS Id,pod,location,hostname,severity,timestamp,message  FROM v_logentries", DBConnect.Connection);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);            
            DBConnect.Close();
            return dt;
        }

        public static DataTable LogClear(int index, DataTable dt)
        {
            var bOK = int.TryParse(dt.Rows[index]["Id"].ToString(), out int logId);
            if (bOK)
            {
                IDbCommand cmd = DBConnect.Connection.CreateCommand();
                cmd.CommandText = $"call LogClear({logId});";
                DBConnect.Open();
                cmd.ExecuteNonQuery();
                DBConnect.Close();                
            }
            return Select();
        }
    }
}
