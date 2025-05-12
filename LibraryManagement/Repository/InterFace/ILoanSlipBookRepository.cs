using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;
using LibraryManagement.Models;

namespace LibraryManagement.Repository.InterFace
{
    public interface ILoanSlipBookRepository
    {
        Task<ApiResponse<LoanSlipBookResponse>> addLoanSlipBookAsync(LoanSlipBookRequest request);
        Task<ApiResponse<LoanSlipBookResponse>> updateLoanSlipBookAsync(LoanSlipBookRequest request, Guid idLoanSlipBook);
        Task<ApiResponse<string>> deleteLoanSlipBookAsync(Guid idLoanSlipBook);

        Task<List<LoanSlipBookResponse>> getListLoanSlipBook(string token);
    }
}
