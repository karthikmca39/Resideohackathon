using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.ComponentModel;

namespace Dataparser
{
    public static class DBConnection
    {

        public static void ExecuteNnQuery(string queryString)
        {

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = ConfigurationManager.AppSettings["Datasource"];
            builder.UserID = ConfigurationManager.AppSettings["SqlServerUserId"];
            builder.Password = ConfigurationManager.AppSettings["Password"];
            builder.InitialCatalog = ConfigurationManager.AppSettings["InitialCatalog"];

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static List<Rawdata> readRawdata()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = ConfigurationManager.AppSettings["Datasource"];
            builder.UserID = ConfigurationManager.AppSettings["SqlServerUserId"];
            builder.Password = ConfigurationManager.AppSettings["Password"];
            builder.InitialCatalog = ConfigurationManager.AppSettings["InitialCatalog"];

            List<Rawdata> dataList = new List<Rawdata>();

            string sqlquery = "SELECT * from cts_rawdata where receivedtime BETWEEN '2021-11-30 00:00:00' AND '2021-11-30 23:59:00'";

            try
            {
                Rawdata data;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sqlquery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                data = new Rawdata();
                                data.ReceivedAt = reader.GetDateTime(0);
                                data.rawdata = reader.GetString(1);
                                dataList.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dataList;

        }

        public static void bulkInsert(List<EnergyReading> energysavaings)
        {
            DataTable dt = new DataTable();
            dt = ConvertToDataTable(energysavaings);

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = ConfigurationManager.AppSettings["Datasource"];
            builder.UserID = ConfigurationManager.AppSettings["SqlServerUserId"];
            builder.Password = ConfigurationManager.AppSettings["Password"];
            builder.InitialCatalog = ConfigurationManager.AppSettings["InitialCatalog"];

            using (var copy = new SqlBulkCopy(builder.ConnectionString))
            {
                copy.DestinationTableName = "dbo.[tblenergysaving]";
                // Add mappings so that the column order doesn't matter
                copy.ColumnMappings.Add(nameof(EnergyReading.locationId), "locationId");
                copy.ColumnMappings.Add(nameof(EnergyReading.deviceId), "deviceId");
                copy.ColumnMappings.Add(nameof(EnergyReading.reportedAt), "reportedAt");
                copy.ColumnMappings.Add(nameof(EnergyReading.Power), "Power");
                copy.ColumnMappings.Add(nameof(EnergyReading.Temperature), "Temperature");
                copy.ColumnMappings.Add(nameof(EnergyReading.Humidity), "Humidity");
                copy.ColumnMappings.Add(nameof(EnergyReading.Occupancy), "Occupancy");
                copy.ColumnMappings.Add(nameof(EnergyReading.ArmState), "ArmState");

                copy.WriteToServer(dt);
            }

        }
        public static void bulkInsert1(List<DeviceInfo> devcieList)
        {
            DataTable dt = new DataTable();
            dt = ConvertToDataTable(devcieList);

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = ConfigurationManager.AppSettings["Datasource"];
            builder.UserID = ConfigurationManager.AppSettings["SqlServerUserId"];
            builder.Password = ConfigurationManager.AppSettings["Password"];
            builder.InitialCatalog = ConfigurationManager.AppSettings["InitialCatalog"];

            using (var copy = new SqlBulkCopy(builder.ConnectionString))
            {
                copy.DestinationTableName = "dbo.[tbldevices]";
                // Add mappings so that the column order doesn't matter
                copy.ColumnMappings.Add(nameof(DeviceInfo.LocationId), "LocationId");
                copy.ColumnMappings.Add(nameof(DeviceInfo.DeviceTypeId), "DeviceTypeId");
                copy.ColumnMappings.Add(nameof(DeviceInfo.reportedAt), "reportedAt");
                copy.ColumnMappings.Add(nameof(DeviceInfo.Occupancy), "Occupancy");

                copy.WriteToServer(dt);
            }

        }
        //converting the objects to DataTable
        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }


    }

}
