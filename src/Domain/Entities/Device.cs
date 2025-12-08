using Domain.Aggregations;
using Domain.Enums;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Domain.Entities
{
    public class Device:SoftDeleteBase
    {
        public int DeviceId { get; private set; }
        public string Name { get; private set; }
        public DeviceType Type { get; private set; }
        public OperationalState OperationalState { get; private set; }
        public int DepartmentId { get; private set; }
        public DateTime AcquisitionDate { get; private set; }
        public override bool IsDisabled { get; set; }

        public Device(string name, DeviceType type, OperationalState operationalState, int? departmentId, DateTime acquisitionDate)
        {
            ValidateName(name);
            ValidateDate(acquisitionDate);
            Name = name;
            Type = type;
            OperationalState = operationalState;
            if (departmentId == null)
            {
                DepartmentId = 1;
            }
            else
                DepartmentId = (int)departmentId;
            AcquisitionDate = acquisitionDate;
        }
        public Device(string name, DeviceType type, OperationalState operationalState, int departmentId, DateTime acquisitionDate, int deviceId)
        {
            ValidateName(name);
            //ValidateDate(acquisitionDate);
            Name = name;
            Type = type;
            OperationalState = operationalState;
            DepartmentId = departmentId;
            AcquisitionDate = acquisitionDate;
            DeviceId = deviceId;
        }

        private void ValidateDate(DateTime acquisitionDate)
        {
            if (acquisitionDate > DateTime.Now)
            {
                throw new ArgumentException();
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
        public void ChangeDepartment(int newDepartmentId)
        {
            DepartmentId = newDepartmentId;
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
