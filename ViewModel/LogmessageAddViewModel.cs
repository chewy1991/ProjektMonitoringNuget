using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Input;
using ProjektMonitoringNuget.Business;
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
                                                                                               , typeof(string)
                                                                                               , typeof(LogmessageAddViewModel)
                                                                                               , new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty SeverityListProperty = DependencyProperty.Register("SeverityList"
                                                                                                   , typeof(List<string>)
                                                                                                   , typeof(LogmessageAddViewModel)
                                                                                                   , new UIPropertyMetadata(new List<string> {"Error", "Warn", "Info", "Debug", "Trace"}));

        public static readonly DependencyProperty DevicesProperty = DependencyProperty.Register("Devices"
                                                                                              , typeof(DataTable)
                                                                                              , typeof(LogmessageAddViewModel)
                                                                                              , new UIPropertyMetadata(DbLogMessageAddLogic.Select()));

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

        public string Severity
        {
            get => (string) GetValue(SeverityProperty);
            set => SetValue(SeverityProperty, value);
        }

        public List<string> SeverityList
        {
            get => (List<string>) GetValue(SeverityProperty);
            set => SetValue(SeverityProperty, value);
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
                                                             DbLogMessageAddLogic.AddMessage(this);
                                                             Message = string.Empty;
                                                             Selectedindex = -1;
                                                             Severity = string.Empty;
                                                         }
                                                       , () => AddCanExecute));
            }
        }

        public bool AddCanExecute => !string.IsNullOrEmpty(Message) && !string.IsNullOrEmpty(Severity) && Selectedindex >= 0;

        #endregion
    }
}