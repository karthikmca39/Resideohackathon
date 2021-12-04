using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Dataparser
{
    public class Program
    {
        static void Main(string[] args)
        {
            var list = DBConnection.readRawdata();

            List<EnergyReading> energyReadingList = new List<EnergyReading>();
            EnergyReading energyReading;
            DataTable dt = new DataTable();

            foreach (var data in list)
            {
                var rawdata = Helpers.DecodeBase64(data.rawdata);
                string cleaned = rawdata.Replace("\n", "");
                string[] subs = cleaned.Split(',');

                energyReading = new EnergyReading();


                var locationid = subs[0].Split(':');
                var substringloc = locationid[1];
                energyReading.locationId = substringloc.Substring(1, substringloc.Length - 2);

                var deviceId = subs[1].Split(':');
                var substringdev = deviceId[1];
                energyReading.deviceId = substringdev.Substring(1, substringdev.Length - 2);

                var powerValue = subs[3].Split(':');
                energyReading.Power = Convert.ToDouble(powerValue[1]);

                var temperature = subs[4].Split(':');
                energyReading.Temperature = Convert.ToDouble(temperature[1]);

                var humidity = subs[5].Split(':');
                energyReading.Humidity = Convert.ToDouble(humidity[1]);

                var occupancy = subs[6].Split(':');
                energyReading.Occupancy = Convert.ToInt32(occupancy[1]);

                var armState = subs[7].Split(':');
                var substringarm = armState[1];
                energyReading.ArmState = substringarm.Substring(1, substringarm.Length - 3);

                energyReading.reportedAt = data.ReceivedAt;
                energyReadingList.Add(energyReading);

            }

            DBConnection.bulkInsert(energyReadingList);

            Console.WriteLine("Power info Insert Completed");

            var deviceInfo = powerCalculation(energyReadingList);

            DBConnection.bulkInsert1(deviceInfo);

            Console.WriteLine("Device info Insert Completed");
            Console.ReadLine();

        }
        public static List<DeviceInfo> powerCalculation(List<EnergyReading> energyReadingList)
        {
            /*
            F = 0.020;
            L = 0.008;
            T = 0.041;
            W = 0.625;
            AC = 0.472;

            F L = 0.028
            F T = 0.061

            F T W = 0.686           
            F T L = 0.069            
            F T AC = 0.553
            F T 3L = 0.085

            F T W AC = 1.158
            F T AC 3L = 0.557
            F T AC 2L = 0.549
            F=1,L=2,T=3,W=4,AC=5
            */

            List<DeviceInfo> devicelist = new List<DeviceInfo>();
            DeviceInfo devices, devices1, devices2, devices3, devices4, devices5;

            foreach (var obj in energyReadingList)
            {

                if (obj.Power >= 0.020 && obj.Power <= 0.030) //F = 0.020;
                {
                    devices = new DeviceInfo();
                    devices.DeviceTypeId = 1;                   
                    devices.reportedAt = obj.reportedAt;
                    devices.Occupancy = obj.Occupancy;
                    devicelist.Add(devices);
                }
                else if (obj.Power >= 0.008 && obj.Power <= 0.018)//L = 0.008;
                {
                    devices = new DeviceInfo();
                    devices.DeviceTypeId = 2;                    
                    devices.reportedAt = obj.reportedAt;
                    devices.Occupancy = obj.Occupancy;
                    devicelist.Add(devices);
                }
                else if(obj.Power >= 0.041 && obj.Power <= 0.051)//T = 0.041;
                {
                    devices = new DeviceInfo();
                    devices.DeviceTypeId = 3;             
                    devices.reportedAt = obj.reportedAt;
                    devices.Occupancy = obj.Occupancy;
                    devicelist.Add(devices);
                }
                else if(obj.Power >= 0.625 && obj.Power <= 0.635)// W = 0.625;
                {
                    devices = new DeviceInfo();
                    devices.DeviceTypeId = 4;                   
                    devices.reportedAt = obj.reportedAt;
                    devices.Occupancy = obj.Occupancy;
                    devicelist.Add(devices);
                }
                else if(obj.Power >= 0.472 && obj.Power <= 0.482)//AC = 0.472;
                {
                    devices = new DeviceInfo();
                    devices.DeviceTypeId = 5;                                
                    devices.reportedAt = obj.reportedAt;
                    devices.Occupancy = obj.Occupancy;
                    devicelist.Add(devices);
                }
                else if (obj.Power >= 0.028 && obj.Power <= 0.038) //F L = 0.028
                {
                    devices = new DeviceInfo();
                    devices.DeviceTypeId = 1;                    
                    devices.reportedAt = obj.reportedAt;
                    devices.Occupancy = obj.Occupancy;
                    devicelist.Add(devices);

                    devices1 = new DeviceInfo();
                    devices1.DeviceTypeId = 2;                   
                    devices1.reportedAt = obj.reportedAt;
                    devices1.Occupancy = obj.Occupancy;
                    devicelist.Add(devices1);
                }
                else if (obj.Power >= 0.061 && obj.Power <= 0.071) //F T = 0.061
                {
                    devices = new DeviceInfo();
                    devices.DeviceTypeId = 1;                    
                    devices.reportedAt = obj.reportedAt;
                    devices.Occupancy = obj.Occupancy;
                    devicelist.Add(devices);

                    devices1 = new DeviceInfo();
                    devices1.DeviceTypeId = 3;                    
                    devices1.reportedAt = obj.reportedAt;
                    devices1.Occupancy = obj.Occupancy;
                    devicelist.Add(devices1);
                }
                else if (obj.Power >= 0.686 && obj.Power <= 0.696) // F T W = 0.686
                {
                    devices = new DeviceInfo();
                    devices.DeviceTypeId = 1;
                    devices.reportedAt = obj.reportedAt;
                    devices.Occupancy = obj.Occupancy;
                    devicelist.Add(devices);

                    devices1 = new DeviceInfo();
                    devices1.DeviceTypeId = 3;
                    devices1.reportedAt = obj.reportedAt;
                    devices1.Occupancy = obj.Occupancy;
                    devicelist.Add(devices1);

                    devices2 = new DeviceInfo();
                    devices2.DeviceTypeId = 4;
                    devices2.reportedAt = obj.reportedAt;
                    devices2.Occupancy = obj.Occupancy;
                    devicelist.Add(devices2);
                }
                else if (obj.Power >= 0.069 && obj.Power <= 0.079) // F T L = 0.069
                {
                    devices = new DeviceInfo();
                    devices.DeviceTypeId = 1;
                    devices.reportedAt = obj.reportedAt;
                    devices.Occupancy = obj.Occupancy;
                    devicelist.Add(devices);

                    devices1 = new DeviceInfo();
                    devices1.DeviceTypeId = 3;
                    devices1.reportedAt = obj.reportedAt;
                    devices1.Occupancy = obj.Occupancy;
                    devicelist.Add(devices1);

                    devices2 = new DeviceInfo();
                    devices2.DeviceTypeId = 2;
                    devices2.reportedAt = obj.reportedAt;
                    devices2.Occupancy = obj.Occupancy;
                    devicelist.Add(devices2);
                }
                else if (obj.Power >= 0.553 && obj.Power <= 0.563) //  F T AC = 0.553
                {
                    devices = new DeviceInfo();
                    devices.DeviceTypeId = 1;
                    devices.reportedAt = obj.reportedAt;
                    devices.Occupancy = obj.Occupancy;
                    devicelist.Add(devices);

                    devices1 = new DeviceInfo();
                    devices1.DeviceTypeId = 3;
                    devices1.reportedAt = obj.reportedAt;
                    devices1.Occupancy = obj.Occupancy;
                    devicelist.Add(devices1);

                    devices2 = new DeviceInfo();
                    devices2.DeviceTypeId = 5;
                    devices2.reportedAt = obj.reportedAt;
                    devices2.Occupancy = obj.Occupancy;
                    devicelist.Add(devices2);
                }
                else if (obj.Power >= 0.085 && obj.Power <= 0.095) //   F T 3L = 0.085
                {
                    devices = new DeviceInfo();
                    devices.DeviceTypeId = 1;
                    devices.reportedAt = obj.reportedAt;
                    devices.Occupancy = obj.Occupancy;
                    devicelist.Add(devices);

                    devices1 = new DeviceInfo();
                    devices1.DeviceTypeId = 3;
                    devices1.reportedAt = obj.reportedAt;
                    devices1.Occupancy = obj.Occupancy;
                    devicelist.Add(devices1);

                    devices2 = new DeviceInfo();
                    devices2.DeviceTypeId = 2;
                    devices2.reportedAt = obj.reportedAt;
                    devices2.Occupancy = obj.Occupancy;
                    devicelist.Add(devices2);

                    devices3 = new DeviceInfo();
                    devices3.DeviceTypeId = 2;
                    devices3.reportedAt = obj.reportedAt;
                    devices3.Occupancy = obj.Occupancy;
                    devicelist.Add(devices3);

                    devices4 = new DeviceInfo();
                    devices4.DeviceTypeId = 2;
                    devices4.reportedAt = obj.reportedAt;
                    devices4.Occupancy = obj.Occupancy;
                    devicelist.Add(devices4);
                }
                else if (obj.Power >= 1.158 && obj.Power <= 1.168) //     F T W AC = 1.158
                {
                    devices = new DeviceInfo();
                    devices.DeviceTypeId = 1;
                    devices.reportedAt = obj.reportedAt;
                    devices.Occupancy = obj.Occupancy;
                    devicelist.Add(devices);

                    devices1 = new DeviceInfo();
                    devices1.DeviceTypeId = 3;
                    devices1.reportedAt = obj.reportedAt;
                    devices1.Occupancy = obj.Occupancy;
                    devicelist.Add(devices1);

                    devices2 = new DeviceInfo();
                    devices2.DeviceTypeId = 4;
                    devices2.reportedAt = obj.reportedAt;
                    devices2.Occupancy = obj.Occupancy;
                    devicelist.Add(devices2);

                    devices3 = new DeviceInfo();
                    devices3.DeviceTypeId = 5;
                    devices3.reportedAt = obj.reportedAt;
                    devices3.Occupancy = obj.Occupancy;
                    devicelist.Add(devices3);                    
                }
                else if (obj.Power >= 0.557 && obj.Power <= 0.567) // F T AC 3L = 0.557
                {
                    devices = new DeviceInfo();
                    devices.DeviceTypeId = 1;
                    devices.reportedAt = obj.reportedAt;
                    devices.Occupancy = obj.Occupancy;
                    devicelist.Add(devices);

                    devices1 = new DeviceInfo();
                    devices1.DeviceTypeId = 3;
                    devices1.reportedAt = obj.reportedAt;
                    devices1.Occupancy = obj.Occupancy;
                    devicelist.Add(devices1);

                    devices2 = new DeviceInfo();
                    devices2.DeviceTypeId = 5;
                    devices2.reportedAt = obj.reportedAt;
                    devices2.Occupancy = obj.Occupancy;
                    devicelist.Add(devices2);

                    devices3 = new DeviceInfo();
                    devices3.DeviceTypeId = 1;
                    devices3.reportedAt = obj.reportedAt;
                    devices3.Occupancy = obj.Occupancy;
                    devicelist.Add(devices3);

                    devices4 = new DeviceInfo();
                    devices4.DeviceTypeId = 1;
                    devices4.reportedAt = obj.reportedAt;
                    devices4.Occupancy = obj.Occupancy;
                    devicelist.Add(devices4);

                    devices5 = new DeviceInfo();
                    devices5.DeviceTypeId = 1;
                    devices5.reportedAt = obj.reportedAt;
                    devices5.Occupancy = obj.Occupancy;
                    devicelist.Add(devices5);
                    
                }
                else if (obj.Power >= 0.549 && obj.Power <= 0.559) //  F T AC 2L = 0.549
                {
                    devices = new DeviceInfo();
                    devices.DeviceTypeId = 1;
                    devices.reportedAt = obj.reportedAt;
                    devices.Occupancy = obj.Occupancy;
                    devicelist.Add(devices);

                    devices1 = new DeviceInfo();
                    devices1.DeviceTypeId = 3;
                    devices1.reportedAt = obj.reportedAt;
                    devices1.Occupancy = obj.Occupancy;
                    devicelist.Add(devices1);

                    devices2 = new DeviceInfo();
                    devices2.DeviceTypeId = 5;
                    devices2.reportedAt = obj.reportedAt;
                    devices2.Occupancy = obj.Occupancy;
                    devicelist.Add(devices2);

                    devices3 = new DeviceInfo();
                    devices3.DeviceTypeId = 1;
                    devices3.reportedAt = obj.reportedAt;
                    devices3.Occupancy = obj.Occupancy;
                    devicelist.Add(devices3);

                    devices4 = new DeviceInfo();
                    devices4.DeviceTypeId = 1;
                    devices4.reportedAt = obj.reportedAt;
                    devices4.Occupancy = obj.Occupancy;
                    devicelist.Add(devices4);                   

                }             


            }

            return devicelist;
        }
    }

}