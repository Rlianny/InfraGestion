using Domain.Aggregations;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

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
            ValidateDate(acquisitionDate);
            Name = name;
            Type = type;
            OperationalState = operationalState;
            DepartmentID = departmentID;
            AcquisitionDate = acquisitionDate;
        }
        public Device(string name, DeviceType type, OperationalState operationalState, int departmentID, DateTime acquisitionDate, int deviceID)
        {
            ValidateName(name);
            ValidateDepartment(departmentID);
            ValidateDate(acquisitionDate);
            Name = name;
            Type = type;
            OperationalState = operationalState;
            DepartmentID = departmentID;
            AcquisitionDate = acquisitionDate;
            DeviceID = deviceID;
        }

        private void ValidateDate(DateTime acquisitionDate)
        {
            if (acquisitionDate > DateTime.Now)
            {
                throw new ArgumentException();
            }
        }

        private void ValidateDepartment(int departmentID)
        {
            if (departmentID < 0)
            {
                throw new ArgumentException("Department ID cannot be negative");
            }
        }

        private void ValidateName(string name)
        {
            if (name == string.Empty || name.Length < 3)
            {
                throw new ArgumentException("Name to short");
            }
        }
        public void UpdateOperationalState(OperationalState newState)
        {
            if (OperationalState == OperationalState.Decommissioned)
            {
                throw new InvalidOperationException("Cannot change state of decommissioned device");
            }
            OperationalState = newState;
        }
        public void ChangeDepartment(int newDepartmentID)
        {
            ValidateDepartment(newDepartmentID);
            DepartmentID = newDepartmentID;
        }
        public bool CanBeDecommissioned()
        {
            return OperationalState != OperationalState.Decommissioned &&
             OperationalState != OperationalState.BeingTransferred;
        }

        private Device()
        {
            Name = String.Empty;
        }
    }
}
