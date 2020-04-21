using MySql.Data.MySqlClient;
using ProjektMonitoringNuget.DBAccess;
using ProjektMonitoringNuget.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjektMonitoringNuget.Business
{
    public static class DbLogMessageAddLogic
    {
        public static DataTable Select()
        {
            DBConnect.Open();
            var dataAdapter = new MySqlDataAdapter("Select pod.point_of_delivery_id AS podid, Device.hostname, Device.ip_adresse, Device.anzahlports FROM Device INNER JOIN Adresse ON Device.device_adresse = Adresse.adresse_id INNER JOIN point_of_delivery AS pod ON pod.pod_adresse = Adresse.adresse_id;", DBConnect.Connection);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            DBConnect.Close();
            return dt;
        }

        public static void AddMessage(LogmessageAddViewModel logmodel)
        {
            LogmessageAddViewModel vm = logmodel;
            var hostname = vm.Devices.Rows[(int)vm.Selectedindex]["hostname"].ToString();
            var bOK = int.TryParse(vm.Devices.Rows[(int)vm.Selectedindex]["podid"].ToString(), out int podId) ;
            if (bOK)
            {
                IDbCommand cmd = DBConnect.Connection.CreateCommand();
                cmd.CommandText = $"call LogMessageAdd('{vm.Message}','{vm.Severity}',{podId},'{hostname}');";
                DBConnect.Open();
                cmd.ExecuteNonQuery();
                DBConnect.Close();

            }
        }
    }
}
