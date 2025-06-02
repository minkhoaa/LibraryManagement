﻿using AutoMapper;
using LibraryManagement.Data;
using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;
using LibraryManagement.Models;
using LibraryManagement.Repository.InterFace;
using LibraryManagement.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace LibraryManagement.Repository
{
    public class BookService : IBookService
    {
        private readonly LibraryManagermentContext _context;
        private readonly IBookReceiptService _bookReceiptRepository;
        private readonly IUpLoadImageFileService _upLoadImageFileRepository;
        private readonly IParameterService _parameterRepository;
        private readonly IAuthenService _authen; 

        public BookService(LibraryManagermentContext context, 
                              IMapper mapper, 
                              IBookReceiptService bookReceiptRepository,
                              IUpLoadImageFileService upLoadImageFileRepository,
                              IParameterService parameterRepository, 
                              IAuthenService authen
            )
        {
            _context = context;
            _bookReceiptRepository = bookReceiptRepository;
            _upLoadImageFileRepository = upLoadImageFileRepository;
            _parameterRepository = parameterRepository;
            _authen = authen; 
        }

        // Hàm thêm mới đầu sách
        public async Task<ApiResponse<HeaderBookResponse>> addBookAsync(HeaderBookCreationRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                // Quy định khoảng cách năm xuất bản
                int publishBookGap = DateTime.Now.Year - request.bookCreateRequest.ReprintYear;
                int publishGap = await _parameterRepository.getValueAsync("PublishGap");
                if (publishBookGap > publishGap)
                {
                    return ApiResponse<HeaderBookResponse>.FailResponse($"Khoảng cách năm xuất bản phải nhỏ hơn {publishGap}", 400);
                }


                // Tạo đầu sách
                string imageUrl = null;

                var typeBookTask = _context.TypeBooks.AsNoTracking()
             .FirstOrDefaultAsync(typebook => typebook.IdTypeBook == request.IdTypeBook);
                var headerBookTask = _context.HeaderBooks
                    .FirstOrDefaultAsync(hb => hb.NameHeaderBook == request.NameHeaderBook);

                await Task.WhenAll(typeBookTask, headerBookTask);
                var typeBook = await typeBookTask;
                var headerBook = await headerBookTask;
                if (typeBook == null)
                {
                    return ApiResponse<HeaderBookResponse>.FailResponse("Không tìm thấy loại sách phù hợp", 404);
                }

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

                    if (request.Authors != null && request.Authors.Any()) // Duyệt qua danh sách tác giả
                    {
                        foreach (var authorId in request.Authors)
                        {
                            var bookWriting = new BookWriting
                            {
                                IdHeaderBook = headerBook.IdHeaderBook,
                                IdAuthor = authorId
                            };
                            _context.BookWritings.Add(bookWriting); // Nạp dữ liệu vào bảng sáng tác
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
                // Chuỗi url ảnh từ cloudinary
                if (request.BookImage != null)
                {
                    imageUrl = await _upLoadImageFileRepository.UploadImageAsync(request.BookImage);
                }
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
                // Lưu ảnh sách vào bảng image nếu có
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    var image = new Image
                    {
                        IdBook = book.IdBook,
                        Url = imageUrl,
                    };
                    _context.Images.Add(image);
                    await _context.SaveChangesAsync();
                }

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
                    TypeBook = new TypeBookResponse
                    {
                        IdTypeBook = typeBook.IdTypeBook,
                        NameTypeBook = typeBook.NameTypeBook
                    },
                    NameHeaderBook = headerBook.NameHeaderBook,
                    DescribeBook = headerBook.DescribeBook,
                    Authors = request.Authors,

                    bookResponse = new BookResponse
                    {
                        IdBook = book.IdBook,
                        Publisher = book.Publisher,
                        ReprintYear = book.ReprintYear,
                        ValueOfBook = book.ValueOfBook,
                    },
                    thebookReponse = new TheBookResponse
                    {
                        IdTheBook = theBook.IdTheBook,
                        Status = theBook.Status
                    }
                };
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return ApiResponse<HeaderBookResponse>.SuccessResponse("Tạo sách thành công", 201, response);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
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

                var createBooks = await _context.BookWritings // Xóa sáng tác của tác giả
                    .Where(cb => cb.IdHeaderBook == book.IdHeaderBook)
                    .ToListAsync();

                _context.BookWritings.RemoveRange(createBooks);
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
            // Quy định khoảng cách năm xuất bản
            int publishBookGap = DateTime.Now.Year - request.bookUpdateRequest.ReprintYear;
            int publishGap = await _parameterRepository.getValueAsync("PublishGap");
            if (publishBookGap > publishGap)
            {
                return ApiResponse<HeaderBookResponse>.FailResponse($"Khoảng cách năm xuất bản phải nhỏ hơn {publishGap}", 400);
            }

            var checkBook = await _context.Books.FirstOrDefaultAsync(b => b.IdBook == idBook.ToString());
            if (checkBook == null)
            {
                return ApiResponse<HeaderBookResponse>.FailResponse("Không tìm thấy sách", 404);
            }

            var typeBook = await _context.TypeBooks.FirstOrDefaultAsync(typebook => typebook.IdTypeBook == request.IdTypeBook);
            if (typeBook == null)
            {
                return ApiResponse<HeaderBookResponse>.FailResponse("Không tìm thấy loại sách phù hợp", 404);
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
                await _context.HeaderBooks.AddAsync(headerBook);
                await _context.SaveChangesAsync();

                if (request.IdAuthors != null && request.IdAuthors.Any()) // Duyệt qua danh sách tác giả
                {
                    foreach (var authorId in request.IdAuthors)
                    {
                        var createBook = new BookWriting
                        {
                            IdHeaderBook = headerBook.IdHeaderBook,
                            IdAuthor = authorId
                        };
                        await _context.BookWritings.AddAsync(createBook); // Nạp dữ liệu vào bảng sáng tác
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
                TypeBook = new TypeBookResponse
                {
                    IdTypeBook = typeBook.IdTypeBook,
                    NameTypeBook = typeBook.NameTypeBook
                },
                NameHeaderBook = headerBook.NameHeaderBook,
                DescribeBook = headerBook.DescribeBook,
                Authors = request.IdAuthors,

                bookResponse = new BookResponse
                {
                    IdBook = checkBook.IdBook,
                    Publisher = checkBook.Publisher,
                    ReprintYear = checkBook.ReprintYear,
                    ValueOfBook = checkBook.ValueOfBook,
                },
                thebookReponse = new TheBookResponse
                {
                    IdTheBook = checkTheBook.IdTheBook,
                    Status = checkTheBook.Status
                }
            };
            return ApiResponse<HeaderBookResponse>.SuccessResponse("Cập nhật sách thành công", 201, response);
        }


        //public Task<BookResponse> findPost(string name_book)
        //{
        //    throw new NotImplementedException();
        //}
        //public async Task<List<HeadbookAndComments>> getHeaderbookandCommentsByid(GetHeaderBookDtoInput dto)
        //{
        //    var user = await _authen.AuthenticationAsync(dto.token);
        //    if (user == null) return null!;
          
        //    var bookResult = await _context.HeaderBooks
        //        .AsNoTracking()
        //        .Where(hd => hd.NameHeaderBook == dto.name_headerbook)
        //        //.Include(hd => hd.Evaluates)
        //        .Select(hb => new HeadbookAndComments
        //        {
        //            idHeaderBook = hb.IdHeaderBook.ToString(),
        //            nameHeaderBook = hb.NameHeaderBook,
        //            describe = hb.DescribeBook,
        //            isLiked = _context.LikedHeaderBooks
        //            .Any(x => x.IdReader == user.IdReader && x.IdBook == hb.IdHeaderBook),
        //            Evaluations = hb.Evaluates
        //            .Select(e => new EvaluationDetails
        //            {
        //                IdEvaluation = e.IdEvaluate,
        //                IdReader = e.IdReader,
        //                Comment = e.EvaComment,
        //                Rating = e.EvaStar,
        //                Create_Date = e.CreateDate
        //            })
        //        .OrderByDescending(e => e.Create_Date) // Sắp xếp theo ngày tạo
        //        .ToList()
        //        }).ToListAsync();

        //    return bookResult ;
        //}

        //public async Task<List<HeadbookAndComments>> getAllHeaderbookandComments(string token)
        //{
        //    var user = await _authen.AuthenticationAsync(token);
        //    if (user == null) return null!;
        //    var bookResult = await _context.HeaderBooks
        //         .AsNoTracking()
        //         .Include(hd => hd.Evaluates)
        //         .Include(bw=>bw.bookWritings)
        //            .ThenInclude(at => at.Author)
        //         .Select(hb => new HeadbookAndComments
        //         {
        //             idHeaderBook = hb.IdHeaderBook.ToString(),
        //             nameHeaderBook = hb.NameHeaderBook,
        //             describe = hb.DescribeBook,
        //             image = hb.ImageBook,
        //             isLiked = _context.LikedHeaderBooks
        //             .Any(x => x.IdReader == user.IdReader && x.IdBook == hb.IdHeaderBook),
        //             Evaluations = hb.Evaluates
        //             .Select(e => new EvaluationDetails
        //             {
        //                 IdEvaluation = e.IdEvaluate,
        //                 IdReader = e.IdReader,
        //                 Comment = e.EvaComment,
        //                 Rating = e.EvaStar,
        //                 Create_Date = e.CreateDate
        //             })
            
        //         .OrderByDescending(e => e.Create_Date) // Sắp xếp theo ngày tạo
        //         .ToList(),
        //           Authors = hb.bookWritings
        //                     .Select(bw=>new AuthorResponse
        //                     {
        //                         IdAuthor = bw.IdAuthor,
        //                         NameAuthor = bw.Author.NameAuthor ,
        //                         Biography = bw.Author.Biography,
        //                         Nationality = bw.Author.Nationality,
        //                         IdTypeBook = bw.Author.IdTypeBook
        //                     })
        //                     .ToList(),
        //         }).ToListAsync();

        //    return bookResult;
        //}
        public async Task<List<EvaluationDetails>> getBooksEvaluation(EvaluationDetailInput dto)
        {
            var user = await _authen.AuthenticationAsync(dto.token);
            var role = await _authen.UserRoleCheck(dto.token);
            if (role != 0) return null!;
            if (user == null) return null!;
            var result = await _context.Evaluates.AsNoTracking().Where(x => x.IdBook == dto.IdBook).Take(50).Select(a => new EvaluationDetails
            {
                IdEvaluation = a.IdEvaluate,
                IdReader = a.IdReader,
                Comment = a.EvaComment,
                Rating = a.EvaStar,
                Create_Date = a.CreateDate
            }).ToListAsync();
            return result;
        }

        public async Task<bool> LikeBook(EvaluationDetailInput dto)
        {
            var user = await _authen.AuthenticationAsync(dto.token);
            if (user == null) return false;

            var likedBook = await _context.FavoriteBooks.Where(x => x.IdReader == user.IdReader && x.IdBook == dto.IdBook).FirstOrDefaultAsync();
            if (likedBook != null)
            {
                _context.Remove(likedBook);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                var likeBook = new FavoriteBook
                {
                    IdBook = dto.IdBook,
                    IdReader = user.IdReader,
                    createDay = DateTime.UtcNow
                };
                await _context.LikedHeaderBooks.AddAsync(likeBook);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<List<BooksAndComments>> getAllBooksInDetail(string token)
        {
            var user = await _authen.AuthenticationAsync(token);
            if (user == null) return null!;

            var result = await _context.Books
                .AsNoTracking()
                .Include(a => a.HeaderBook)
                .ThenInclude(x => x.bookWritings)
                .ThenInclude(c => c.Author)
                .Include(a => a.Evaluates)
                .Include(a=>a.images)
                .Select(x => new BooksAndComments
                {
                    idBook = x.IdBook,
                    nameBook = x.HeaderBook.NameHeaderBook,
                    describe = x.HeaderBook.DescribeBook,
                    isLiked = _context.FavoriteBooks.Any(k => k.IdReader == user.IdReader && k.IdBook == x.IdBook),
                    Evaluations = _context.Evaluates
                                .Where(a => a.IdBook == x.IdBook)
                                .Select(a => new EvaluationDetails
                                {
                                    IdEvaluation = a.IdEvaluate,
                                    IdReader = a.IdReader,
                                    Comment = a.EvaComment,
                                    Rating = a.EvaStar,
                                    Create_Date = a.CreateDate
                                }).ToList(),
                    Authors = x.HeaderBook.bookWritings
                              .Select(a => new AuthorResponse
                              {
                                  IdAuthor = a.IdAuthor,
                                  NameAuthor = a.Author.NameAuthor,
                                  Biography = a.Author.Biography,
                                  IdTypeBook = a.Author.IdTypeBook,
                                  Nationality = a.Author.Nationality
                              }).ToList(),
                    image = x.images.FirstOrDefault() != null ? x.images.FirstOrDefault()!.Url : string.Empty

                }).ToListAsync();
            return result;
        }
   
        public async Task<bool> DeleteEvaluation(DeleteEvaluationInput dto)
        {
            var user = await _authen.AuthenticationAsync(dto.token);
            if (user == null) return false;
            var evaluation = await _context.Evaluates.FirstOrDefaultAsync(x => x.IdEvaluate == dto.IdValuation);
            if (evaluation == null) return false; 
            _context.Evaluates.Remove(evaluation);
            await _context.SaveChangesAsync();
            return true; 

        }

        public async Task<List<GetHeaderbookResponse>> GetAllHeaderBooks(string token)
        {
            var reader = await _authen.AuthenticationAsync(token);
            if (reader == null) return null!;

            var result = await _context.HeaderBooks
                .AsNoTracking()
                .Select(a => new GetHeaderbookResponse
                {
                    IdHeaderbook = a.IdHeaderBook,
                    NameBook = a.NameHeaderBook,
                    Describe = a.DescribeBook
                }).ToListAsync();

            return result; 
        }

    }
}
