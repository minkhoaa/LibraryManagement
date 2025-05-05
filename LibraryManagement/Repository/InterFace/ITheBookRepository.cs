using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;

namespace LibraryManagement.Repository.InterFace
{
    public interface ITheBookRepository
    {
        public Task<ApiResponse<TheBookResponse>> addTheBookAsync(TheBookRequest request);
        public Task<ApiResponse<TheBookResponse>> updateTheBookAsync(TheBookRequest request, string idTheBook);
        public Task<ApiResponse<string>> deleteTheBookAsync(string idTheBook);

    }
}
