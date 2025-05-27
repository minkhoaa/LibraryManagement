using LibraryManagement.Data;
using LibraryManagement.Dto.Request;
using LibraryManagement.Repository.InterFace;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanSlipBookController : ControllerBase
    {
        private readonly ILoanBookService _loanSlipBookService;
        private readonly LibraryManagermentContext _context;

        public LoanSlipBookController(ILoanBookService loanSlipBookService, LibraryManagermentContext context )
        {
            _loanSlipBookService = loanSlipBookService;
            _context = context;
        }

        // Enpoint danh sách phiếu mượn trả sách
        [HttpPost("getAllBookLoanSlip")]
        public async Task<IActionResult> getAllBookLoanSlip([FromBody] string token)
        {
            var result = await _loanSlipBookService.getListLoanSlipBook(token);
            if (result == null) return Unauthorized("Vui lòng đăng nhập");
            return Ok(result);

        }

        // Enpoint tạo phiếu mượn sách
        [HttpPost("add_loanbook")]
        public async Task<IActionResult> addLoanBook([FromBody] LoanSlipBookRequest request)
        {
            var result = await _loanSlipBookService.addLoanBookAsync(request);
            if (result.Success)
                return Created("", result);
            return BadRequest(result);
        }

        // Endpoint xóa phiếu mượn sách
        [HttpDelete("delete_loanbook/{idLoanSlipBook}")]
        public async Task<IActionResult> deleteLoanBook(Guid idLoanSlipBook)
        {
            var result = await _loanSlipBookService.deleteLoanBookAsync(idLoanSlipBook);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        // Enpoint tạo phiếu trả sách

        // Endpoint xóa phiếu trả sách
    } 
}
