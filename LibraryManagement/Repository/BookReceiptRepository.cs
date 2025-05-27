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
        public BookReceiptRepository(LibraryManagermentContext context)
        {
            _context = context;
        }

        // Hàm tạo Id sách
        public async Task<string> generateNextIdBookAsync()
        {
            var nextID = await _context.Books.OrderByDescending(id => id.IdBook).FirstOrDefaultAsync();

            int nextNumber = 1;

            if (nextID != null && nextID.IdBook.StartsWith("book")) // Kiểm tra có tổn tại sách không và ký tự đầu tiên là book
            {
                string numberPart = nextID.IdBook.Substring(4);
                if (int.TryParse(numberPart, out int parsed)) // Kiểm tra chuyển đổi từ string qua int
                {
                    nextNumber = parsed + 1;
                }
            }
            return $"book{nextNumber:D3}";
        }

        // Hàm tạo Id cuốn sách
        public async Task<string> generateNextIdTheBookAsync()
        {
            var nextID = await _context.TheBooks.OrderByDescending(id => id.IdTheBook).FirstOrDefaultAsync();

            int nextNumber = 1;

            if (nextID != null && nextID.IdTheBook.StartsWith("tb")) // Kiểm tra có tổn tại sách không và ký tự đầu tiên là book
            {
                string numberPart = nextID.IdTheBook.Substring(2);
                if (int.TryParse(numberPart, out int parsed)) // Kiểm tra chuyển đổi từ string qua int
                {
                    nextNumber = parsed + 1;
                }
            }
            return $"tb{nextNumber:D5}";
        }

        public async Task<ApiResponse<BooKReceiptResponse>> addBookReceiptAsync(BookReceiptRequest request)
        {
            var headerBook = await _context.HeaderBooks.FirstOrDefaultAsync(hb => hb.NameHeaderBook == request.headerBook.NameHeaderBook);
            // Tạo HeaderBook
            if (headerBook == null)
            {
                headerBook = new HeaderBook
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
                        var createBook = new BookWriting
                        {
                            IdHeaderBook = headerBook.IdHeaderBook,
                            IdAuthor = authorId
                        };
                        _context.BookWritings.Add(createBook); // Nạp dữ liệu vào bảng sáng tác
                    }
                }
            }
            else
            {
                headerBook.DescribeBook = request.headerBook.DescribeBook; // Luôn cập nhật Describe của Book
                _context.HeaderBooks.Update(headerBook);
                await _context.SaveChangesAsync();
            }

            // Tạo Book
            var book = new Book
            {
                IdBook = await generateNextIdBookAsync(),
                IdHeaderBook = headerBook.IdHeaderBook,
                Publisher = request.headerBook.bookCreateRequest.Publisher,
                ReprintYear = request.headerBook.bookCreateRequest.ReprintYear,
                ValueOfBook = request.headerBook.bookCreateRequest.ValueOfBook
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
                    UnitPrice = request.headerBook.bookCreateRequest.ValueOfBook
                };
                _context.DetailBookReceipts.Add(detailEntry);

                var firstId = await generateNextIdTheBookAsync();
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
                    UnitPrice = request.headerBook.bookCreateRequest.ValueOfBook
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

        public async Task<List<BookReceiptInformationOutput>> getAllReceiptHistory(string token)
        {
            var result = await _context.Readers
                 .Join(_context.BookReceipts,
                 reader => reader.IdReader,
                 receipt => receipt.IdReader,
                 (reader, receipt) => new { reader, receipt })
                 .Join(
                _context.DetailBookReceipts,
                combined => combined.receipt.IdBookReceipt,
                detail => detail.IdBookReceipt,
                (combined, detail) => new { combined.reader, combined.receipt, detail }
               )
                 .Join(_context.Books,
                 combined => combined.detail.IdBook,
                 book => book.IdBook,
                 (combined, book) => new
                 {
                     combined.reader,
                     combined.receipt,
                     combined.detail,
                     book
                 })
                 .Select(x => new BookReceiptInformationOutput
                 {
                     IdReader = x.reader.IdReader,
                     ReaderName = x.reader.NameReader,
                     receivedDate = x.receipt.ReceivedDate,
                     Quantity = x.detail.Quantity,
                     unitprice = x.detail.UnitPrice,
                     IdBook = x.book.IdBook,
                   

                 }).ToListAsync();
            return result;
                
        }
    }
}
