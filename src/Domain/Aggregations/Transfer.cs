using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregations
{
    public class Transfer
    {
        public Transfer(Guid equipmentID, DateOnly dateTime, Guid sourceSectionID, Guid destinySectionID)
        {
            EquipmentID = equipmentID;
            DateTime = dateTime;
            SourceSectionID = sourceSectionID;
            DestinySectionID = destinySectionID;
        }

        public Guid EquipmentID {  get; set; } 
        public DateOnly DateTime { get; set; }
        public Guid SourceSectionID { get; set; }    
        public Guid DestinySectionID { get; set; }
        public Guid TransferID { get; set; } 
        public Guid EquipmentReceiverID { get; set; }   
    }
}
