using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using ProjektMonitoringNuget.DBAccess;
using ProjektMonitoringNuget.View;
using ProjektMonitoringNuget.View.Commands;

namespace ProjektMonitoringNuget.ViewModel
{
    public class MonitoringViewModel : DependencyObject
    {
        #region Dependency Properties

        public static readonly DependencyProperty AddLogmessageProperty = DependencyProperty.Register("AddLogmessage"
                                                                                                    , typeof(LogmessageAdd)
                                                                                                    , typeof(MonitoringViewModel)
                                                                                                    , new UIPropertyMetadata(null));

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex"
                                                                                                    , typeof(int)
                                                                                                    , typeof(MonitoringViewModel)
                                                                                                    , new UIPropertyMetadata(-1));

        public static readonly DependencyProperty LogentriesProperty = DependencyProperty.Register("Logentries", typeof(DataTable), typeof(MonitoringViewModel));

        #endregion

        #region Binding Properties

        public LogmessageAdd AddLogmessage
        {
            get => (LogmessageAdd) GetValue(AddLogmessageProperty);
            set => SetValue(AddLogmessageProperty, value);
        }

        public int SelectedIndex
        {
            get => (int) GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        public DataTable Logentries
        {
            get => (DataTable) GetValue(LogentriesProperty);
            set => SetValue(LogentriesProperty, value);
        }

        #endregion

        #region Commandbindings

        private ICommand _loadCommand;

        public ICommand LoadCommand
        {
            get { return _loadCommand ?? (_loadCommand = new CommandHandler(() => Logentries = Select(), () => LoadCanExecute)); }
        }

        public bool LoadCanExecute => true;

        private ICommand _logClearCommand;

        public ICommand LogClearCommand
        {
            get { return _logClearCommand ?? (_logClearCommand = new CommandHandler(() => LogClear(), () => LogCanExecute)); }
        }

        public bool LogCanExecute => SelectedIndex >= 0;
        private ICommand _addDataCommand;

        public ICommand AddDataCommand
        {
            get
            {
                return _addDataCommand
                    ?? (_addDataCommand = new CommandHandler(() =>
                                                             {
                                                                 AddLogmessage = new LogmessageAdd();
                                                                 AddLogmessage.Show();
                                                             }
                                                           , () => AddCanExecute));
            }
        }

        public bool AddCanExecute => AddLogmessage == null || !AddLogmessage.IsLoaded;

        #endregion

        #region Methoden

        public static DataTable Select()
        {
            var dt = new DataTable();
            using (SqlConnection conn = new DbConnect().Connection)
            {
                var dataAdapter = new SqlDataAdapter(new SqlCommand("Select id AS Id,pod,location,hostname,severity,timestamp,message  FROM v_logentries;", conn));
                dataAdapter.Fill(dt);
            }

            return dt;
        }

        public void LogClear()
        {
            var bOk = int.TryParse(Logentries.Rows[SelectedIndex]["Id"].ToString(), out int logId);
            if (bOk)
            {
                using (SqlConnection conn = new DbConnect().Connection)
                {
                    using (SqlCommand cmd = new SqlCommand("LogClear", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = logId;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            Logentries = Select();
        }

        #endregion
    }
}