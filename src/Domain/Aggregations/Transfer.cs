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
        public Transfer(Guid equipmentID, DateOnly dateTime)
        {
            EquipmentID = equipmentID;
            DateTime = dateTime;
        }
        private Transfer()
        {
        }

        public Guid EquipmentID { get; private set; }
        public DateOnly DateTime { get; private set; }
        public int SourceSectionID { get; private set; }
        public int DestinySectionID { get; private set; }
        public int TransferID { get; private set; }
        public int DeviceReceiverID { get; private set; }

        // Navigation properties
        public virtual Device? Device { get; private set; }
        public virtual Section? SourceSection { get; private set; }
        public virtual Section? DestinySection { get; private set; }
        public virtual DeviceReceiver? DeviceReceiver { get; private set; }

    }
}
