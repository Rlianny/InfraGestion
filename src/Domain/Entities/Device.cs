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
        public int DeviceID { get; private set; }

        public string Name { get; private set; }

        public DeviceType Type { get; private set; }

        public OperationalState OperationalState { get; private set; }
        public int DepartmentID { get; private set; }

        public DateTime AcquisitionDate { get; private set; }

        public virtual Department? Department { get; private set; }
        private Device()
        {
        }
    }
}
