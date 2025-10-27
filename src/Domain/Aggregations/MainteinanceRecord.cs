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
        public string? Type { get; private set; }
        public int MaintenanceRecordID { get; private set; }

        private MaintenanceRecord() { }
        public MaintenanceRecord(DateOnly date, double cost, string type, int technicianID, int deviceID)
        {
            TechnicianID = technicianID;
            DeviceID = deviceID;
            Date = date;
            Cost = cost;
            Type = type;
        }
    }
}
