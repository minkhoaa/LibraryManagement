using LibraryManagement.Dto.Request;
using LibraryManagement.Repository.InterFace;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeReaderController : ControllerBase
    {
        private readonly ITypeReaderRepository _typeReaderRepository;

        public TypeReaderController(ITypeReaderRepository typeReaderRepository)
        {
            _typeReaderRepository = typeReaderRepository;
        }

        // Endpoint thêm loại độc giả
        [HttpPost("add_typereader")]
        public async Task<IActionResult> addTypeReader([FromBody] TypeReaderRequest request)
        {
            var result = await _typeReaderRepository.addTypeReaderAsync(request);
            if (result.Success)
                return Created("", result);
            return BadRequest(result);
        }

        // Endpoint sửa loại độc giả
        [HttpPut("update_typereader/{idTypeReader}")]
        public async Task<IActionResult> updateTypeReader([FromBody] TypeReaderRequest request, Guid idTypeReader)
        {
            var result = await _typeReaderRepository.updateTypeReaderAsync(request, idTypeReader);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        // Endpoint xóa loại độc giả
        [HttpDelete("delete_typereader/{idTypeReader}")]
        public async Task<IActionResult> deleteTypeReader(Guid idTypeReader)
        {
            var result = await _typeReaderRepository.deleteTypeReaderAsync(idTypeReader);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }
    }
}
