using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using MySql.Data.MySqlClient;
using ProjektMonitoringNuget.Business;
using ProjektMonitoringNuget.DBAccess;
using ProjektMonitoringNuget.DBAccess.DBContext;
using ProjektMonitoringNuget.View.Commands;

namespace ProjektMonitoringNuget.ViewModel
{
    public class LogmessageAddViewModel : DependencyObject
    {
        #region Dependency Properties
        
        private static DataSet _database = DbContext.GetDbContext();

        public static DependencyProperty AbrechnungProperty = DependencyProperty.Register("Abrechnung"
                                                                                              , typeof(DataTable)
                                                                                              , typeof(LogmessageAddViewModel)
                                                                                              , new UIPropertyMetadata(_database.Tables["Abrechnung"]));
        public static DependencyProperty AbrechnungspositionProperty = DependencyProperty.Register("Abrechnungsposition"
                                                                                              , typeof(DataTable)
                                                                                              , typeof(LogmessageAddViewModel)
                                                                                              , new UIPropertyMetadata(_database.Tables["Abrechnungsposition"]));
        public static DependencyProperty AdresseProperty = DependencyProperty.Register("Adresse"
                                                                                              , typeof(DataTable)
                                                                                              , typeof(LogmessageAddViewModel)
                                                                                              , new UIPropertyMetadata(_database.Tables["Adresse"]));
        public static DependencyProperty CredentialsProperty = DependencyProperty.Register("Credentials"
                                                                                              , typeof(DataTable)
                                                                                              , typeof(LogmessageAddViewModel)
                                                                                              , new UIPropertyMetadata(_database.Tables["Credentials"]));
        public static DependencyProperty DevicesProperty = DependencyProperty.Register("Devices"
                                                                                              , typeof(DataTable)
                                                                                              , typeof(LogmessageAddViewModel)
                                                                                              , new UIPropertyMetadata(_database.Tables["Device"]));
        public static DependencyProperty DevicecategoryProperty = DependencyProperty.Register("Devicecategory"
                                                                                              , typeof(DataTable)
                                                                                              , typeof(LogmessageAddViewModel)
                                                                                              , new UIPropertyMetadata(_database.Tables["Devicecategory"]));
        public static DependencyProperty KontaktpersonProperty = DependencyProperty.Register("Kontaktperson"
                                                                                              , typeof(DataTable)
                                                                                              , typeof(LogmessageAddViewModel)
                                                                                              , new UIPropertyMetadata(_database.Tables["Kontaktperson"]));
        public static DependencyProperty KundeProperty = DependencyProperty.Register("Kunde"
                                                                                              , typeof(DataTable)
                                                                                              , typeof(LogmessageAddViewModel)
                                                                                              , new UIPropertyMetadata(_database.Tables["Kunde"]));
        public static DependencyProperty LocationProperty = DependencyProperty.Register("Location"
                                                                                              , typeof(DataTable)
                                                                                              , typeof(LogmessageAddViewModel)
                                                                                              , new UIPropertyMetadata(_database.Tables["Location"]));
        public static DependencyProperty LogmessageProperty = DependencyProperty.Register("Logmessage"
                                                                                              , typeof(DataTable)
                                                                                              , typeof(LogmessageAddViewModel)
                                                                                              , new UIPropertyMetadata(_database.Tables["Logmessage"]));
        public static DependencyProperty NetworkinterfaceProperty = DependencyProperty.Register("Networkinterface"
                                                                                                 , typeof(DataTable)
                                                                                                 , typeof(LogmessageAddViewModel)
                                                                                                 , new UIPropertyMetadata(_database.Tables["Networkinterface"]));
        public static DependencyProperty Point_of_deliveryProperty = DependencyProperty.Register("Point_of_delivery"
                                                                                                 , typeof(DataTable)
                                                                                                 , typeof(LogmessageAddViewModel)
                                                                                                 , new UIPropertyMetadata(_database.Tables["Point_of_delivery"]));
        public static DependencyProperty PortsProperty = DependencyProperty.Register("Ports"
                                                                                                 , typeof(DataTable)
                                                                                                 , typeof(LogmessageAddViewModel)
                                                                                                 , new UIPropertyMetadata(_database.Tables["Ports"]));

        #endregion

        #region Binding Properties

        public DataTable Abrechnung
        {
            get => (DataTable) GetValue(AbrechnungProperty);
            set => SetValue(AbrechnungProperty, value);
        }

