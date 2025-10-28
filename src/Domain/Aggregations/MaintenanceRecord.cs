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
        public int TechnicianID { get; private set; }
        public int DeviceID { get; private set; }
        public DateOnly Date { get; private set; }
        public double Cost { get; private set; }
        public MaintenanceType Type { get; private set; }
        public int MaintenanceRecordID { get; private set; }

        private MaintenanceRecord() { }
        public MaintenanceRecord(int technicianID, int deviceID, DateOnly date, double cost, MaintenanceType type)
        {
            ValidateDate(date);

            TechnicianID = technicianID;
            DeviceID = deviceID;
            Date = date;
            Cost = cost;
            Type = type;
        }

        private void ValidateDate(DateOnly date)
        {
            if (date > DateOnly.FromDateTime(DateTime.Now))
                throw new ArgumentException("Maintenance date cannot be in the future");

        }

        private void ValidateCost(double cost)
        {
            if (cost < 0)
                throw new ArgumentException("Maintenance cost cannot be negative");

        }
    }
}