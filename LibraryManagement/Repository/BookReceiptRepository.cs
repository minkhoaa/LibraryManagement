using LibraryManagement.Data;
using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;
using LibraryManagement.Models;
using LibraryManagement.Repository.InterFace;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repository
{
    public class BookReceiptRepository : IBookReceiptRepository
    {
        private readonly LibraryManagermentContext _context;
        private readonly IBookRepository _bookRepository;
        public readonly ITheBookRepository _theBookRepository;
        public BookReceiptRepository(LibraryManagermentContext context, IBookRepository bookRepository, ITheBookRepository theBookRepository)
        {
            _context = context;
            _bookRepository = bookRepository;
            _theBookRepository = theBookRepository;
        }

        public async Task<ApiResponse<BooKReceiptResponse>> addBookReceiptAsync(BookReceiptRequest request)
        {
            // Tạo HeaderBook
            var headerBook = new HeaderBook
            {
                IdTypeBook = request.headerBook.IdTypeBook,
                NameHeaderBook = request.headerBook.NameHeaderBook,
                DescribeBook = request.headerBook.DescribeBook,
            };
            _context.HeaderBooks.Add(headerBook);
            await _context.SaveChangesAsync();

            if (request.headerBook.IdAuthors != null && request.headerBook.IdAuthors.Any()) // Duyệt qua danh sách tác giả
            {
                foreach (var authorId in request.headerBook.IdAuthors)
                {
                    var createBook = new CreateBook
                    {
                        IdHeaderBook = headerBook.IdHeaderBook,
                        IdAuthor = authorId
                    };
                    _context.CreateBooks.Add(createBook); // Nạp dữ liệu vào bảng sáng tác
                }
            }

            // Tạo Book
            var book = new Book
            {
                IdBook = await _bookRepository.generateNextIdBookAsync(),
                IdHeaderBook = headerBook.IdHeaderBook,
                Publisher = request.book.Publisher,
                ReprintYear = request.book.ReprintYear,
                ValueOfBook = request.book.ValueOfBook
            };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();


            // Lưu dữ liệu xuống phiếu nhập sách
            var bookReceipt = new BookReceipt
            {
                IdReader = request.IdReader,
            };

            _context.BookReceipts.Add(bookReceipt);

            var detailResponses = new List<DetailBookReceiptResponse>(); // Danh sách chi tiết trả về

            foreach (var detail in request.listDetailsRequest)
            {
                // Lưu dữ liệu xuống chi tiết phiếu nhập sách
                var detailEntry = new DetailBookReceipt
                {
                    IdBookReceipt = bookReceipt.IdBookReceipt,
                    IdBook = book.IdBook,
                    Quantity = detail.Quantity,
                    UnitPrice = request.book.ValueOfBook
                };
                _context.DetailBookReceipts.Add(detailEntry);

                var firstId = await _theBookRepository.generateNextIdTheBookAsync();
                var nextID = int.Parse(firstId.Substring(2));
                for (int i = 0; i < detail.Quantity; i++) // Tạo THEBOOK thông qua số lượng từ BOOK
                {
                    var theBook = new TheBook
                    {
                        IdTheBook = $"tb{(nextID + i):D5}",
                        IdBook = book.IdBook,
                        Status = "Có sẵn" 
                    };
                    _context.TheBooks.Add(theBook);
                }
                detailResponses.Add(new DetailBookReceiptResponse // Lưu vào response để trả về danh sách
                {
                    Quantity = detail.Quantity,
                    UnitPrice = request.book.ValueOfBook
                });
            }

            await _context.SaveChangesAsync();

            var response = new BooKReceiptResponse
            {
                ReceivedDate = bookReceipt.ReceivedDate,
                listDetailsResponse = detailResponses
            };
            return ApiResponse<BooKReceiptResponse>.SuccessResponse("Tạo phiếu nhập sách thành công", 201, response);
        }

        // Xóa phiếu nhập sách
        public async Task<ApiResponse<string>> deleteBookReceiptAsync(Guid idBookReipt)
        {
            // Lấy BookReceipt
            var bookReceipt = await _context.BookReceipts.FirstOrDefaultAsync(br => br.IdBookReceipt == idBookReipt);
            if (bookReceipt == null)
                return ApiResponse<string>.FailResponse("Phiếu nhập sách không tồn tại", 404);

            var detailReceipts = await _context.DetailBookReceipts
                .Where(d => d.IdBookReceipt == bookReceipt.IdBookReceipt)
                .ToListAsync();

            // Lấy danh sách Book
            var bookIds = detailReceipts.Select(d => d.IdBook).Distinct().ToList();
            var books = await _context.Books.
                Where(b => bookIds.Contains(b.IdBook))
                .ToListAsync();

            // Lấy danh sách HeaderBook
            var headerBookIds = books.Select(b => b.IdHeaderBook).Distinct().ToList();
            var usedHeaderBookIds = await _context.Books
            .Where(b => !bookIds.Contains(b.IdBook)) // những Book không thuộc phiếu nhập này
            .Select(b => b.IdHeaderBook)
            .Distinct()
            .ToListAsync();

            var headerBooksToDelete = await _context.HeaderBooks
                .Where(h => headerBookIds.Contains(h.IdHeaderBook) && !usedHeaderBookIds.Contains(h.IdHeaderBook))
                .ToListAsync();

            _context.BookReceipts.Remove(bookReceipt);
            _context.HeaderBooks.RemoveRange(headerBooksToDelete);

             await _context.SaveChangesAsync();
             return ApiResponse<string>.SuccessResponse("Xóa phiếu nhập sách thành công", 200, "");
        }

        // Sửa phiếu nhập sách
        public async Task<ApiResponse<BooKReceiptResponse>> updateBookReceiptAsync(BookReceiptRequest request, Guid idBookReipt)
        {
            throw new NotImplementedException();
        }
    }
}
