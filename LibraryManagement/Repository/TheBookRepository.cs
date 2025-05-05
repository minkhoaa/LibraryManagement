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
    public class TheBookRepository : ITheBookRepository
    {
        private readonly LibraryManagermentContext _context;
        private readonly IMapper _mapper;

        public TheBookRepository(LibraryManagermentContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<TheBookResponse>> addTheBookAsync(TheBookRequest request)
        {
            var newTheBook = _mapper.Map<TheBook>(request);
            newTheBook.IdTheBook = await generateNextIdTheBookAsync();
            _context.TheBooks.Add(newTheBook);
            await _context.SaveChangesAsync();

            var theBookResponse = _mapper.Map<TheBookResponse>(newTheBook);
            return ApiResponse<TheBookResponse>.SuccessResponse("Thêm cuốn sách thành công", 201, theBookResponse);
        }

        public async Task<ApiResponse<string>> deleteTheBookAsync(string idTheBook)
        {
            var deleteTheBook = await _context.TheBooks.FirstOrDefaultAsync(thebook => thebook.IdTheBook == idTheBook);
            if (deleteTheBook == null)
            {
                return ApiResponse<string>.FailResponse("Không tìm thấy cuốn sách", 404);
            }
            _context.TheBooks.Remove(deleteTheBook);
            await _context.SaveChangesAsync();
            return ApiResponse<string>.SuccessResponse("Đã xóa cuốn sách", 200, "");
        }

        public async Task<ApiResponse<TheBookResponse>> updateTheBookAsync(TheBookRequest request, string idTheBook)
        {
            var updateTheBook = await _context.TheBooks.FirstOrDefaultAsync(thebook => thebook.IdTheBook == idTheBook);
            if (updateTheBook == null)
            {
                return ApiResponse<TheBookResponse>.FailResponse("Không tìm thấy cuốn sách", 404);
            }
            _mapper.Map(request, updateTheBook);

            _context.TheBooks.Update(updateTheBook);
            await _context.SaveChangesAsync();
            var theBookResponse = _mapper.Map<TheBookResponse>(updateTheBook);
            return ApiResponse<TheBookResponse>.SuccessResponse("Thay đổi thông tin cuốn sách thành công", 200, theBookResponse);
        }

        // Hàm tạo Id sách
        public async Task<string> generateNextIdTheBookAsync()
        {
            var nextID = await _context.TheBooks.OrderByDescending(id => id.IdTheBook).FirstOrDefaultAsync();

            int nextNumber = 1;

            if (nextID != null && nextID.IdTheBook.StartsWith("tb00001")) // Kiểm tra có tổn tại sách không và ký tự đầu tiên là book
            {
                string numberPart = nextID.IdTheBook.Substring(2);
                if (int.TryParse(numberPart, out int parsed)) // Kiểm tra chuyển đổi từ string qua int
                {
                    nextNumber = parsed + 1;
                }
            }
            return $"tb{nextNumber:D5}";
        }
    }
}
