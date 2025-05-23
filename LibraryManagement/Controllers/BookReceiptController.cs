﻿using LibraryManagement.Dto.Request;
using LibraryManagement.Repository.InterFace;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookReceiptController : ControllerBase
    {
        private readonly IBookReceiptRepository _bookReceiptRepository;

        public BookReceiptController(IBookReceiptRepository bookReceiptRepository)
        {
            _bookReceiptRepository = bookReceiptRepository;
        }

        // Endpoint thêm phiếu nhập sách
        [HttpPost("add_bookreceipt")]
        public async Task<IActionResult> addBookReceipt([FromBody] BookReceiptRequest request)
        {
            var result = await _bookReceiptRepository.addBookReceiptAsync(request);
            if (result.Success)
                return Created("", result);
            else return BadRequest(result);
        }

        // Endpoint xóa phiếu nhập sách
        [HttpDelete("delete_bookreceipt/{idBookReceipt}")]
        public async Task<IActionResult> deleteBookReceipt(Guid idBookReceipt)
        {
            var result = await _bookReceiptRepository.deleteBookReceiptAsync(idBookReceipt);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }
        [HttpPost("getAllReceipt")]
        public async Task<IActionResult> getAllReceipt([FromBody]string token)
        {
            var result = await _bookReceiptRepository.getAllReceiptHistory(token);
            if (result == null) return Unauthorized("Không có quyền admin");
            return Ok(result);
        }
    }
}
