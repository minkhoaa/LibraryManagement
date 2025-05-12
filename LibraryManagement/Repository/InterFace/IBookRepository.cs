using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;
using LibraryManagement.Models;
using Microsoft.Extensions.Primitives;

namespace LibraryManagement.Repository.InterFace
{
    public interface IBookRepository
    {
        public Task<List<BookResponse>> getListBook();
        public Task<ApiResponse<BookResponse>> addBookAsync(BookRequest request);
        public Task<ApiResponse<BookResponse>> updateBookAsync(BookRequest request, string idBook);
        public Task<ApiResponse<string>> deleteBookAsync(string idBook);
        public Task<string> generateNextIdBookAsync();

        public Task<BookResponse> findPost(string name_book);
    }
}
