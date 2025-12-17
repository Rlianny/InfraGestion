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
        [PdfReportDisplay("Técnico")]
        public string TechnicianName { get; set; } = string.Empty;

        [PdfReportDisplay("Total Intervenciones")]
        public int TotalInterventions { get; set; }
        [PdfReportDisplay("Menor Calificación")]
        public double LowestRating { get; set; }
        
        [PdfReportDisplay("Calificación Promedio")]
        public double AverageRating { get; set; }

        [PdfReportDisplay("Mayor Calificación")]
        public double HighestRating { get; set; }
        
        [PdfReportDisplay("Cantidad de Valoraciones")]
        public int RatingCount { get; set; }

    }
}
