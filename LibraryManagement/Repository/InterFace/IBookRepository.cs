using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;

namespace LibraryManagement.Repository.InterFace
{
    public interface IBookRepository
    {
        public Task<ApiResponse<HeaderBookResponse>> addBookAsync(HeaderBookCreationRequest request);
        public Task<ApiResponse<HeaderBookResponse>> updateBookAsync(HeaderBookUpdateRequest request, string idBook, string idTheBook);
        public Task<ApiResponse<string>> deleteBookAsync(string idBook);
    }
}
