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
        public Transfer(DateOnly dateTime, int deviceID, int sourceSectionID, int destinationSectionID, int deviceReceiverID)
        {
            Date = dateTime;
            DeviceID = deviceID;
            SourceSectionID = sourceSectionID;
            DestinationSectionID = destinationSectionID;
            DeviceReceiverID = deviceReceiverID;
        }
        private Transfer()
        {
        }

        public int DeviceID { get; private set; }
        public DateOnly Date { get; private set; }
        public int SourceSectionID { get; private set; }
        public int DestinationSectionID { get; private set; }
        public int TransferID { get; private set; }
        public int DeviceReceiverID { get; private set; }

    }
}
