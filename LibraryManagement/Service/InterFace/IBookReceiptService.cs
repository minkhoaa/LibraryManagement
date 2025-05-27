using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;

namespace LibraryManagement.Repository.InterFace
{
    public interface IBookReceiptService
    {
        Task<ApiResponse<BooKReceiptResponse>> addBookReceiptAsync(BookReceiptRequest request);
        Task<ApiResponse<BooKReceiptResponse>> updateBookReceiptAsync(BookReceiptRequest request, Guid idBookReipt);

        Task<ApiResponse<string>> deleteBookReceiptAsync(Guid idBookReipt);
        public Task<string> generateNextIdBookAsync();
        public Task<string> generateNextIdTheBookAsync();

        public Task<List<BookReceiptInformationOutput>> getAllReceiptHistory(string token);
     }
}
