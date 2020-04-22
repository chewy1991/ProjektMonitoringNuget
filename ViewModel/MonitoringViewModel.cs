using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MySql.Data.MySqlClient;
using ProjektMonitoringNuget.Business;
using ProjektMonitoringNuget.Commands;
using ProjektMonitoringNuget.View;

namespace ProjektMonitoringNuget.ViewModel
{
    public class MonitoringViewModel: DependencyObject 
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

        public static readonly DependencyProperty LogentriesProperty = DependencyProperty.Register("Logentries"
                                                                                                 , typeof(DataTable)
                                                                                                 , typeof(MonitoringViewModel));
        
        #endregion
        #region Binding Properties
        public LogmessageAdd AddLogmessage
        {
            get { return (LogmessageAdd)GetValue(AddLogmessageProperty); }
            set { SetValue(AddLogmessageProperty, value); }
        }

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public DataTable Logentries
        {
            get { return (DataTable)GetValue(LogentriesProperty); }
            set { SetValue(LogentriesProperty, value); }
        }
        #endregion

        #region Commandbindings
        private ICommand _loadCommand;
        public ICommand LoadCommand
        {
            get
            {
                return _loadCommand ?? (_loadCommand = new CommandHandler(() => Logentries = DbMonitoringLogic.Select(), () => LoadCanExecute));               
            }
        }
        public bool LoadCanExecute
        {
            get { return true; }
        }

        private ICommand _logClearCommand;
        public ICommand LogClearCommand
        {
            get
            {
                return _logClearCommand ?? (_logClearCommand = new CommandHandler(() => Logentries = DbMonitoringLogic.LogClear(SelectedIndex, Logentries), () => LogCanExecute));
            }
        }
        public bool LogCanExecute
        {
            get { return SelectedIndex >= 0; }
        }
        private ICommand _addDataCommand;
        public ICommand AddDataCommand
        {
            get
            {
                return _addDataCommand ?? (_addDataCommand = new CommandHandler(() => { AddLogmessage = new LogmessageAdd(); AddLogmessage.Show(); }, () => AddCanExecute));
            }
        }
        public bool AddCanExecute
        {
            get { return AddLogmessage == null || !AddLogmessage.IsLoaded; }
        }
        #endregion       

        public  MonitoringViewModel()
        {
        }         

        

    }
}
