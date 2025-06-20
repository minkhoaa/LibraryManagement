﻿using AutoMapper;
using LibraryManagement.Data;
using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;
using LibraryManagement.Models;
using LibraryManagement.Repository.InterFace;
using LibraryManagement.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace LibraryManagement.Repository
{
    public class AuthorService : IAuthorService
    {
        private readonly LibraryManagermentContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthenService _account;
        private readonly IUpLoadImageFileService _upLoadImageFileService;

        public AuthorService(LibraryManagermentContext context, IMapper mapper, 
                                                                IAuthenService account, 
                                                                IUpLoadImageFileService upLoadImageFileService)
        {
            _context = context;
            _mapper = mapper;
            _account = account;    
            _upLoadImageFileService = upLoadImageFileService;
        }

        // Lấy danh sách tác giả
        public async Task<List<AuthorResponse>> getListAuthor()
        {

            var authorResponse = await _context.Authors.AsNoTracking().Select(x => new AuthorResponse
            {
                IdAuthor = x.IdAuthor,
                IdTypeBook = new TypeBookResponse { IdTypeBook = x.IdTypeBook, NameTypeBook = x.TypeBook.NameTypeBook },
                NameAuthor = x.NameAuthor,
                Nationality = x.Nationality,
                Biography = x.Biography,
                UrlAvatar = x.Images.Where(x => x.IdAuthor == x.IdAuthor).Select(x => x.Url).FirstOrDefault()
            }).ToListAsync();
            return authorResponse;
        }

        // Thêm tác giả
        public async Task<ApiResponse<AuthorResponse>> addAuthorAsync(AuthorRequest request)
        {
            var newAuthor = _mapper.Map<Author>(request);

            // Chuỗi url ảnh từ cloudinary
            string imageUrl = null!;
            if (request.AvatarImage != null)
            {
                imageUrl = await _upLoadImageFileService.UploadImageAsync(request.AvatarImage);
            }
            await _context.Authors.AddAsync(newAuthor);
      
            await _context.SaveChangesAsync();

            // Lưu avatar vào bảng image nếu có
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var image = new Image
                {
                    IdAuthor = newAuthor.IdAuthor,
                    Url = imageUrl,
                };
                await _context.Images.AddAsync(image);
                await _context.SaveChangesAsync();
            }

            var authorResponse = _mapper.Map<AuthorResponse>(newAuthor);

            var typeBook = await _context.TypeBooks.FindAsync(newAuthor.IdTypeBook);
            authorResponse.IdTypeBook = new TypeBookResponse
            {
                IdTypeBook = newAuthor.IdTypeBook,
                NameTypeBook = typeBook!.NameTypeBook,
            };
            authorResponse.UrlAvatar = imageUrl;
            return ApiResponse<AuthorResponse>.SuccessResponse("Thêm tác giả thành công", 201, authorResponse);
        }

        // Xóa tác giả
        public async Task<ApiResponse<string>> deleteAuthorAsync(Guid idAuthor)
        {
            var deleteAuthor = await _context.Authors.FirstOrDefaultAsync(author => author.IdAuthor == idAuthor);
            if (deleteAuthor == null)
            {
                return ApiResponse<string>.FailResponse("Không tìm thấy tác giả", 404);
            }
            _context.Authors.Remove(deleteAuthor);
            await _context.SaveChangesAsync();
            return ApiResponse<string>.SuccessResponse("Đã xóa tác giả", 200, "");
        }

        // Sửa tác giả
        public async Task<ApiResponse<AuthorResponse>> updateAuthorAsync(AuthorUpdateRequest request, Guid idAuthor)
        {
            var updateAuthor = await _context.Authors.FirstOrDefaultAsync(author => author.IdAuthor == idAuthor);
            if (updateAuthor == null)
            {
                return ApiResponse<AuthorResponse>.FailResponse("Không tìm thấy tác giả", 404);
            }

            // Chỉ cập nhật khi có dữ liệu truyền lên
            if (request.IdTypeBook.HasValue && request.IdTypeBook != Guid.Empty)
                updateAuthor.IdTypeBook = request.IdTypeBook.Value;

            if (!string.IsNullOrEmpty(request.NameAuthor))
                updateAuthor.NameAuthor = request.NameAuthor;

            if (!string.IsNullOrEmpty(request.Nationality))
                updateAuthor.Nationality = request.Nationality;

            if (!string.IsNullOrEmpty(request.Biography))
                updateAuthor.Biography = request.Biography;

            string imageUrl = null;
            if (request.AvatarImage != null)
            {
                imageUrl = await _upLoadImageFileService.UploadImageAsync(request.AvatarImage);
            }

            await _context.SaveChangesAsync();

            // Cập nhật hoặc thêm mới ảnh nếu có ảnh mới
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var existingAvatar = await _context.Images.FirstOrDefaultAsync(av => av.IdAuthor == updateAuthor.IdAuthor);
                if (existingAvatar != null)
                {
                    existingAvatar.Url = imageUrl;
                    _context.Images.Update(existingAvatar);
                }
                else
                {
                    var image = new Image
                    {
                        IdAuthor = updateAuthor.IdAuthor,
                        Url = imageUrl,
                    };
                    _context.Images.Add(image);
                }

                await _context.SaveChangesAsync(); // lưu lại ảnh
            }

            var authorResponse = _mapper.Map<AuthorResponse>(updateAuthor);

            var typeBook = await _context.TypeBooks.FindAsync(updateAuthor.IdTypeBook);
            authorResponse.IdTypeBook = new TypeBookResponse
            {
                IdTypeBook = updateAuthor.IdTypeBook,
                NameTypeBook = typeBook?.NameTypeBook ?? ""
            };
            authorResponse.UrlAvatar = imageUrl ?? (await _context.Images
                .Where(i => i.IdAuthor == updateAuthor.IdAuthor)
                .Select(i => i.Url)
                .FirstOrDefaultAsync());

            return ApiResponse<AuthorResponse>.SuccessResponse("Thay đổi thông tin tác giả thành công", 200, authorResponse);
        }

        public async Task<List<AuthorResponse>> findAuthor(FindAuthorInputDto dto)
        {
            var authors = await _context.Authors.AsNoTracking().Where(x => x.NameAuthor.ToLower().Contains(dto.nameAuthor))
                .Select(a=>new AuthorResponse
                {

                    IdAuthor = a.IdAuthor,
                    NameAuthor = a.NameAuthor,
                    Biography = a.Biography,
                    IdTypeBook = new TypeBookResponse
                    {
                        IdTypeBook = a.TypeBook.IdTypeBook,
                        NameTypeBook = a.TypeBook.NameTypeBook
                    },
                    Nationality = a.Nationality
                }).ToListAsync();
            return authors;
        }

        public async Task<GetAuthorByIdResponse> GetAuthorById(Guid idauthor)
        {
            var authors = await _context.Authors.AsNoTracking().Where(x => x.IdAuthor == idauthor)
               .Select( a => new GetAuthorByIdResponse
               {

                   IdAuthor = a.IdAuthor,
                   NameAuthor = a.NameAuthor,
                   Biography = a.Biography,
                   IdTypeBook = new TypeBookResponse
                   {
                       IdTypeBook = a.TypeBook.IdTypeBook,
                       NameTypeBook = a.TypeBook.NameTypeBook
                   },

                   Nationality = a.Nationality,
                   UrlAvatar = _context.Images.Where(x => x.IdAuthor == a.IdAuthor).Select(x => x.Url).FirstOrDefault(),
                   Books = _context.BookWritings
                   .Include(x => x.HeaderBook).ThenInclude(x => x.Books)
                   .SelectMany(bw=>bw.HeaderBook.Books.Select(book => new BookResponse
                   {
                       IdBook = book.IdBook,
                       NameBook = book.HeaderBook.NameHeaderBook,
                       Publisher =book.Publisher,
                       ReprintYear =book.ReprintYear,
                       ValueOfBook = book.ValueOfBook,
                       UrlImage = _context.Images.Where(x=>x.IdBook == book.IdBook).Select(x=>x.Url).FirstOrDefault()??string.Empty
                   })).ToList(),
               }).FirstOrDefaultAsync() ?? null!;
            return authors;
        }
    }
}
