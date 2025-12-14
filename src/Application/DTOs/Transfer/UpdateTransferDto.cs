using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Transfer
{
    public class UpdateTransferDto
    {
        public UpdateTransferDto(int id, int deviceId, string origin, string destination, DateTime transferDate, string receiverName, string? notes)
        {
            Id = id;
            DeviceId = deviceId;
            Origin = origin;
            Destination = destination;
            TransferDate = transferDate;
            ReceiverName = receiverName;
            Notes = notes;
        }

        public int Id { get; set; }
        public int DeviceId { get; set; }
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public DateTime TransferDate { get; set; }
        public string ReceiverName { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}
