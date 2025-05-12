using AutoMapper;
using LibraryManagement.Data;
using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;
using LibraryManagement.Models;
using LibraryManagement.Repository.InterFace;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryManagermentContext _context;
        private readonly IMapper _mapper;

        public BookRepository(LibraryManagermentContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Hàm lấy danh sách sách
        public async Task<List<BookResponse>> getListBook()
        {
            var listBook = await _context.Books.ToListAsync();
            return _mapper.Map<List<BookResponse>>(listBook);
        }

        // Hàm thêm mới sách
        public async Task<ApiResponse<BookResponse>> addBookAsync(BookRequest request)
        {
            var newBook = _mapper.Map<Book>(request);
            newBook.IdBook = await generateNextIdBookAsync();
            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();

            var bookResponse = _mapper.Map<BookResponse>(newBook);
            return ApiResponse<BookResponse>.SuccessResponse("Thêm sách thành công", 201, bookResponse);
        }

        // Hàm xóa sách
        public async Task<ApiResponse<string>> deleteBookAsync(string idBook)
        {
            var deleteBook = await _context.Books.FirstOrDefaultAsync(book => book.IdBook == idBook);
            if (deleteBook == null)
            {
                return ApiResponse<string>.FailResponse("Không tìm thấy sách", 404);
            }
            _context.Books.Remove(deleteBook);
            await _context.SaveChangesAsync();
            return ApiResponse<string>.SuccessResponse("Đã xóa sách", 200, "");
        }

        // Hàm sửa sách
        public async Task<ApiResponse<BookResponse>> updateBookAsync(BookRequest request, string idBook)
        {
            var updateBook = await _context.Books.FirstOrDefaultAsync(book => book.IdBook == idBook);
            if (updateBook == null)
            {
                return ApiResponse<BookResponse>.FailResponse("Không tìm thấy sách", 404);
            }
            _mapper.Map(request, updateBook);

            _context.Books.Update(updateBook);
            await _context.SaveChangesAsync();
            var bookResponse = _mapper.Map<BookResponse>(updateBook);
            return ApiResponse<BookResponse>.SuccessResponse("Thay đổi thông tin sách thành công", 200, bookResponse);
        }

        // Hàm tạo Id sách
        public async Task<string> generateNextIdBookAsync()
        {
            var nextID = await _context.Books.OrderByDescending(id => id.IdBook).FirstOrDefaultAsync();

            int nextNumber = 1;

            if (nextID != null && nextID.IdBook.StartsWith("book")) // Kiểm tra có tổn tại sách không và ký tự đầu tiên là book
            {
                string numberPart = nextID.IdBook.Substring(4);
                if(int.TryParse(numberPart, out int parsed)) // Kiểm tra chuyển đổi từ string qua int
                {
                    nextNumber = parsed + 1;
                }    
            }
            return $"book{nextNumber:D3}";
        }

        public Task<BookResponse> findPost(string name_book)
        {
            throw new NotImplementedException();
        }
    }
}
