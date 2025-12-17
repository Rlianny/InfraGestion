using Domain.Attributes;
namespace Application.DTOs.Report
{
    public class DeviceMantainenceReportDto
    {
        [PdfReportDisplay("Fecha")]
        public DateTime Date { get; set; }
        [PdfReportDisplay("Tipo")]
        public string Type { get; set; }
        [PdfReportDisplay("Descripción")]
        public string Description { get; set; }
        [PdfReportDisplay("Técnico")]
        public string Technician { get; set; }
    }
}