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
    public class Equipment
    {
        public Guid EquipmentID { get; set; }

        public string Name { get; set; } 

        public EquipmentType Type { get; set; }

        public OperationalState OperationalState { get; set; }

        public Guid DepartmentID { get; set; }
        
        public DateTime AcquisitionDate { get; set; }

    }
}
