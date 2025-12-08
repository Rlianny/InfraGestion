using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Report
{
    public class DecommissioningReportDto
    {
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public string DecommissionCause { get; set; }
        public string FinalDestination { get; set; }
        public string ReceiverName { get; set; }
        public DateTime DecommissionDate { get; set; }
    }
}
