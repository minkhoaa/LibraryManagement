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

        // Endpoint lấy danh sách sách
        [HttpGet("list_book")]
        public async Task<IActionResult> getListBook()
        {
            try
            {
                return Ok(await _bookRepository.getListBook());
            }
            catch
            {
                return BadRequest();
            }
        }

        // Endpoint thêm sách
        [HttpPost("add_book")]
        public async Task<IActionResult> addBook(BookRequest request)
        {
            var result = await _bookRepository.addBookAsync(request);
            if (result.Success)
                return Created("", result);
            return BadRequest(result);
        }

        // Endpoint sửa sách
        [HttpPut("update_book/{idBook}")]
        public async Task<IActionResult> updateBook(BookRequest request, string idBook)
        {
            var result = await _bookRepository.updateBookAsync(request, idBook);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        // Endpoint xóa sách
        [HttpDelete("delete_book/{idBook}")]
        public async Task<IActionResult> deleteBook(string idBook)
        {
            var result = await _bookRepository.deleteBookAsync(idBook);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }
    }
}
