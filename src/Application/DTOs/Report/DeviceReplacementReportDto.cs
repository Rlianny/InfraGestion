using System;
using Domain.Attributes;

namespace Application.DTOs.Report
{
    /// <summary>
    /// Reporte de equipos que deben ser reemplazados.
    /// Identifica equipos que han recibido más de 3 mantenimientos en el último año.
    /// Requisito de normativa: estos equipos deben ser reemplazados.
    /// </summary>
    public class DeviceReplacementReportDto
    {
        [PdfReportDisplay("ID Equipo")]
        public int DeviceId { get; set; }

        [PdfReportDisplay("Nombre del Equipo")]
        public string DeviceName { get; set; } = string.Empty;

        [PdfReportDisplay("Tipo de Equipo")]
        public string DeviceType { get; set; } = string.Empty;

        [PdfReportDisplay("Departamento")]
        public string DepartmentName { get; set; } = string.Empty;

        [PdfReportDisplay("Sección")]
        public string SectionName { get; set; } = string.Empty;

        [PdfReportDisplay("Fecha de Adquisición")]
        public DateTime AcquisitionDate { get; set; }

        [PdfReportDisplay("Mantenimientos Último Año")]
        public int MaintenanceCountLastYear { get; set; }

        [PdfReportDisplay("Costo Total Mantenimiento")]
        public double TotalMaintenanceCost { get; set; }

        [PdfReportDisplay("Costo Promedio por Mantenimiento")]
        public double AverageMaintenanceCost { get; set; }

        [PdfReportDisplay("Última Fecha Mantenimiento")]
        public DateTime? LastMaintenanceDate { get; set; }

        [PdfReportDisplay("Años en Servicio")]
        public int YearsInService { get; set; }

        [PdfReportDisplay("Estado Operacional")]
        public string OperationalState { get; set; } = string.Empty;

        [PdfReportDisplay("Razón de Reemplazo")]
        public string ReplacementReason { get; set; } = "Mantenimientos excesivos (>3 en último año)";
    }
}
