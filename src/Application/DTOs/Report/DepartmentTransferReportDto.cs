using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Attributes;
namespace Application.DTOs.Report
{
    /// <summary>
    /// Reporte de transferencias de equipos entre departamentos.
    /// Incluye información de envío, recepción e identidad de los responsables.
    /// </summary>
    public class DepartmentTransferReportDto
    {
        [PdfReportDisplay("ID Transferencia")]
        public int TransferId { get; set; }

        [PdfReportDisplay("ID Equipo")]
        public int DeviceId { get; set; }

        [PdfReportDisplay("Nombre del Equipo")]
        public string DeviceName { get; set; } = string.Empty;

        [PdfReportDisplay("Tipo de Equipo")]
        public string DeviceType { get; set; } = string.Empty;

        [PdfReportDisplay("Fecha de Transferencia")]
        public DateTime TransferDate { get; set; }

        [PdfReportDisplay("Departamento de Origen")]
        public string SourceDepartmentName { get; set; } = string.Empty;

        [PdfReportDisplay("Sección de Origen")]
        public string SourceSectionName { get; set; } = string.Empty;

        [PdfReportDisplay("Departamento de Destino")]
        public string DestinationDepartmentName { get; set; } = string.Empty;

        [PdfReportDisplay("Sección de Destino")]
        public string DestinationSectionName { get; set; } = string.Empty;

        [PdfReportDisplay("Nombre Remitente")]
        public string SenderName { get; set; } = string.Empty;

        [PdfReportDisplay("Nombre Receptor")]
        public string ReceiverName { get; set; } = string.Empty;

        [PdfReportDisplay("Empresa/Origen")]
        public string SenderCompanyOrganization { get; set; } = string.Empty;

        [PdfReportDisplay("Estado de Transferencia")]
        public string TransferStatus { get; set; } = string.Empty;

        [PdfReportDisplay("Fecha de Entrega")]
        public DateTime? DeliveryDate { get; set; }
    }
}
