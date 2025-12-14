using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public record TableRow(IReadOnlyList<string> Cells);
    public record Table(IReadOnlyList<string> Headers, IReadOnlyList<TableRow> Rows);
    public interface IPdfReportGenerator
    {
        Task<byte[]> CreatePdfTable(Table table);
    }
}
