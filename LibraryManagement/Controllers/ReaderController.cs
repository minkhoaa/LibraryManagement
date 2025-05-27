using LibraryManagement.Dto.Request;
using LibraryManagement.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/reader/[controller]")]
    [ApiController]
    public class ReaderController : ControllerBase
    {
        private IReaderService _readerService;

        public ReaderController(IReaderService readerService)
        {
            _readerService = readerService;
        }

        // Endpoint lấy danh sách độc giả
    //    [Authorize(Roles = "Reader")]
        [HttpPost("list_reader")]
        public async Task<IActionResult> gettAllReader([FromBody]string token)
        {
            var result = await _readerService.getAllReaderAsync(token);
            if (result == null) return Unauthorized();
            return Ok(result);   
        }

        // Endpoint thêm độc giả
        [HttpPost("add_reader")]
        public async Task<IActionResult> addNewReader([FromForm] ReaderCreationRequest request)
        {
            var result = await _readerService.addReaderAsync(request);
            if (result.Success)
                return Created("", result);
            return BadRequest(result);
        }

        // Endpont sửa độc giả
        [HttpPut("update_reader/{idReader}")]
        public async Task<IActionResult> updateReader([FromForm] ReaderUpdateRequest request, string idReader)
        {
            var result = await _readerService.updateReaderAsync(request, idReader);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        // Endpoint xóa độc giả
        [HttpDelete("delete_reader/{idReader}")]
        public async Task<IActionResult> deleteReader(string idReader)
        {
            var result = await _readerService.deleteReaderAsync(idReader);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        [HttpPost("find_reader")]
        public async Task<IActionResult> findReader(FindReaderInputDto dto)
        {
            try
            {
                var result = await _readerService.findReaderAsync(dto);
                if (result == null) return Unauthorized("Yêu cầu quyền admin");
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
