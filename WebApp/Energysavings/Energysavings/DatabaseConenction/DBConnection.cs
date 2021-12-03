using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Energysavings.Models;

namespace Energysavings.DatabaseConenction
{
    public static class DBConnection
    {
        public static void ExecuteNnQuery(string queryString)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(
                       connectionString))
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

            string sqlquery = "SELECT * from cts_rawdata where receivedtime BETWEEN GETDATE()-5 AND GETDATE() order by receivedtime DESC";

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

        public static List<EnergyReading> readEnergyData()
        {

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = ConfigurationManager.AppSettings["Datasource"];
            builder.UserID = ConfigurationManager.AppSettings["SqlServerUserId"];
            builder.Password = ConfigurationManager.AppSettings["Password"];
            builder.InitialCatalog = ConfigurationManager.AppSettings["InitialCatalog"];

            List<EnergyReading> energyReadingList = new List<EnergyReading>();

            string sqlquery = "SELECT * from tblenergysaving where reportedAt BETWEEN GETDATE()-5 AND GETDATE()";

            try
            {
                EnergyReading data;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sqlquery, connection))
                    {
                        using (SqlDataReader rdr = command.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                data = new EnergyReading();
                                data.locationId = rdr["locationId"].ToString();
                                data.deviceId = rdr["deviceId"].ToString();
                                data.reportedAt =Convert.ToDateTime(rdr["reportedAt"].ToString());
                                data.Power= Convert.ToDouble(rdr["Power"].ToString());
                                data.Temperature= Convert.ToDouble(rdr["Temperature"].ToString());
                                data.Humidity = Convert.ToDouble(rdr["Humidity"].ToString());
                                data.Occupancy = Convert.ToInt32(rdr["Occupancy"].ToString());
                                data.ArmState= rdr["ArmState"].ToString(); 
                                energyReadingList.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return energyReadingList;

        }

        public static List<Devices> getDevices()
        {

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = ConfigurationManager.AppSettings["Datasource"];
            builder.UserID = ConfigurationManager.AppSettings["SqlServerUserId"];
            builder.Password = ConfigurationManager.AppSettings["Password"];
            builder.InitialCatalog = ConfigurationManager.AppSettings["InitialCatalog"];

            List<Devices> deviceList = new List<Devices>();

            string sqlquery = "SELECT * from tbldevices where reportedAt BETWEEN GETDATE()-5 AND GETDATE()";

            try
            {
                Devices data;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sqlquery, connection))
                    {
                        using (SqlDataReader rdr = command.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                data = new Devices();
                                data.LocationId = Convert.ToInt32(rdr["LocationId"].ToString());                                
                                data.reportedAt = Convert.ToDateTime(rdr["reportedAt"].ToString());                             
                                data.DeviceTypeId = Convert.ToInt32(rdr["DeviceTypeId"].ToString());
                                data.Occupancy = Convert.ToInt32(rdr["Occupancy"].ToString());
                                deviceList.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return deviceList;

        }
    }
}