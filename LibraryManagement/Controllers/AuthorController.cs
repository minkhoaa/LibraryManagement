using LibraryManagement.Dto.Request;
using LibraryManagement.Repository.InterFace;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        // Endpoint lấy danh sách tác giả
        [HttpGet("list_author")]
        public async Task<IActionResult> gettListAuthor()
        {
            try
            {
                return Ok(await _authorRepository.getListAuthor());
            }
            catch
            {
                return BadRequest();
            }
        }

        // Endpoint thêm tác giả
        [HttpPost("add_author")]
        public async Task<IActionResult> addAuthor(AuthorCreationRequest request)
        {
            var result = await _authorRepository.addAuthorAsync(request);
            if (result.Success)
                return Created("", result);
            return BadRequest(result);
        }

        // Endpoint sửa tác giả
        [HttpPut("update_author/{idAuthor}")]
        public async Task<IActionResult> updateAuthor(AuthorUpdateRequest request, Guid idAuthor)
        {
            var result = await _authorRepository.updateAuthorAsync(request, idAuthor);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        // Endpoint xóa tác giả
        [HttpDelete("delete_author/{idAuthor}")]
        public async Task<IActionResult> deleteAuthor(Guid idAuthor)
        {
            var result = await _authorRepository.deleteAuthorAsync(idAuthor);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }
    }
}
