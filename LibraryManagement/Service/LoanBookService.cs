using LibraryManagement.Data;
using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;
using LibraryManagement.Models;
using LibraryManagement.Repository.InterFace;
using LibraryManagement.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repository
{
    public class LoanBookService : ILoanBookService
    {
        private readonly LibraryManagermentContext _context;
        private readonly IAuthenService _account;
        private readonly IParameterService _parameterRepository;


        public LoanBookService(LibraryManagermentContext context, 
                                      IAuthenService authen, 
                                      IParameterService parameterRepository)
        {
            _account = authen;
            _context = context;
            _parameterRepository = parameterRepository;
        }

        // Tạo phiếu mượn sách
        public async Task<ApiResponse<LoanBookResponse>> addLoanBookAsync(LoanSlipBookRequest request)
        {
            // Các quy định
            int cardExpirationMonths = await _parameterRepository.getValueAsync("CardExpirationDate");
            int borrowingLimit = await _parameterRepository.getValueAsync("BorrowingLimit");
            int borrowingPeriodDays = await _parameterRepository.getValueAsync("BorrowingPeriodDays");

            // Kiểm tra Reader có tồn tại hay không
            var reader = await _context.Readers.FirstOrDefaultAsync(rd => rd.IdReader == request.IdReader);
            if (reader == null)
            {
                return ApiResponse<LoanBookResponse>.FailResponse("không tìm thấy độc giả", 404);
            }

            // Kiểm tra TheBook có tồn tại hay không
            var theBook = await _context.TheBooks.FirstOrDefaultAsync(tb => tb.IdTheBook == request.IdTheBook);
            if (theBook == null)
            {
                return ApiResponse<LoanBookResponse>.FailResponse("không tìm thấy cuốn sách", 404);
            }

            // Kiểm tra trạng thái cuốn sách
            if (theBook.Status == "Đã mượn")
            {
                return ApiResponse<LoanBookResponse>.FailResponse("Cuốn sách đang được mượn", 400);
            }

            // Kiểm tra thẻ độc giả còn hạn hay không
            DateTime cardIssueDate = reader.CreateDate;
            DateTime cardExpirationDate = cardIssueDate.AddMonths(cardExpirationMonths);
            if (cardExpirationDate < DateTime.Now)
            {
                return ApiResponse<LoanBookResponse>.FailResponse("Thẻ độc giả đã quá hạn 6 tháng", 400);
            }

            // Kiểm tra trong 4 ngày không được mượn quá 5 quyển sách
            DateTime dateThreshold = DateTime.UtcNow.AddDays(-borrowingPeriodDays);
            int borrowedInPeriod = await _context.LoanSlipBooks
                .Where(l => l.IdReader == request.IdReader && l.BorrowDate >= dateThreshold)
                .CountAsync();

            if (borrowedInPeriod >= borrowingLimit)
            {
                return ApiResponse<LoanBookResponse>.FailResponse($"Trong {borrowingPeriodDays} ngày gần nhất, độc giả đã mượn tối đa {borrowingLimit} cuốn sách", 400);
            }

            // Tạo phiếu mượn sách
            var loanBook = new LoanSlipBook
            {
                IdTheBook = request.IdTheBook,
                IdReader = request.IdReader,
                BorrowDate = DateTime.UtcNow,
            };
            _context.LoanSlipBooks.Add(loanBook);
            theBook.Status = "Đã mượn";
            _context.TheBooks.Update(theBook);

            await _context.SaveChangesAsync();

            var response = new LoanBookResponse
            {
                IdLoanSlipBook = loanBook.IdLoanSlipBook,
                IdReader = loanBook.IdReader,
                IdTheBook = loanBook.IdTheBook,
                BorrowDate = loanBook.ReturnDate
            };
            return ApiResponse<LoanBookResponse>.SuccessResponse("Tạo phiếu mượn thành công", 200, response);
        }

        // Xóa phiếu mượn
        public async Task<ApiResponse<string>> deleteLoanBookAsync(Guid idLoanSlipBook)
        {
            var deleteLoanBook = await _context.LoanSlipBooks.FirstOrDefaultAsync(lb => lb.IdLoanSlipBook == idLoanSlipBook);
            if (deleteLoanBook == null)
            {
                return ApiResponse<string>.FailResponse("Không tìm thấy phiếu mượn", 404);
            }
            _context.LoanSlipBooks.Remove(deleteLoanBook);
            await _context.SaveChangesAsync();
            return ApiResponse<string>.SuccessResponse("Đã xóa phiếu mượn sách thành công", 200, "");
        }

        public async Task<List<LoanSlipBookResponse>> getListLoanSlipBook(string token)
        {
            var user = await _account.AuthenticationAsync(token);
            if (user == null) return null;
            var result = await _context.LoanSlipBooks.Select(a => new LoanSlipBookResponse
            {
                IdLoanSlipBook = a.IdLoanSlipBook,
                IdTheBook = a.IdTheBook,
                IdReader = a.IdReader,
                BorrowDate = a.BorrowDate,
                ReturnDate = a.ReturnDate,
                FineAmount = a.FineAmount
            }).ToListAsync();
            return result; 
        }
    }
}
