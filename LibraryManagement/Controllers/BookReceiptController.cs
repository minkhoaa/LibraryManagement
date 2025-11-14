using LibraryManagement.Dto.Request;
using LibraryManagement.Repository.InterFace;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookReceiptController : ControllerBase
    {
        private readonly IBookReceiptService _bookReceiptService;

        public BookReceiptController(IBookReceiptService bookReceiptService)
        {
            _bookReceiptService = bookReceiptService;
        }

        // Endpoint thêm phiếu nhập sách
        [HttpPost("add_bookreceipt")]
        public async Task<IActionResult> addBookReceipt([FromForm] BookReceiptRequest request)
        {
            var result = await _bookReceiptService.addBookReceiptAsync(request);
            if (result.Success)
                return Created("", result);
            else return BadRequest(result);
        }

        // Endpoint xóa phiếu nhập sách
        [HttpDelete("delete_bookreceipt/{idBookReceipt}")]
        public async Task<IActionResult> deleteBookReceipt(Guid idBookReceipt)
        {
            var result = await _bookReceiptService.deleteBookReceiptAsync(idBookReceipt);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }
        [HttpPost("getAllReceipt")]
        public async Task<IActionResult> getAllReceipt([FromBody]string token)
        {
            var result = await _bookReceiptService.getAllReceiptHistory(token);
            if (result == null) return Unauthorized("Không có quyền admin");
            return Ok(result);
        }
    }
}