        public DataTable Abrechnungsposition
        {
            get => (DataTable) GetValue(AbrechnungspositionProperty);
            set => SetValue(AbrechnungspositionProperty, value);
        }

        public DataTable Adresse
        {
            get => (DataTable) GetValue(AdresseProperty);
            set => SetValue(AdresseProperty, value);
        }
        
        public DataTable Credentials
        {
            get => (DataTable) GetValue(CredentialsProperty);
            set => SetValue(CredentialsProperty, value);
        }

        public DataTable Devices
        {
            get => (DataTable)GetValue(DevicesProperty);
            set => SetValue(DevicesProperty, value);
        }

        public DataTable Devicecategory
        {
            get => (DataTable) GetValue(DevicecategoryProperty);
            set => SetValue(DevicecategoryProperty, value);
        }

        public DataTable Kontaktperson
        {
            get => (DataTable) GetValue(KontaktpersonProperty);
            set => SetValue(KontaktpersonProperty, value);
        }

        public DataTable Kunde
        {
            get => (DataTable) GetValue(KundeProperty);
            set => SetValue(KundeProperty, value);
        }

        public DataTable Location
        {
            get => (DataTable) GetValue(LocationProperty);
            set => SetValue(LocationProperty, value);
        }

        public DataTable Logmessage
        {
            get => (DataTable) GetValue(LogmessageProperty);
            set => SetValue(LogmessageProperty, value);
        }

        public DataTable Networkinterface
        {
            get => (DataTable) GetValue(NetworkinterfaceProperty);
            set => SetValue(NetworkinterfaceProperty, value);
        }

        public DataTable Point_of_delivery
        {
            get => (DataTable) GetValue(Point_of_deliveryProperty);
            set => SetValue(Point_of_deliveryProperty, value);
        }

        public DataTable Ports
        {
            get => (DataTable) GetValue(PortsProperty);
            set => SetValue(PortsProperty, value);
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
                                                                UpdateDb();
                                                             }
                                                           , () => AddCanExecute));
            }
        }
        public bool AddCanExecute => _database.HasChanges();

        #endregion

        private void UpdateDb()
        {
            if (_database.HasChanges())
            {
                _database.AcceptChanges();

                try
                {
                    var dataAdapter = new MySqlDataAdapter("Select * FROM device;", DbConnect.Connection);
                    //dataAdapter.TableMappings.Add("abrechnung", "abrechnung");
                    //dataAdapter.TableMappings.Add("abrechnungsposition", "abrechnungsposition");
                    //dataAdapter.TableMappings.Add("adresse", "adresse");
                    //dataAdapter.TableMappings.Add("credentials", "credentials");
                    dataAdapter.TableMappings.Add("device", "device");
                    //dataAdapter.TableMappings.Add("devicecategory", "devicecategory");
                    //dataAdapter.TableMappings.Add("kontaktperson", "kontaktperson");
                    //dataAdapter.TableMappings.Add("kunde", "kunde");
                    //dataAdapter.TableMappings.Add("location", "location");
                    //dataAdapter.TableMappings.Add("logmessage", "logmessage");
                    //dataAdapter.TableMappings.Add("networkinterface", "networkinterface");
                    //dataAdapter.TableMappings.Add("point_of_delivery", "point_of_delivery");
                    //dataAdapter.TableMappings.Add("ports", "ports");
                    dataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    dataAdapter.MissingMappingAction = MissingMappingAction.Passthrough;
                    MySqlCommandBuilder cmdbuilder = new MySqlCommandBuilder(dataAdapter);
                    DbConnect.Open();
                    dataAdapter.UpdateCommand = cmdbuilder.GetUpdateCommand();
                    dataAdapter.UpdateCommand.ExecuteNonQuery();
                    dataAdapter.InsertCommand = cmdbuilder.GetInsertCommand();
                    dataAdapter.InsertCommand.ExecuteNonQuery();
                    dataAdapter.DeleteCommand = cmdbuilder.GetDeleteCommand();
                    dataAdapter.DeleteCommand.ExecuteNonQuery();
                    dataAdapter.FillSchema(_database, SchemaType.Source);
                    dataAdapter.Update(_database);
                    DbConnect.Close();
                }
                catch (Exception e)
                {
                    DbConnect.Close();
                }
            }
        }
    }
}