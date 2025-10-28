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

        public Device(string name, DeviceType type, OperationalState operationalState, int departmentID, DateTime acquisitionDate)
        {
            ValidateName(name);
            ValidateDepartment(departmentID);
            Name = name;
            Type = type;
            OperationalState = operationalState;
            DepartmentID = departmentID;
            AcquisitionDate = acquisitionDate;
        }

        private void ValidateDepartment(int departmentID)
        {
            if (departmentID < 0)
            {
                throw new Exception("Department ID cannot be negative");
            }
        }

        private void ValidateName(string name)
        {
            if (name == string.Empty || name.Length < 3)
            {
                throw new Exception("Name to short");
            }
        }

        private Device()
        {
            Name = String.Empty;
        }
    }
}
