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

        [PdfReportDisplay("Mantenimientos Último Año")]
        public int MaintenanceCountLastYear { get; set; }

        [PdfReportDisplay("Costo Total Mantenimiento")]
        public double TotalMaintenanceCost { get; set; }

    }
}
