using AutoMapper;
using LibraryManagement.Data;
using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;
using LibraryManagement.Models;
using LibraryManagement.Repository.InterFace;
using LibraryManagement.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace LibraryManagement.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryManagermentContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthenRepository _account; 

        public AuthorRepository(LibraryManagermentContext context, IMapper mapper, IAuthenRepository account)
        {
            _context = context;
            _mapper = mapper;
            _account = account;         }

        // Lấy danh sách tác giả
        public async Task<List<AuthorResponse>> getListAuthor(string token)
        {
            var reader = await _account.AuthenticationAsync(token);
            var role = await _account.UserRoleCheck(token); 
            if (reader == null || role != 0) return null!;


            var listAuthor = await _context.Authors.ToListAsync();
            return _mapper.Map<List<AuthorResponse>>(listAuthor);
        }

        // Thêm tác giả
        public async Task<ApiResponse<AuthorResponse>> addAuthorAsync(AuthorRequest request)
        {
            var newAuthor = _mapper.Map<Author>(request);
            _context.Authors.Add(newAuthor);
            await _context.SaveChangesAsync();

            var authorResponse = _mapper.Map<AuthorResponse>(newAuthor);
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

            _context.Authors.Update(updateAuthor);
            await _context.SaveChangesAsync();
            var authorResponse = _mapper.Map<AuthorResponse>(updateAuthor);
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
