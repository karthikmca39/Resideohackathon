using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Energysavings.Models
{
    public class Devices
    {       
        public int LocationId { get; set; }
        public int DeviceTypeId { get; set; }
        public DateTime reportedAt { get; set; }
        public int Occupancy { get; set; }
    }
}