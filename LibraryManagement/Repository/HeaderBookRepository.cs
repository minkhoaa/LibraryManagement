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
    public class HeaderBookRepository : IHeaderBookRepository
    {
        private readonly LibraryManagermentContext _context;
        private readonly IMapper _mapper;

        public HeaderBookRepository(LibraryManagermentContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Hàm lấy danh sách đầu sách
        public async Task<List<HeaderBookResponse>> getListHeaderBook()
        {
            var listHeaderBook = await _context.HeaderBooks.ToListAsync();
            return _mapper.Map<List<HeaderBookResponse>>(listHeaderBook);
        }

        // Hàm thêm mới đầu sách
        public async Task<ApiResponse<HeaderBookResponse>> addHeaderBookAsync(HeaderBookRequest request)
        {
            var newHeaderBook = _mapper.Map<HeaderBook>(request);
            _context.HeaderBooks.Add(newHeaderBook);
            await _context.SaveChangesAsync();

            var headerBookResponse = _mapper.Map<HeaderBookResponse>(newHeaderBook);
            return ApiResponse<HeaderBookResponse>.SuccessResponse("Thêm đầu sách thành công", 201, headerBookResponse);
        }

        // Hàm xóa đầu sách
        public async Task<ApiResponse<string>> deleteHeaderBookAsync(Guid idHeaderBook)
        {
            var deleteHeaderBook = await _context.HeaderBooks.FirstOrDefaultAsync(
                headerbook => headerbook.IdHeaderBook == idHeaderBook);
            if (deleteHeaderBook == null)
            {
                return ApiResponse<string>.FailResponse("Không tìm thấy đầu sách", 404);
            }
            _context.HeaderBooks.Remove(deleteHeaderBook);
            await _context.SaveChangesAsync();
            return ApiResponse<string>.SuccessResponse("Đã xóa đầu sách", 200, "");
        }

        // Hàm cập nhật đầu sách
        public async Task<ApiResponse<HeaderBookResponse>> updateHeaderBookAsync(HeaderBookRequest request, 
                                                                                 Guid idHeaderBook)
        {
            var updateHeaderBook = await _context.HeaderBooks.FirstOrDefaultAsync(
                headerbook => headerbook.IdHeaderBook == idHeaderBook);
            if (updateHeaderBook == null)
            {
                return ApiResponse<HeaderBookResponse>.FailResponse("Không tìm thấy đầu sách", 404);
            }
            _mapper.Map(request, updateHeaderBook);

            _context.HeaderBooks.Update(updateHeaderBook);
            await _context.SaveChangesAsync();
            var headerBookResponse = _mapper.Map<HeaderBookResponse>(updateHeaderBook);
            return ApiResponse<HeaderBookResponse>.SuccessResponse("Thay đổi thông tin đầu sách thành công", 200, headerBookResponse);
        }
    }
}
