using System;
using Domain.Attributes;

namespace Application.DTOs.Report
{
    /// <summary>
    /// Reporte de efectividad del personal técnico.
    /// Calcula métricas de rendimiento basadas en intervenciones, mantenimientos y valoraciones.
    /// </summary>
    public class PersonnelEffectivenessReportDto
    {
        [PdfReportDisplay("ID Técnico")]
        public int TechnicianId { get; set; }

        [PdfReportDisplay("Nombre del Técnico")]
        public string TechnicianName { get; set; } = string.Empty;

        [PdfReportDisplay("Especialidad")]
        public string Specialty { get; set; } = string.Empty;

        [PdfReportDisplay("Años Experiencia")]
        public int YearsOfExperience { get; set; }

        [PdfReportDisplay("Mantenimientos Realizados")]
        public int MaintenanceInterventions { get; set; }

        [PdfReportDisplay("Bajas Registradas")]
        public int DecommissioningRequests { get; set; }

        [PdfReportDisplay("Intervenciones Totales")]
        public int TotalInterventions { get; set; }

        [PdfReportDisplay("Costo Total Mantenimiento")]
        public double TotalMaintenanceCost { get; set; }

        [PdfReportDisplay("Costo Promedio por Intervención")]
        public double AverageCostPerIntervention { get; set; }

        [PdfReportDisplay("Calificación Promedio")]
        public double AverageRating { get; set; }

        [PdfReportDisplay("Última Intervención")]
        public DateTime? LastInterventionDate { get; set; }

        [PdfReportDisplay("Departamento")]
        public string DepartmentName { get; set; } = string.Empty;

        [PdfReportDisplay("Sección")]
        public string SectionName { get; set; } = string.Empty;
    }
}
