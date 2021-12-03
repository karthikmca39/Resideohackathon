using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Energysavings.DatabaseConenction;
using Energysavings.Models;
using Newtonsoft.Json;
using System.Text;
using Energysavings.Helpers;
using System.Globalization;

namespace Energysavings.Controllers
{
    public class EnergySavingController : Controller
    {
        // GET: EnergySaving
        public ActionResult Index()
        {
            CultureInfo provider = CultureInfo.InvariantCulture;

            List<EnergyReading> energyReadingList = DBConnection.readEnergyData();

            List<DataPoint> dataPoints = new List<DataPoint>();

            string charttime = string.Empty;
            double powerPerHoure = 0;

            foreach (var reading in energyReadingList.OrderBy(o => o.reportedAt).ToList())
            {

                var reportedTime = reading.reportedAt.ToString("HH:mm tt");

                if (reportedTime == charttime)
                {
                    powerPerHoure += reading.Power;
                }
                else
                {
                    string chartXaxis = reading.reportedAt.ToString("MMM dd") + " " + charttime;
                    dataPoints.Add(new DataPoint(chartXaxis, powerPerHoure));
                    charttime = reportedTime;
                    powerPerHoure = 0;
                }

            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            //Per device data

            List<Devices> deviceList = DBConnection.getDevices();


            List<PiDataPoint> pidataPoints = new List<PiDataPoint>();
            pidataPoints.Add(new PiDataPoint("Fan", (deviceList.Count(x => x.DeviceTypeId == 1)) * 0.02));
            pidataPoints.Add(new PiDataPoint("Light", (deviceList.Count(x => x.DeviceTypeId == 2)) * 0.008));
            pidataPoints.Add(new PiDataPoint("Washing Machine", (deviceList.Count(x => x.DeviceTypeId == 4)) * 0.625));
            pidataPoints.Add(new PiDataPoint("AC", (deviceList.Count(x => x.DeviceTypeId == 5)) * 0.472));
            pidataPoints.Add(new PiDataPoint("TV", (deviceList.Count(x => x.DeviceTypeId == 3)) * 0.041));

            ViewBag.PiDataPoints = JsonConvert.SerializeObject(pidataPoints);


            //Energy Savings

            List<EnergysavingDataPoint> dataPoints1 = new List<EnergysavingDataPoint>();
            List<EnergysavingDataPoint> dataPoints2 = new List<EnergysavingDataPoint>();
            string chartday = string.Empty;

            foreach (var reading in deviceList.OrderBy(o => o.reportedAt).ToList())
            {
                //First record
                chartday = reading.reportedAt.ToString("MMM dd");
                break;
            }

            double acctualPower = 0;
            double energysaving = 0;

            foreach (var reading in deviceList.OrderBy(o => o.reportedAt).ToList())
            {
                var currentday = reading.reportedAt.ToString("MMM dd");

                if (currentday.Equals(chartday))
                {
                    if (reading.DeviceTypeId == 1 && reading.Occupancy == 1)
                    {
                        if (reading.Occupancy == 1)
                        {
                            acctualPower += 0.02; //FAN With Occupancy
                        }
                        else { energysaving += 0.02; }
                    }
                    if (reading.DeviceTypeId == 2)
                    {
                        acctualPower += 0.008;
                    }
                    if (reading.DeviceTypeId == 3 && reading.Occupancy == 1)
                    {
                        if (reading.Occupancy == 1)
                        {
                            acctualPower += 0.041; //TV With Occupancy
                        }
                        else { energysaving += 0.041; }
                    }
                    if (reading.DeviceTypeId == 4 && reading.Occupancy == 1)
                    {
                        acctualPower += 0.02;
                    }
                    if (reading.DeviceTypeId == 5 && reading.Occupancy == 1)
                    {
                        if (reading.Occupancy == 1)
                        {
                            acctualPower += 0.472;
                        }
                        else { energysaving += 0.472; }
                    }
                }
                else
                {
                    dataPoints1.Add(new EnergysavingDataPoint(chartday, acctualPower));
                    dataPoints2.Add(new EnergysavingDataPoint(chartday, (acctualPower * 30/100)));
                    acctualPower = 0;
                    chartday = currentday;
                }
            }

            double TotalPowersavingmode = 0;

            foreach (var reading in deviceList.OrderBy(o => o.reportedAt).ToList())
            {
                if ((reading.Occupancy == 0))
                {
                    if (reading.DeviceTypeId == 1)
                    {
                        TotalPowersavingmode += 0.02;
                    }
                    if (reading.DeviceTypeId == 3)
                    {
                        TotalPowersavingmode += 0.041;
                    }
                    if (reading.DeviceTypeId == 5)
                    {
                        TotalPowersavingmode += 0.472;
                    }
                }
            }

            ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);
            ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints2);

            ViewBag.TotalPower = Math.Round(energyReadingList.Sum(x => x.Power), 2); 
            ViewBag.EnergySaving = Math.Round((energyReadingList.Sum(x => x.Power)*30/100), 2);

            return View();
        }

        public static float powerValue(int deviceType)
        {
            switch (deviceType)
            {
                case 1: return 0.02F;
                case 2: return 0.008F;
                case 3: return 0.041F;
                case 4: return 0.625F;
                case 5: return 0.472F;
                default: return 0.005F;
            }
        }

    }
}