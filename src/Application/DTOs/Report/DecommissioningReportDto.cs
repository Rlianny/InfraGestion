using Domain.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Application.DTOs.Report
{
    public class DecommissioningReportDto
    {
        [PdfReportDisplay("EquipoId")]
        public int EquipmentId { get; set; }

        [PdfReportDisplay("Nombre de Equipo")]
        public string EquipmentName { get; set; }

        [PdfReportDisplay("Causa de Baja")]
        public string DecommissionCause { get; set; }

        [PdfReportDisplay("Destino Final")]
        public string FinalDestination { get; set; }

        [PdfReportDisplay("Nombre del Recividor")]
        public string ReceiverName { get; set; }

        [PdfReportDisplay("Fecha de Baja")]
        public DateTime DecommissionDate { get; set; }
    }
}
