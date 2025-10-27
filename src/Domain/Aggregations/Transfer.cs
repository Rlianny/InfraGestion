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
        public Transfer(Device device, Section sourceSection, Section destinationSection, DeviceReceiver deviceReceiver, DateOnly dateTime)
        {
            Date = dateTime;
            Device = device;
            SourceSection = sourceSection;
            DestinationSection = destinationSection;
            DeviceReceiver = deviceReceiver;
            DeviceID = Device.DeviceID;
            SourceSectionID = SourceSection.SectionID;
            DestinationSectionID = DestinationSection.SectionID;
            DeviceReceiverID = DeviceReceiver.UserID;
             
            
        }
        private Transfer() { }

        public int TransferID { get; private set; }
        public int DeviceID { get; private set; }
        public DateOnly Date { get; private set; }
        public int SourceSectionID { get; private set; }
        public int DestinationSectionID { get; private set; }
        public int DeviceReceiverID { get; private set; }

        // Navigation properties
        public virtual Device? Device { get; private set; }
        public virtual Section? SourceSection { get; private set; }
        public virtual Section? DestinationSection { get; private set; }
        public virtual DeviceReceiver? DeviceReceiver { get; private set; }

    }
}
