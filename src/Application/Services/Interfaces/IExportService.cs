using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    /// <summary>
    /// Service for exporting a reports and views.
    /// </summary>
    /// <typeparam name="TReport"></typeparam>
    public interface IExportService<TReport> where TReport : class
    {
        Task Export(IEnumerable<TReport> report,string path);
    }
}
