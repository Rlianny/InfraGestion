using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    // Service for sorting a view or report by a column
    public interface ISortingService<TReport> where TReport : class
    {
        IEnumerable<TReport> Sort(IEnumerable<TReport> report,string column,bool isAscending=false);
    }
    
}
