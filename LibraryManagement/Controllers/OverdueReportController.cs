using LibraryManagement.Dto.Request;
using LibraryManagement.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OverdueReportController : ControllerBase
    {
        private readonly IOverdueReportService _overdueReportService;

        public OverdueReportController(IOverdueReportService overdueReportService)
        {
            _overdueReportService = overdueReportService;
        }

        // Endpoint tạo báo cáo sách trả trễ theo ngày
        [HttpPost("add_overdue_report")]
        public async Task<IActionResult> addOverdueReport([FromBody] OverdueReportRequest request)
        {
            var result = await _overdueReportService.addOverdueReportAsync(request);
            if (result.Success)
                return Created("", result);
            return BadRequest(result);
        }

        // Endpoint xóa báo cáo sách trả trễ
        [HttpDelete("delete_overdue_report/{idOverdueReport}")]
        public async Task<IActionResult> deleteOverdueReport(Guid idOverdueReport)
        {
            var result = await _overdueReportService.deleteOverReportAsync(idOverdueReport);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }
    }
}
