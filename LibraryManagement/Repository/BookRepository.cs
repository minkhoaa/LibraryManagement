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
        private readonly IBookReceiptRepository _bookReceiptRepository;

        public BookRepository(LibraryManagermentContext context, 
                              IMapper mapper, 
                              IBookReceiptRepository bookReceiptRepository)
        {
            _context = context;
            _bookReceiptRepository = bookReceiptRepository;
        }

        // Hàm thêm mới đầu sách
        public async Task<ApiResponse<HeaderBookResponse>> addBookAsync(HeaderBookCreationRequest request)
        {
            var headerBook = await _context.HeaderBooks.FirstOrDefaultAsync(hb => hb.NameHeaderBook == request.NameHeaderBook);
            // Tạo đầu sách
            if (headerBook == null)
            {
                headerBook = new HeaderBook
                {
                    IdTypeBook = request.IdTypeBook,
                    NameHeaderBook = request.NameHeaderBook,
                    DescribeBook = request.DescribeBook,
                };
                _context.HeaderBooks.Add(headerBook);
                await _context.SaveChangesAsync();

                if (request.IdAuthors != null && request.IdAuthors.Any()) // Duyệt qua danh sách tác giả
                {
                    foreach (var authorId in request.IdAuthors)
                    {
                        var createBook = new CreateBook
                        {
                            IdHeaderBook = headerBook.IdHeaderBook,
                            IdAuthor = authorId
                        };
                        _context.CreateBooks.Add(createBook); // Nạp dữ liệu vào bảng sáng tác
                    }
                }
            }
            else
            {
                headerBook.DescribeBook = request.DescribeBook; // Luôn cập nhật Describe của Book
                _context.HeaderBooks.Update(headerBook);
                await _context.SaveChangesAsync();
            }

            // Tạo sách
            var book = new Book
            {
                IdBook = await _bookReceiptRepository.generateNextIdBookAsync(),
                IdHeaderBook = headerBook.IdHeaderBook,
                Publisher = request.bookCreateRequest.Publisher,
                ReprintYear = request.bookCreateRequest.ReprintYear,
                ValueOfBook = request.bookCreateRequest.ValueOfBook
            };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
      
            // Tạo cuốn sách
            var theBook = new TheBook
            {
                IdTheBook = await _bookReceiptRepository.generateNextIdTheBookAsync(),
                IdBook = book.IdBook,
                Status = "Có sẵn"
            };
            _context.TheBooks.Add(theBook);
            await _context.SaveChangesAsync();

            // Ánh xạ response
            var response = new HeaderBookResponse
            {
                IdTypeBook = headerBook.IdTypeBook,
                NameHeaderBook = headerBook.NameHeaderBook,
                DescribeBook = headerBook.DescribeBook,
                IdAuthors = request.IdAuthors,

                bookResponse = new BookResponse
                {
                    Publisher = book.Publisher,
                    ReprintYear = book.ReprintYear,
                    ValueOfBook = book.ValueOfBook,
                },
                thebookReponse = new TheBookResponse
                {
                    Status = theBook.Status
                }
            };
            return ApiResponse<HeaderBookResponse>.SuccessResponse("Tạo sách thành công", 201, response);
        }

        // Hàm xóa sách
        public async Task<ApiResponse<string>> deleteBookAsync(string idBook)
        {
            // Tìm Book
            var book = await _context.Books.FirstOrDefaultAsync(b => b.IdBook == idBook.ToString());
            if (book == null)
            {
                return ApiResponse<string>.FailResponse("Không tìm thấy sách", 404);
            }

            // Tìm các cuốn sách thuộc Book
            var theBooks = await _context.TheBooks
                .Where(tb => tb.IdBook == idBook.ToString())
                .ToListAsync();

            _context.TheBooks.RemoveRange(theBooks);
            _context.Books.Remove(book);

            // Kiểm tra có còn bản sách nào cho đầu sách này không
            int remainingBooks = await _context.Books
                .CountAsync(b => b.IdHeaderBook == book.IdHeaderBook && b.IdBook != idBook.ToString());

            if (remainingBooks == 0)
            {
                
                var headerBook = await _context.HeaderBooks // Xóa đầu sách
                    .FirstOrDefaultAsync(hb => hb.IdHeaderBook == book.IdHeaderBook);

                var createBooks = await _context.CreateBooks // Xóa sáng tác của tác giả
                    .Where(cb => cb.IdHeaderBook == book.IdHeaderBook)
                    .ToListAsync();

                _context.CreateBooks.RemoveRange(createBooks);
                if (headerBook != null)
                {
                    _context.HeaderBooks.Remove(headerBook);
                }
            }
            await _context.SaveChangesAsync();
            return ApiResponse<string>.SuccessResponse("Xóa sách thành công", 200, null);
        }

        // Hàm cập nhật sách
        public async Task<ApiResponse<HeaderBookResponse>> updateBookAsync(HeaderBookUpdateRequest request, 
                                                                           string idBook, string idTheBook)
        {
            var checkBook = await _context.Books.FirstOrDefaultAsync(b => b.IdBook == idBook.ToString());
            if (checkBook == null)
            {
                return ApiResponse<HeaderBookResponse>.FailResponse("Không tìm thấy sách", 404);
            }

            var headerBook = await _context.HeaderBooks.FirstOrDefaultAsync(hb => hb.NameHeaderBook == request.NameHeaderBook);
            // Tạo đầu sách
            if (headerBook == null)
            {
                headerBook = new HeaderBook
                {
                    IdTypeBook = request.IdTypeBook,
                    NameHeaderBook = request.NameHeaderBook,
                    DescribeBook = request.DescribeBook,
                };
                _context.HeaderBooks.Add(headerBook);
                await _context.SaveChangesAsync();

                if (request.IdAuthors != null && request.IdAuthors.Any()) // Duyệt qua danh sách tác giả
                {
                    foreach (var authorId in request.IdAuthors)
                    {
                        var createBook = new CreateBook
                        {
                            IdHeaderBook = headerBook.IdHeaderBook,
                            IdAuthor = authorId
                        };
                        _context.CreateBooks.Add(createBook); // Nạp dữ liệu vào bảng sáng tác
                    }
                }
            }else
            {
                headerBook.DescribeBook = request.DescribeBook; // Luôn cập nhật Describe của Book
                _context.HeaderBooks.Update(headerBook);
                await _context.SaveChangesAsync();
            }

            // Cập nhật thông tin sách
            checkBook.Publisher = request.bookUpdateRequest.Publisher;
            checkBook.ReprintYear = request.bookUpdateRequest.ReprintYear;
            checkBook.ValueOfBook = request.bookUpdateRequest.ValueOfBook;
            checkBook.IdHeaderBook = headerBook.IdHeaderBook;

            var checkTheBook = await _context.TheBooks.FirstOrDefaultAsync(tb => tb.IdTheBook == idTheBook.ToString());
            if (checkTheBook == null)
            {
                return ApiResponse<HeaderBookResponse>.FailResponse("Không tìm thấy cuốn sách", 404);
            }
            // Cập nhật thông tin cuốn sách
            checkTheBook.Status = request.theBookUpdateRequest.Status;

            // Ánh xạ response
            var response = new HeaderBookResponse
            {
                IdTypeBook = headerBook.IdTypeBook,
                NameHeaderBook = headerBook.NameHeaderBook,
                DescribeBook = headerBook.DescribeBook,
                IdAuthors = request.IdAuthors,

                bookResponse = new BookResponse
                {
                    Publisher = checkBook.Publisher,
                    ReprintYear = checkBook.ReprintYear,
                    ValueOfBook = checkBook.ValueOfBook,
                },
                thebookReponse = new TheBookResponse
                {
                    Status = checkTheBook.Status
                }
            };
            return ApiResponse<HeaderBookResponse>.SuccessResponse("Cập nhật sách thành công", 201, response);
        }

        public Task<BookResponse> findPost(string name_book)
        {
            throw new NotImplementedException();
        }
    }
}
