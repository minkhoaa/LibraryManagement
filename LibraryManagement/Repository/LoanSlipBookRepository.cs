using LibraryManagement.Data;
using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;
using LibraryManagement.Models;
using LibraryManagement.Repository.InterFace;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repository
{
    public class LoanSlipBookRepository : ILoanSlipBookRepository
    {
        private readonly LibraryManagermentContext _context;

        public LoanSlipBookRepository(LibraryManagermentContext context)
        {
            _context = context;
        }

        public Task<ApiResponse<LoanSlipBookResponse>> addLoanSlipBookAsync(LoanSlipBookRequest request)
        {
            //var newLoanSlipBook = new LoanSlipBook
            //{
            //    IdReader = request.IdReader,
            //    IdTheBook = request.IdTheBook,
            //    BorrowDate = DateTime.SpecifyKind(request.BorrowDate, DateTimeKind.Utc),
            //    ReturnDate = DateTime.SpecifyKind(request.ReturnDate, DateTimeKind.Utc)
            //};
            //var thebook = _context.TheBooks.FirstOrDefaultAsync(tb => tb.)
            throw new NotImplementedException();
        }

        public Task<ApiResponse<string>> deleteLoanSlipBookAsync(Guid idLoanSlipBook)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<LoanSlipBookResponse>> updateLoanSlipBookAsync(LoanSlipBookRequest request, Guid idLoanSlipBook)
        {
            throw new NotImplementedException();
        }
    }
}
