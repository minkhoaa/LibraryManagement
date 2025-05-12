using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;
<<<<<<< HEAD
using LibraryManagement.Models;
using Microsoft.Extensions.Primitives;
=======
>>>>>>> 804cd4f59b533a1f03e4525db3d688cc55bd999f

namespace LibraryManagement.Repository.InterFace
{
    public interface IBookRepository
    {
        public Task<ApiResponse<HeaderBookResponse>> addBookAsync(HeaderBookCreationRequest request);
        public Task<ApiResponse<HeaderBookResponse>> updateBookAsync(HeaderBookUpdateRequest request, string idBook, string idTheBook);
        public Task<ApiResponse<string>> deleteBookAsync(string idBook);
<<<<<<< HEAD
        public Task<string> generateNextIdBookAsync();

        public Task<BookResponse> findPost(string name_book);
=======
>>>>>>> 804cd4f59b533a1f03e4525db3d688cc55bd999f
    }
}
