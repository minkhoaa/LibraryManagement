using LibraryManagement.Dto.Request;
using LibraryManagement.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/reader/[controller]")]
    [ApiController]
    public class ReaderController : ControllerBase
    {
        private IReaderRepository _readerRepository;

        public ReaderController(IReaderRepository readerRepository)
        {
            _readerRepository = readerRepository;
        }

        // Endpoint lấy danh sách độc giả
    //    [Authorize(Roles = "Reader")]
        [HttpGet("list_reader")]
        public async Task<IActionResult> gettAllReader()
        {
            try
            {
                return Ok(await _readerRepository.getAllReaderAsync());
            }
            catch
            {
                return BadRequest();
            }
        }

        // Endpoint thêm độc giả
        [HttpPost("add_reader")]
        public async Task<IActionResult> addNewReader(ReaderRequest request)
        {
            var result = await _readerRepository.addReaderAsync(request);
            if (result.Success)
                return Created("", result);
            return BadRequest(result);
        }

        // Endpont sửa độc giả
        [HttpPut("update_reader/{idReader}")]
        public async Task<IActionResult> updateReader(ReaderUpdateRequest request, Guid idReader)
        {
            var result = await _readerRepository.updateReaderAsync(request, idReader);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        // Endpoint xóa độc giả
        [HttpDelete("delete_reader/{idReader}")]
        public async Task<IActionResult> deleteReader(Guid idReader)
        {
            var result = await _readerRepository.deleteReaderAsync(idReader);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }
    }
}
