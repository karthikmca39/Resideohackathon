using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataparser
{
    public class DeviceInfo
    {
        public DeviceInfo()
        {
            LocationId = 12345;           
        }
        public int LocationId { get; set; }       
        public int DeviceTypeId { get; set; }       
        
        public DateTime reportedAt { get; set; } 
        
        public int Occupancy { get; set; }    
        
    }
}
