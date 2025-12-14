using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Attributes;

namespace Application.DTOs.Report
{
    /// <summary>
    /// Reporte de determinación de bonificaciones y penalizaciones del personal.
    /// Compara rendimiento de técnicos basado en:
    /// - Valoraciones de sus superiores
    /// - Cantidad de intervenciones realizadas
    /// - Calidad del trabajo (reflejada en equipos que llegan a baja por fallo irreparable)
    /// Requisito de negocio: Funcionalidad 6
    /// </summary>
    public class BonusDeterminationReportDto
    {
        [PdfReportDisplay("ID Técnico")]
        public int TechnicianId { get; set; }

        [PdfReportDisplay("Nombre del Técnico")]
        public string TechnicianName { get; set; } = string.Empty;

        [PdfReportDisplay("Especialidad")]
        public string Specialty { get; set; } = string.Empty;

        [PdfReportDisplay("Años de Experiencia")]
        public int YearsOfExperience { get; set; }

        [PdfReportDisplay("Departamento")]
        public string DepartmentName { get; set; } = string.Empty;

        [PdfReportDisplay("Total Intervenciones")]
        public int TotalInterventions { get; set; }

        [PdfReportDisplay("Mantenimientos Realizados")]
        public int MaintenanceCount { get; set; }

        [PdfReportDisplay("Bajas Registradas")]
        public int DecommissioningCount { get; set; }

        [PdfReportDisplay("Costo Total Mantenimiento")]
        public double TotalMaintenanceCost { get; set; }

        [PdfReportDisplay("Calificación Promedio")]
        public double AverageRating { get; set; }

        [PdfReportDisplay("Máxima Calificación")]
        public double HighestRating { get; set; }

        [PdfReportDisplay("Mínima Calificación")]
        public double LowestRating { get; set; }

        [PdfReportDisplay("Cantidad de Valoraciones")]
        public int RatingCount { get; set; }

        [PdfReportDisplay("Total Bonificaciones")]
        public double TotalBonuses { get; set; }

        [PdfReportDisplay("Total Penalizaciones")]
        public double TotalPenalties { get; set; }

        [PdfReportDisplay("Índice de Efectividad")]
        public double EffectivenessIndex { get; set; }

        [PdfReportDisplay("Recomendación")]
        public string Recommendation { get; set; } = string.Empty;

        [PdfReportDisplay("Monto Ajuste Salario")]
        public double SalaryAdjustmentAmount { get; set; }

        [PdfReportDisplay("Tipo de Ajuste")]
        public string AdjustmentType { get; set; } = string.Empty; // "Bonificación" o "Penalización"

        [PdfReportDisplay("Observaciones")]
        public string Comments { get; set; } = string.Empty;
    }
}
