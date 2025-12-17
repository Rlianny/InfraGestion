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
        [PdfReportDisplay("Técnico")]
        public string TechnicianName { get; set; } = string.Empty;

        [PdfReportDisplay("Especialidad")]
        public string Specialty { get; set; } = string.Empty;

        [PdfReportDisplay("Índice de Correlación")]
        public double CorrelationIndex { get; set; }

        [PdfReportDisplay("Calificación Promedio")]
        public double AveragePerformanceRating { get; set; }

        

    }
}
