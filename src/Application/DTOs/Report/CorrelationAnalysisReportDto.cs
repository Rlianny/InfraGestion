using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Attributes;

namespace Application.DTOs.Report
{
    /// <summary>
    /// Análisis de correlación entre rendimiento de técnicos y longevidad de equipos.
    /// Identifica los 5 técnicos con peor correlación:
    /// - Alto costo de mantenimiento
    /// - Baja longevidad de equipos (dados de baja por fallo técnico irreparable)
    /// Requisito de negocio: Funcionalidad 4
    /// </summary>
    public class CorrelationAnalysisReportDto
    {
        [PdfReportDisplay("Posición")]
        public int Rank { get; set; }

        [PdfReportDisplay("ID Técnico")]
        public int TechnicianId { get; set; }

        [PdfReportDisplay("Nombre del Técnico")]
        public string TechnicianName { get; set; } = string.Empty;

        [PdfReportDisplay("Especialidad")]
        public string Specialty { get; set; } = string.Empty;

        [PdfReportDisplay("Años de Experiencia")]
        public int YearsOfExperience { get; set; }

        [PdfReportDisplay("Tipo de Equipo")]
        public string EquipmentType { get; set; } = string.Empty;

        [PdfReportDisplay("Equipos Mantenidos")]
        public int EquipmentCount { get; set; }

        [PdfReportDisplay("Equipos Dados de Baja")]
        public int DecommissionedEquipmentCount { get; set; }

        [PdfReportDisplay("Equipos Baja por Fallo Irreparable")]
        public int IrreparableFailureCount { get; set; }

        [PdfReportDisplay("Costo Total Mantenimiento")]
        public double TotalMaintenanceCost { get; set; }

        [PdfReportDisplay("Costo Promedio por Equipo")]
        public double AverageMaintenanceCostPerEquipment { get; set; }

        [PdfReportDisplay("Longevidad Promedio Equipos (Años)")]
        public double AverageEquipmentLongevity { get; set; }

        [PdfReportDisplay("Índice de Correlación")]
        public double CorrelationIndex { get; set; }

        [PdfReportDisplay("Rendimiento Promedio")]
        public double AveragePerformanceRating { get; set; }

        [PdfReportDisplay("Bonificaciones Totales")]
        public double TotalBonuses { get; set; }

        [PdfReportDisplay("Penalizaciones Totales")]
        public double TotalPenalties { get; set; }

        [PdfReportDisplay("Balance Neto")]
        public double NetBalance { get; set; }

        [PdfReportDisplay("Observaciones")]
        public string Observations { get; set; } = string.Empty;
    }
}
