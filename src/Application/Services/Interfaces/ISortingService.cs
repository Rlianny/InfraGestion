using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    /// <summary>
    /// Service for sorting a view or report by a column
    /// </summary>
    /// <typeparam name="TReport"></typeparam>
    public interface ISortingService<TReport> where TReport : class
    {
        /// <summary>
        /// Sorts the report in
        /// </summary>
        /// <param name="report">The report to sort</param>
        /// <param name="column">The column(criteria) to sort</param>
        /// <param name="isAscending">If the order its ascending or descending</param>
        /// <returns>The report sorted by the given column in the specified order</returns>
        IEnumerable<TReport> Sort(IEnumerable<TReport> report,string column,bool isAscending=false);
    }
    
}
