using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregations
{
    public class MaintenanceRecord
    {
        public int TechnicianId { get; private set; }
        public int DeviceId { get; private set; }
        public DateTime Date { get; private set; }
        public double Cost { get; private set; }
        public MaintenanceType Type { get; private set; }
        public int MaintenanceRecordId { get; private set; }
        public string Description { get; private set; }

        private MaintenanceRecord() { }
        public MaintenanceRecord(int technicianId, int deviceId, DateTime date, double cost, MaintenanceType type,string description)
        {
            ValidateDate(date);
            Description = description;
            TechnicianId = technicianId;
            DeviceId = deviceId;
            Date = date;
            Cost = cost;
            Type = type;
        }

        private void ValidateDate(DateTime date)
        {
            if (date > DateTime.Now)
                throw new ArgumentException("Maintenance date cannot be in the future");

        }

        private void ValidateCost(double cost)
        {
            if (cost < 0)
                throw new ArgumentException("Maintenance cost cannot be negative");

        }
    }
}