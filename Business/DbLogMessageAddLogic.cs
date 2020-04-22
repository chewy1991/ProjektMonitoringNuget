using MySql.Data.MySqlClient;
using ProjektMonitoringNuget.DBAccess;
using ProjektMonitoringNuget.ViewModel;
using System.Data;

namespace ProjektMonitoringNuget.Business
{
    public static class DbLogMessageAddLogic
    {
        public static DataTable Select()
        {
            DbConnect.Open();
            var dataAdapter = new MySqlDataAdapter("Select pod.point_of_delivery_id AS podid, Device.hostname, Device.ip_adresse, Device.anzahlports FROM Device INNER JOIN Adresse ON Device.device_adresse = Adresse.adresse_id INNER JOIN point_of_delivery AS pod ON pod.pod_adresse = Adresse.adresse_id;", DbConnect.Connection);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            DbConnect.Close();
            return dt;
        }

        public static void AddMessage(LogmessageAddViewModel logmodel)
        {
            LogmessageAddViewModel vm = logmodel;
            var hostname = vm.Devices.Rows[(int)vm.Selectedindex]["hostname"].ToString();
            var bOK = int.TryParse(vm.Devices.Rows[(int)vm.Selectedindex]["podid"].ToString(), out int podId) ;
            if (bOK)
            {
                MySqlCommand cmd = DbConnect.Connection.CreateCommand();                
                cmd.CommandText = "call LogMessageAdd(@message,@severity,@podid,@hostname);";
                cmd.Parameters.AddWithValue("@message",vm.Message);
                cmd.Parameters.AddWithValue("@severity", vm.Severity);
                cmd.Parameters.AddWithValue("@podid", podId);
                cmd.Parameters.AddWithValue("@hostname", hostname);
                DbConnect.Open();
                cmd.ExecuteNonQuery();
                DbConnect.Close();

            }
        }
    }
}
