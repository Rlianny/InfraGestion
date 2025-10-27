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
        public Transfer(DateOnly dateTime, Device device, Section sourceSection, Section destinationSection, DeviceReceiver deviceReceiver)
        {
            DateTime = dateTime;
            Device = device;
            SourceSection = sourceSection;
            DestinationSection = destinationSection;
            DeviceReceiver = deviceReceiver;
        }
        private Transfer()
        {
        }

        public int DeviceID { get; private set; }
        public DateOnly DateTime { get; private set; }
        public int SourceSectionID { get; private set; }
        public int DestinySectionID { get; private set; }
        public int TransferID { get; private set; }
        public int DeviceReceiverID { get; private set; }

        // Navigation properties
        public virtual Device? Device { get; private set; }
        public virtual Section? SourceSection { get; private set; }
        public virtual Section? DestinationSection { get; private set; }
        public virtual DeviceReceiver? DeviceReceiver { get; private set; }

    }
}
