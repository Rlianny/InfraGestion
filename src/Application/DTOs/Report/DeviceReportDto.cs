using Domain.Attributes;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Report
{
    /// <summary>
    /// Reporte de inventario de equipos.
    /// Muestra información detallada de equipos: especificaciones, ubicación, estado operacional,
    /// historial de mantenimientos y costos asociados.
    /// Requisito de negocio: Funcionalidad 1 - Inventario base
    /// </summary>
    /// <summary>
    
    public class DeviceReportDto
    {
        public DeviceReportDto(int deviceId, string name, DeviceType deviceType, OperationalState operationalState, int departmentId, string departmentName, int? sectionId, string? sectionName, DateTime acquisitionDate, int maintenanceCount, double totalMaintenanceCost, DateTime? lastMaintenanceDate)
        {
            DeviceId = deviceId;
            Name = name;
            DeviceType = deviceType;
            OperationalState = operationalState;
            DepartmentId = departmentId;
            DepartmentName = departmentName;
            SectionId = sectionId;
            SectionName = sectionName;
            AcquisitionDate = acquisitionDate;
            MaintenanceCount = maintenanceCount;
            TotalMaintenanceCost = totalMaintenanceCost;
            LastMaintenanceDate = lastMaintenanceDate;
        }

        public int DeviceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DeviceType DeviceType { get; set; }
        public OperationalState OperationalState { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public int? SectionId { get; set; }
        public string? SectionName { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public int MaintenanceCount { get; set; }
        public double TotalMaintenanceCost { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }
    }
}
