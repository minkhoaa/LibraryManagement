using LibraryManagement.Dto.Request;
using LibraryManagement.Repository.InterFace;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheBookController : ControllerBase
    {
        private readonly ITheBookRepository _theBookRepository;

        public TheBookController(ITheBookRepository theBookRepository)
        {
            _theBookRepository = theBookRepository;
        }

        // Endpoint thêm cuốn sách
        [HttpPost("add_thebook")]
        public async Task<IActionResult> addTheBook(TheBookRequest request)
        {
            var result = await _theBookRepository.addTheBookAsync(request);
            if (result.Success)
                return Created("", result);
            return BadRequest(result);
        }

        // Endpoint sửa cuốn sách
        [HttpPut("update_thebook/{idTheBook}")]
        public async Task<IActionResult> updateTheBook(TheBookRequest request, string idTheBook)
        {
            var result = await _theBookRepository.updateTheBookAsync(request, idTheBook);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        // Endpoint xóa cuốn sách
        [HttpDelete("delete_thebook/{idTheBook}")]
        public async Task<IActionResult> deleteTheBook(string idTheBook)
        {
            var result = await _theBookRepository.deleteTheBookAsync(idTheBook);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }
    }
}
