using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ProjektMonitoringNuget.DBAccess.DBContext
{
    public static class DbContext
    {
        public static DataSet GetDbContext()
        {
            DataSet testatDb = new DataSet();
            var tablelist = new List<string>()
                            {
                                "abrechnung"
                              , "abrechnungsposition"
                              , "adresse"
                              , "credentials"
                              , "device"
                              , "devicecategory"
                              , "kontaktperson"
                              , "kunde"
                              , "location"
                              , "logmessage"
                              , "networkinterface"
                              , "point_of_delivery"
                              , "ports"
                            };

            foreach (var table in tablelist)
            {
                testatDb.Tables.Add(GetTables(table));
                if (testatDb.HasErrors)
                    testatDb.RejectChanges();
                else
                    testatDb.AcceptChanges();
            }
            
            

            return testatDb;
        }

        private static DataTable GetTables(string tablename)
        {
            DataTable table = new DataTable(tablename);

            var dataAdapter = new MySqlDataAdapter($"Select * FROM {tablename};", DbConnect.Connection);
            dataAdapter.TableMappings.Add(tablename, tablename);
            dataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            dataAdapter.FillSchema(table, SchemaType.Source);
            dataAdapter.Fill(table);

            return table;
        }

        public static string GetConnStrings()
        {
            string connstring = string.Empty;
            var tablelist = new List<string>()
                            {
                                "abrechnung"
                              , "abrechnungsposition"
                              , "adresse"
                              , "credentials"
                              , "device"
                              , "devicecategory"
                              , "kontaktperson"
                              , "kunde"
                              , "location"
                              , "logmessage"
                              , "networkinterface"
                              , "point_of_delivery"
                              , "ports"
                            };
            foreach (var table in tablelist) { connstring += $"SELECT * FROM {table}; "; }

            return connstring;
        }
    }
}
