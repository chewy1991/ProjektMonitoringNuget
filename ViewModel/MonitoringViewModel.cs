using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ProjektMonitoringNuget.ViewModel
{
    public class MonitoringViewModel: INotifyPropertyChanged
    {
        private MySqlConnection _connection = null;
        private DataTable _result;
        public event PropertyChangedEventHandler PropertyChanged;

        public DataTable Result { get { return this._result; } set { this._result = value; NotifyPropertyChanged(); }}
        private MySqlBaseConnectionStringBuilder builder = new MySqlConnectionStringBuilder()
        {
            Server = "localhost",
            Database = "testat",
            UserID = "Monitoring",
            Password = "secret"
        };

        public  MonitoringViewModel()
        {
            _connection = new MySqlConnection(builder.ConnectionString);
        }

        private void Open()
        {
            try
            {
                _connection.Open();                
            }
            catch(Exception e)
            {
                _connection.Close();
            }            
        }
        private void CLose()
        {
            if(_connection.State == ConnectionState.Open)
                _connection.Close();
        }

        public void Select()
        {
            Open();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter("Select * FROM v_logentries",_connection);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            Result = dt;
            CLose();

        }



        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
