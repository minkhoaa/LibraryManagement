using LibraryManagement.Dto.Request;
using LibraryManagement.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryReportController : ControllerBase
    {
        private readonly ICategoryReportService _categoryReportService;
        public CategoryReportController(ICategoryReportService categoryReportService)
        {
            _categoryReportService = categoryReportService;
        }

        // Endpoint tạo báo cáo thể loại theo tháng
        [HttpPost("add_category_report")]
        public async Task<IActionResult> addCategoryReport([FromBody] CategoryReportRequest request)
        {
            var result = await _categoryReportService.addCategoryReportAsync(request);
            if (result.Success)
                return Created("", result);
            return BadRequest(result);
        }
    }
}
