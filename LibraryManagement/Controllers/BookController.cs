using LibraryManagement.Dto.Request;
using LibraryManagement.Repository.InterFace;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

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
        public async Task<IActionResult> addHeaderBook([FromForm] HeaderBookCreationRequest request)
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
        public async Task<IActionResult> updateHeaderBook([FromForm] HeaderBookUpdateRequest request, 
                                                          string idBook, 
                                                          string idTheBook)
        {
            var result = await _bookRepository.updateBookAsync(request, idBook, idTheBook);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        [HttpPost("getBookAndComments")]
        public async Task<IActionResult> getBooksAndComments([FromBody] GetHeaderBookDtoInput dto)
        {
            var result = await _bookRepository.getHeaderbookandCommentsByid(dto);
            return Ok(result);
        }
        [HttpPost("getEvaluation")]

        public async Task<IActionResult> getDetailedEvaluation(EvaluationDetailInput dto)
        {
            var result = await _bookRepository.getBooksEvaluation(dto);
            if (result == null) return Unauthorized("Không có quyền admin");
            return Ok(result);
        }

        [HttpPost("LikeHeaderBook")]
        public async Task<IActionResult> likeHeaderBook(EvaluationDetailInput dto)
        {
            var result = await _bookRepository.LikeHeaderBook(dto);
            if (result == false) return Unauthorized("Vui lòng đăng nhập ");
            return Ok("Success");
        }
        [HttpPost("getAllBooksAndComments")]
        public async Task<IActionResult> getAllBooksAndComments([FromBody] string token)
        {
            var result = await _bookRepository.getAllHeaderbookandComments(token);
            if (result == null) return Unauthorized("Vui lòng đăng nhập");
            return Ok(result); 
        }
    }
}
