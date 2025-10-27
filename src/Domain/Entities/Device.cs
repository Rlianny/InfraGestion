using Domain.Aggregations;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Device
    {
        public int DeviceID { get; set; }

        public string Name { get; set; } 

        public DeviceType Type { get; set; }

        public OperationalState OperationalState { get; set; }

        public int DepartmentID { get; set; }
        
        public DateTime AcquisitionDate { get; set; }

        public virtual Department? Department { get; set; }
    }
}
