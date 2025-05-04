using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;

namespace LibraryManagement.Repository.InterFace
{
    public interface IHeaderBookRepository
    {
        public Task<List<HeaderBookResponse>> getListHeaderBook();
        public Task<ApiResponse<HeaderBookResponse>> addHeaderBookAsync(HeaderBookRequest request);
        public Task<ApiResponse<HeaderBookResponse>> updateHeaderBookAsync(HeaderBookRequest request, Guid idHeaderBook);
        public Task<ApiResponse<string>> deleteHeaderBookAsync(Guid idHeaderBook);
    }
}
