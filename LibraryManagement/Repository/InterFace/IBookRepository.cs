using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;

using LibraryManagement.Models;
using Microsoft.Extensions.Primitives;


namespace LibraryManagement.Repository.InterFace
{
    public interface IBookRepository
    {
        public Task<ApiResponse<HeaderBookResponse>> addBookAsync(HeaderBookCreationRequest request);
        public Task<ApiResponse<HeaderBookResponse>> updateBookAsync(HeaderBookUpdateRequest request, string idBook, string idTheBook);
        public Task<ApiResponse<string>> deleteBookAsync(string idBook);

        public Task<string> generateNextIdBookAsync();

        public Task<BookResponse> findPost(string name_book);

        public Task<List<HeadbookAndComments>> getHeaderbookandComments(string name_headerbook);

        public Task<List<EvaluationDetails>> getBooksEvaluation(EvaluationDetailInput dto);

        public Task<bool> LikeHeaderBook(EvaluationDetailInput dto);
    } 
}
