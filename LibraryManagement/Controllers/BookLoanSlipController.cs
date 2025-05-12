using LibraryManagement.Data;
using LibraryManagement.Repository.InterFace;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookLoanSlipController : ControllerBase
    {
        private readonly ILoanSlipBookRepository _repo;
        private readonly LibraryManagermentContext _context;
        public BookLoanSlipController(ILoanSlipBookRepository repository, LibraryManagermentContext context )
        {
            _repo = repository;
            _context = context;
        }
        [HttpPost("getAllBookLoanSlip")]
        public async Task<IActionResult> getAllBookLoanSlip([FromBody] string token)
        {
            var result = await _repo.getListLoanSlipBook(token);
            if (result == null) return Unauthorized("Vui lòng đăng nhập");
            return Ok(result);

        }
    } 
}
