using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Energysavings.Models
{
    public class EnergyReading
    {
        public string locationId { get; set; }
        public string deviceId { get; set; }
        public DateTime reportedAt { get; set; }
        public double Power { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public int Occupancy { get; set; }
        public string ArmState { get; set; }
    }
}