using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;


namespace LibraryManagement.Repository.InterFace
{
    public interface IBookService
    {
        public Task<ApiResponse<HeaderBookResponse>> addBookAsync(HeaderBookCreationRequest request);
        public Task<ApiResponse<HeaderBookResponse>> updateBookAsync(HeaderBookUpdateRequest request, string idBook, string idTheBook);
        public Task<ApiResponse<string>> deleteBookAsync(string idBook);

        public Task<BookResponse> findPost(string name_book);

        public Task<List<HeadbookAndComments>> getHeaderbookandCommentsByid(GetHeaderBookDtoInput dto);

        public  Task<List<HeadbookAndComments>> getAllHeaderbookandComments(string token);

        public Task<List<EvaluationDetails>> getBooksEvaluation(EvaluationDetailInput dto);

        public Task<bool> LikeHeaderBook(EvaluationDetailInput dto);

        public Task<List<HeadbookAndComments>> getLikedHeaderBook(string token);

        public Task<bool> DeleteEvaluation(DeleteEvaluationInput dto);


      
    } 
}
