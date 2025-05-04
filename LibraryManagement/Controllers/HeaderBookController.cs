using LibraryManagement.Dto.Request;
using LibraryManagement.Repository.InterFace;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeaderBookController : ControllerBase
    {
        private readonly IHeaderBookRepository _headerBookRepository;

        public HeaderBookController(IHeaderBookRepository headerBookRepository)
        {
            _headerBookRepository = headerBookRepository;
        }

        // Endpoint lấy danh sách đầu sách
        [HttpGet("list_headerbook")]
        public async Task<IActionResult> getListHeaderBook()
        {
            try
            {
                return Ok(await _headerBookRepository.getListHeaderBook());
            }
            catch
            {
                return BadRequest();
            }
        }

        // Endpoint tạo đầu sách
        [HttpPost("add_headerbook")]
        public async Task<IActionResult> addHeaderBook(HeaderBookRequest request)
        {
            var result = await _headerBookRepository.addHeaderBookAsync(request);
            if (result.Success)
                return Created("", result);
            return BadRequest(result);
        }

        // Endpoint xóa đầu sách
        [HttpDelete("delete_headerbook/{idHeaderBook}")]
        public async Task<IActionResult> deleteHeaderBook(Guid idHeaderBook)
        {
            var result = await _headerBookRepository.deleteHeaderBookAsync(idHeaderBook);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        // Endpoint sửa đầu sách
        [HttpPut("update_headerbook/{idHeaderBook}")]
        public async Task<IActionResult> updateHeaderBook(HeaderBookRequest request, Guid idHeaderBook)
        {
            var result = await _headerBookRepository.updateHeaderBookAsync(request, idHeaderBook);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }
    }
}
