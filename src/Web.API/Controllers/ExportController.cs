using Microsoft.AspNetCore.Mvc;
using Web.API.DTOs;

namespace Web.API.Controllers
{
    [ApiController]
    [Route("reports")]
    public class ExportController :BaseApiController 
    {

        #region
        [HttpPost("pdf")]
        public async Task<IActionResult> GeneratePdf([FromBody] ExportReportDTO exportReportDTO)
        {

        }
        #endregion
    }
}
