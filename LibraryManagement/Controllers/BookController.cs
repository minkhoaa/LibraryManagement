using LibraryManagement.Dto.Request;
using LibraryManagement.Repository.InterFace;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // Endpoint tạo sách
        [HttpPost("add_book")]
        public async Task<IActionResult> addHeaderBook([FromBody] HeaderBookCreationRequest request)
        {
            var result = await _bookRepository.addBookAsync(request);
            if (result.Success)
                return Created("", result);
            return BadRequest(result);
        }

        // Endpoint xóa sách
        [HttpDelete("delete_book/{idBook}/{idTheBook}")]
        public async Task<IActionResult> deleteHeaderBook(string idBook)
        {
            var result = await _bookRepository.deleteBookAsync(idBook);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        // Endpoint sửa sách
        [HttpPut("update_book/{idBook}/{idTheBook}")]
        public async Task<IActionResult> updateHeaderBook([FromBody] HeaderBookUpdateRequest request, 
                                                          string idBook, 
                                                          string idTheBook)
        {
            var result = await _bookRepository.updateBookAsync(request, idBook, idTheBook);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }
    }
}
