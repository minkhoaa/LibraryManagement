using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;

namespace LibraryManagement.Repository.InterFace
{
    public interface ITypeBookRepository
    {
        public Task<ApiResponse<TypeBookResponse>> addTypeBookAsync(TypeBookRequest request);
        public Task<ApiResponse<TypeBookResponse>> updateTypeBookAsync(TypeBookRequest request, Guid idTypeBook);
        public Task<ApiResponse<string>> deleteTypeBook(Guid idTypeBook);

        public Task<List<TypeBookResponseAndBook>> getTypebookAndBooks();
    }
}
