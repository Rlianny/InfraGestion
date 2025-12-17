using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.DTOs.Report
{
    public class Report<T>
    {
      
        public IEnumerable<T> ReportData { get; set; }
        public byte[] PdfBytes { get; set; }
    }
}

