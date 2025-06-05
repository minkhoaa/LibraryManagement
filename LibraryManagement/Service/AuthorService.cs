﻿using AutoMapper;
using LibraryManagement.Data;
using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;
using LibraryManagement.Models;
using LibraryManagement.Repository.InterFace;
using LibraryManagement.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

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
        public async Task<List<AuthorResponse>> getListAuthor(string token)
        {
            var reader = await _account.AuthenticationAsync(token);
            var role = await _account.UserRoleCheck(token); 
            if (reader == null || role != 0) return null!;

            var listAuthor = await _context.Authors
                .Include(a => a.Images)
                .Include(a => a.TypeBook)
                .ToListAsync();

            var authorResponse = new List<AuthorResponse>();

            foreach (var author in listAuthor)
            {
                var response = new AuthorResponse
                {
                    IdAuthor = author.IdAuthor,
                    NameAuthor = author.NameAuthor,
                    Nationality = author.Nationality,
                    Biography = author.Biography,
                    UrlAvatar = author.Images?.FirstOrDefault()?.Url,
                    IdTypeBook = author.TypeBook != null
                        ? new TypeBookResponse
                        {
                            IdTypeBook = author.TypeBook.IdTypeBook,
                            NameTypeBook = author.TypeBook.NameTypeBook
                        }
                        : null
                };

                authorResponse.Add(response);
            }
            return authorResponse;
        }

        // Thêm tác giả
        public async Task<ApiResponse<AuthorResponse>> addAuthorAsync(AuthorRequest request)
        {
            var newAuthor = _mapper.Map<Author>(request);

            // Chuỗi url ảnh từ cloudinary
            string imageUrl = null;
            if (request.AvatarImage != null)
            {
                imageUrl = await _upLoadImageFileService.UploadImageAsync(request.AvatarImage);
            }
            _context.Authors.Add(newAuthor);
            await _context.SaveChangesAsync();

            // Lưu avatar vào bảng image nếu có
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var image = new Image
                {
                    IdAuthor = newAuthor.IdAuthor,
                    Url = imageUrl,
                };
                _context.Images.Add(image);
                await _context.SaveChangesAsync();
            }

            var authorResponse = _mapper.Map<AuthorResponse>(newAuthor);

            var typeBook = await _context.TypeBooks.FindAsync(newAuthor.IdTypeBook);
            authorResponse.IdTypeBook = new TypeBookResponse
            {
                IdTypeBook = newAuthor.IdTypeBook,
                NameTypeBook = typeBook?.NameTypeBook,
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
        public async Task<ApiResponse<AuthorResponse>> updateAuthorAsync(AuthorRequest request, Guid idAuthor)
        {
            var updateAuthor = await _context.Authors.FirstOrDefaultAsync(author => author.IdAuthor == idAuthor);
            if (updateAuthor == null)
            {
                return ApiResponse<AuthorResponse>.FailResponse("Không tìm thấy tác giả", 404);
            }
            _mapper.Map(request, updateAuthor);

            // Chuỗi url ảnh từ cloudinary
            string imageUrl = null;
            if (request.AvatarImage != null)
            {
                imageUrl = await _upLoadImageFileService.UploadImageAsync(request.AvatarImage);
            }

            _context.Authors.Update(updateAuthor);
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
            }

            var authorResponse = _mapper.Map<AuthorResponse>(updateAuthor);

            var typeBook = await _context.TypeBooks.FindAsync(updateAuthor.IdTypeBook);
            authorResponse.IdTypeBook = new TypeBookResponse
            {
                IdTypeBook = updateAuthor.IdTypeBook,
                NameTypeBook = typeBook?.NameTypeBook,
            };
            authorResponse.UrlAvatar = imageUrl;
            return ApiResponse<AuthorResponse>.SuccessResponse("Thay đổi thông tin tác giả thành công", 200, authorResponse);
        }

        public async Task<List<Author>> findAuthor(FindAuthorInputDto dto)
        {
            var user = await _account.AuthenticationAsync(dto.token);
            var role = await _account.UserRoleCheck(dto.token);

            if (user == null || role != 0) return null!;

            var authors = await _context.Authors.Where(x => x.NameAuthor == dto.nameAuthor).ToListAsync();
            return authors;
        }
    }
}
