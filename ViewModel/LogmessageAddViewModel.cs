using ProjektMonitoringNuget.Business;
using ProjektMonitoringNuget.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjektMonitoringNuget.ViewModel
{
    public class LogmessageAddViewModel: INotifyPropertyChanged
    {
        private string _message;
        private List<string> _severitylist = new List<string>() { "Error", "Warn", "Info", "Debug", "Trace" };
        private string _severity;
        private DataTable _devices = DbLogMessageAddLogic.Select();
        private int? _selectedindex;

        public string Message { get { return this._message; } set { this._message = value; NotifyPropertyChanged(); } }
        public List<string> SeverityList { get { return this._severitylist; } }
        public string Severity { get { return this._severity; } set { this._severity = value; NotifyPropertyChanged(); } }
        public DataTable Devices { get { return this._devices; } }
        public int? Selectedindex { get { return this._selectedindex; } set { this._selectedindex = value; NotifyPropertyChanged(); } }

        #region Commandbindings
        private ICommand _addcommand;
        public ICommand Addcommand
        {
            get
            {
                return _addcommand ?? (_addcommand = new CommandHandler(() => Add(), () => AddCanExecute));
            }
        }
        public bool AddCanExecute
        {
            get { return !string.IsNullOrEmpty(_message) && !string.IsNullOrEmpty(_severity) && _selectedindex != null; }
        }
        #endregion

        private void Add()
        {
            DbLogMessageAddLogic.AddMessage(this);
            Selectedindex = null;
            this.Message = string.Empty;            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
