using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;

namespace LibraryManagement.Repository.InterFace
{
    public interface IAuthorRepository
    {
        public Task<List<AuthorResponse>> getListAuthor();
        public Task<ApiResponse<AuthorResponse>> addAuthorAsync(AuthorCreationRequest request);
        public Task<ApiResponse<AuthorResponse>> updateAuthorAsync(AuthorUpdateRequest request, Guid idAuthor);
        public Task<ApiResponse<string>> deleteAuthorAsync(Guid idAuthor);
    }
}
