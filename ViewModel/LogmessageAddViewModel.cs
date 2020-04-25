using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using ProjektMonitoringNuget.DBAccess;
using ProjektMonitoringNuget.View.Commands;

namespace ProjektMonitoringNuget.ViewModel
{
    public class LogmessageAddViewModel : DependencyObject
    {
        #region Dependency Properties

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message"
                                                                                              , typeof(string)
                                                                                              , typeof(LogmessageAddViewModel)
                                                                                              , new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty SeverityProperty = DependencyProperty.Register("Severity"
                                                                                               , typeof(DataTable)
                                                                                               , typeof(LogmessageAddViewModel)
                                                                                               , new UIPropertyMetadata(SelectSeverity()));

        public static readonly DependencyProperty SelectedIndexSeverityProperty = DependencyProperty.Register("SelectedIndexSeverity"
                                                                                                            , typeof(int)
                                                                                                            , typeof(LogmessageAddViewModel)
                                                                                                            , new UIPropertyMetadata(-1));

        public static readonly DependencyProperty DevicesProperty = DependencyProperty.Register("Devices"
                                                                                              , typeof(DataTable)
                                                                                              , typeof(LogmessageAddViewModel)
                                                                                              , new UIPropertyMetadata(SelectDevices()));

        public static readonly DependencyProperty SelectedindexProperty = DependencyProperty.Register("Selectedindex"
                                                                                                    , typeof(int)
                                                                                                    , typeof(LogmessageAddViewModel)
                                                                                                    , new UIPropertyMetadata(-1));

        #endregion

        #region Binding Properties

        public string Message
        {
            get => (string) GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public DataTable Severity
        {
            get => (DataTable) GetValue(SeverityProperty);
            set => SetValue(SeverityProperty, value);
        }

        public int SelectedIndexSeverity
        {
            get => (int) GetValue(SelectedIndexSeverityProperty);
            set => SetValue(SelectedIndexSeverityProperty, value);
        }

        public DataTable Devices
        {
            get => (DataTable) GetValue(DevicesProperty);
            set => SetValue(DevicesProperty, value);
        }

        public int Selectedindex
        {
            get => (int) GetValue(SelectedindexProperty);
            set => SetValue(SelectedindexProperty, value);
        }

        #endregion

        #region Commandbindings

        private ICommand _addcommand;

        public ICommand Addcommand
        {
            get
            {
                return _addcommand
                    ?? (_addcommand = new CommandHandler(() =>
                                                         {
                                                             AddMessage();
                                                             Message = string.Empty;
                                                             Selectedindex = -1;
                                                             SelectedIndexSeverity = -1;
                                                         }
                                                       , () => AddCanExecute));
            }
        }

        public bool AddCanExecute => !string.IsNullOrEmpty(Message) && SelectedIndexSeverity >= 0 && Selectedindex >= 0;

        #endregion

        #region Methoden

        private static DataTable SelectDevices()
        {
            var dt = new DataTable();

            using (var conn = new DbConnect().Connection)
            {
                var dataAdapter = new SqlDataAdapter(new SqlCommand("Select pod.Bezeichnung AS PodName, Device.hostname, Device.ip_adresse, Device.anzahlports FROM Device INNER JOIN Adresse ON Device.AdressId = Adresse.id INNER JOIN point_of_delivery AS pod ON pod.podadresse = Adresse.id;", conn));
                dataAdapter.Fill(dt);
            }

            return dt;
        }

        private static DataTable SelectSeverity()
        {
            var dt = new DataTable();

            using (var conn = new DbConnect().Connection)
            {
                var dataAdapter = new SqlDataAdapter(new SqlCommand("SELECT * FROM Severity", conn));
                dataAdapter.Fill(dt);
            }

            return dt;
        }

        public void AddMessage()
        {
            var hostname = Devices.Rows[Selectedindex]["hostname"].ToString();
            var podName = Devices.Rows[Selectedindex]["PodName"].ToString();
            var severityId = Convert.ToInt32(Severity.Rows[SelectedIndexSeverity]["Id"].ToString());

            using (var conn = new DbConnect().Connection)
            {
                using (var cmd = new SqlCommand("LogMessageAdd", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@logmessage", SqlDbType.NVarChar).Value = Message;
                    cmd.Parameters.Add("@PodName", SqlDbType.NVarChar).Value = podName;
                    cmd.Parameters.Add("@Severity", SqlDbType.Int).Value = severityId;
                    cmd.Parameters.Add("@hostname", SqlDbType.NVarChar).Value = hostname;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion
    }
}