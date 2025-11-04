using Application.DTOs.Inventory;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Report
{
    public class DeviceReportFilterDto
    {
        public DeviceType? DeviceType { get; set; }
        public OperationalState? OperationalState { get; set; }
        public string? Department { get; set; }
    }
}
