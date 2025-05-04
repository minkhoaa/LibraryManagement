using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;
using LibraryManagement.Models;

namespace LibraryManagement.Repository.InterFace
{
    public interface IBookRepository
    {
        public Task<List<BookResponse>> getListBook();
        public Task<ApiResponse<BookResponse>> addBookAsync(BookRequest request);
        public Task<ApiResponse<BookResponse>> updateBookAsync(BookRequest request, string idBook);
        public Task<ApiResponse<string>> deleteBookAsync(string idBook);
    }
}
