using AutoMapper;
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
    public class ReaderRepository : IReaderRepository
    {
        private readonly LibraryManagermentContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthenRepository _account;
        private readonly IParameterRepository _parameterRepository;
        public ReaderRepository(LibraryManagermentContext contex, 
                                IMapper mapper, 
                                IAuthenRepository authen,
                                IParameterRepository parameterRepository)
        {
            _account = authen;
            _context = contex;
            _mapper = mapper;
            _parameterRepository = parameterRepository;
        }

        // Hàm tạo Id độc giả
         public async Task<string> generateNextIdReaderAsync()
        {
            var nextID = await _context.Readers.OrderByDescending(id => id.IdReader).FirstOrDefaultAsync();

            int nextNumber = 1;

            if (nextID != null && nextID.IdReader.StartsWith("rd"))
            {
                string numberPart = nextID.IdReader.Substring(2);
                if (int.TryParse(numberPart, out int parsed))
                {
                    nextNumber = parsed + 1;
                }
            }
            return $"rd{nextNumber:D5}";
        }
        // Hàm thêm độc giả
        public async Task<ApiResponse<ReaderResponse>> addReaderAsync(ReaderCreationRequest request)
        {
            // Quy định tuổi độc giả
            int readerAge = DateTime.Now.Year - request.Dob.Year;
            if (request.Dob.Date > DateTime.Now.AddYears(-readerAge)) // Kiểm đã qua sinh nhật hay chưa
                readerAge--;

            int minAge = await _parameterRepository.getValueAsync("MinReaderAge");
            int maxAge = await _parameterRepository.getValueAsync("MaxReaderAge");
            if(readerAge < minAge || readerAge > maxAge) // Kiểm tra tuổi độc giả
            {
                return ApiResponse<ReaderResponse>.FailResponse($"Tuổi độc giả phải từ {minAge} đến {maxAge} tuổi", 400);
            }

            var newReader = _mapper.Map<Reader>(request);

            newReader.IdReader = await generateNextIdReaderAsync();
            newReader.ReaderUsername = request.Email;
            newReader.ExpiryDate = newReader.CreateDate.AddMonths(6);
            newReader.ReaderPassword = BCrypt.Net.BCrypt.HashPassword(request.ReaderPassword);
            newReader.RoleName = AppRoles.Reader;

            _context.Readers.Add(newReader);
            await _context.SaveChangesAsync();

            var readerResponse = _mapper.Map<ReaderResponse>(newReader);
            return ApiResponse<ReaderResponse>.SuccessResponse("Thêm độc giả thành công", 201, readerResponse);
        }

        // Hàm lấy danh sách độc giả
        public async Task<List<ReaderResponse>> getAllReaderAsync(string token)
        {
            var reader = await _account.AuthenticationAsync(token);

            var role = await _account.UserRoleCheck(token);

            if (reader == null || role != 0) return null!;

            
            var listReaders = await _context.Readers.ToListAsync(); 
            return _mapper.Map<List<ReaderResponse>>(listReaders);
        }

        // Hàm sửa độc giả
        public async Task<ApiResponse<ReaderResponse>> updateReaderAsync(ReaderUpdateRequest request, string idReader)
        {
            var updateReader = await _context.Readers.FirstOrDefaultAsync(reader => reader.IdReader == idReader);
            if (updateReader == null)
            {
                return ApiResponse<ReaderResponse>.FailResponse("Không tìm thấy độc giả", 404);
            }
            // Quy định tuổi độc giả
            int readerAge = DateTime.Now.Year - request.Dob.Year;
            if (request.Dob.Date > DateTime.Now.AddYears(-readerAge)) // Kiểm đã qua sinh nhật hay chưa
                readerAge--;

            int minAge = await _parameterRepository.getValueAsync("MinReaderAge");
            int maxAge = await _parameterRepository.getValueAsync("MaxReaderAge");
            if (readerAge < minAge || readerAge > maxAge) // Kiểm tra tuổi độc giả
            {
                return ApiResponse<ReaderResponse>.FailResponse($"Tuổi độc giả phải từ {minAge} đến {maxAge} tuổi", 400);
            }

            _mapper.Map(request, updateReader);
            updateReader.Dob = DateTime.SpecifyKind(request.Dob, DateTimeKind.Utc);
            updateReader.ReaderUsername = request.Email;
            await _context.SaveChangesAsync();
            var readerResponse = _mapper.Map<ReaderResponse>(updateReader);
            return ApiResponse<ReaderResponse>.SuccessResponse("Thay đổi thông tin độc giả thành công", 200, readerResponse);
        }

        // Hàm xóa độc giả
        public async Task<ApiResponse<string>> deleteReaderAsync(string idReader)
        {
            var deleteReader = await _context.Readers.FirstOrDefaultAsync(reader => reader.IdReader == idReader);
            if (deleteReader == null)
            {
                return ApiResponse<string>.FailResponse("Không tìm thấy độc giả", 404);
            }
            _context.Readers.Remove(deleteReader);
            await _context.SaveChangesAsync();
            return ApiResponse<string>.SuccessResponse("Đã xóa độc giả", 200, "");
        }

        public async Task<FindReaderOutputDto> findReaderAsync(FindReaderInputDto dto)
        {
            var user = await _account.AuthenticationAsync(dto.token);
            var role = await _account.UserRoleCheck(dto.token);
            if (user == null || role != 0) return null!;

            var listReader = await _context.Readers.Where(x => x.ReaderUsername == dto.username).Select(a => new FindReaderOutputDto
            {
                username = a.ReaderUsername,
                phone = a.Phone!, 
                Email = a.Email!,
                password = a.ReaderPassword,
                DateCreate = a.CreateDate
            }
            ).FirstOrDefaultAsync();
            return listReader!;
        }
    }
}
