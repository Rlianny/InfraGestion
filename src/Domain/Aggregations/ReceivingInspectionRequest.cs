using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregations
{
    public class ReceivingInspectionRequest
    {
        public ReceivingInspectionRequest(Guid equipmentID, Guid administratorID, Guid technicianID, DateTime emissionDate)
        {
            EquipmentID = equipmentID;
            AdministratorID = administratorID;
            TechnicianID = technicianID;
            EmissionDate = emissionDate;
        }

        public Guid EquipmentID { get; set; }   
        public Guid AdministratorID { get; set; }
        public Guid TechnicianID { get; set; }  
        public DateTime EmissionDate { get; set; }
        public DateTime? AcceptedDate { get; set; }
        public DateTime? RejectionDate { get; set; }
    }
}
