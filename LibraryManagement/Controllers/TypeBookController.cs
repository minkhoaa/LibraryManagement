using LibraryManagement.Dto.Request;
using LibraryManagement.Repository.InterFace;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeBookController : ControllerBase
    {
        private readonly ITypeBookRepository _typeBookRepository;

        public TypeBookController(ITypeBookRepository typeBookRepository)
        {
            _typeBookRepository = typeBookRepository;
        }

        // Endpoint thêm loại sách
        [HttpPost("add_typebook")]
        public async Task<IActionResult> addTypeBook(TypeBookRequest request)
        {
            var result = await _typeBookRepository.addTypeBookAsync(request);
            if (result.Success)
                return Created("", result);
            return BadRequest(result);
        }

        // Endpoint sửa loại sách
        [HttpPut("update_typebook/{idTypeBook}")]
        public async Task<IActionResult> updateTypeBook(TypeBookRequest request, Guid idTypeBook)
        {
            var result = await _typeBookRepository.updateTypeBookAsync(request, idTypeBook);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        // Endpoint xóa loại sách
        [HttpDelete("delete_typebook/{idTypeBook}")]
        public async Task<IActionResult> deleteTypeBook(Guid idTypeBook)
        {
            var result = await _typeBookRepository.deleteTypeBook(idTypeBook);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }
    }
}
