using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;

namespace LibraryManagement.Repository.InterFace
{
    public interface ILoanBookService
    {
        Task<ApiResponse<LoanBookResponse>> addLoanBookAsync(LoanSlipBookRequest request);
        Task<ApiResponse<string>> deleteLoanBookAsync(Guid idLoanSlipBook);

        Task<List<LoanSlipBookResponse>> getListLoanSlipBook(string token);
    }
}
