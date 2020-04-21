using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MySql.Data.MySqlClient;
using ProjektMonitoringNuget.Business;
using ProjektMonitoringNuget.Commands;
using ProjektMonitoringNuget.View;

namespace ProjektMonitoringNuget.ViewModel
{
    public class MonitoringViewModel: INotifyPropertyChanged
    {        
        private DataTable logentries = new DataTable();
        private int? _selectedindex;
        public event PropertyChangedEventHandler PropertyChanged;        
        public DataTable Logentries { get { return this.logentries; } set { this.logentries = value; NotifyPropertyChanged(); }}
        public int? SelectedIndex { get { return this._selectedindex; }set { this._selectedindex = value; } }
        private LogmessageAdd addlogmessage ;
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
                return _logClearCommand ?? (_logClearCommand = new CommandHandler(() => Logentries = DbMonitoringLogic.LogClear((int)_selectedindex,logentries,out this._selectedindex), () => LogCanExecute));
            }
        }
        public bool LogCanExecute
        {
            get { return this._selectedindex != null; }
        }
        private ICommand _addDataCommand;
        public ICommand AddDataCommand
        {
            get
            {
                return _addDataCommand ?? (_addDataCommand = new CommandHandler(() => AddData(), () => AddCanExecute));
            }
        }
        public bool AddCanExecute
        {
            get { return addlogmessage == null || !addlogmessage.IsLoaded; }
        }
        #endregion       

        public  MonitoringViewModel()
        {
        }       

        /// <summary>
        /// Öffnet das Fenster zum hinzufügen von Log-Messages
        /// </summary>
        private void AddData()
        {
            addlogmessage = new LogmessageAdd();
            addlogmessage.Show();
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
