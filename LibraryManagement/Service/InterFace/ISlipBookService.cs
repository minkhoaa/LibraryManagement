using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;

namespace LibraryManagement.Service.InterFace
{
    public interface ISlipBookService
    {
        Task<ApiResponse<SlipBookResponse>> addSlipBookAsync(SlipBookRequest request);
        Task<ApiResponse<string>> deleteSlipBookAsync(Guid idLoanSlipBook);
    }
}
