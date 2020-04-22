using System.Data;
using System.Windows;
using System.Windows.Input;
using ProjektMonitoringNuget.Business;
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
            get { return _loadCommand ?? (_loadCommand = new CommandHandler(() => Logentries = DbMonitoringLogic.Select(), () => LoadCanExecute)); }
        }

        public bool LoadCanExecute => true;

        private ICommand _logClearCommand;

        public ICommand LogClearCommand
        {
            get { return _logClearCommand ?? (_logClearCommand = new CommandHandler(() => Logentries = DbMonitoringLogic.LogClear(SelectedIndex, Logentries), () => LogCanExecute)); }
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
    }
}