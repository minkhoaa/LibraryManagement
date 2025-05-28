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
    public class ReaderService : IReaderService
    {
        private readonly LibraryManagermentContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthenService _account;
        private readonly IParameterService _parameterRepository;
        private readonly IUpLoadImageFileService _upLoadImageFileRepository;
        public ReaderService(LibraryManagermentContext contex, 
                                IMapper mapper, 
                                IAuthenService authen,
                                IParameterService parameterRepository,
                                IUpLoadImageFileService upLoadImageFileRepository)
        {
            _account = authen;
            _context = contex;
            _mapper = mapper;
            _parameterRepository = parameterRepository;
            _upLoadImageFileRepository = upLoadImageFileRepository;
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

            // Chuỗi url ảnh từ cloudinary
            string imageUrl = null;
            if (request.AvatarImage != null)
            {
                imageUrl = await _upLoadImageFileRepository.UploadImageAsync(request.AvatarImage);
            }

            var newReader = new Reader
            {
                IdReader = await generateNextIdReaderAsync(),
                IdTypeReader = request.IdTypeReader,
                NameReader = request.NameReader,
                Sex = request.Sex,
                Address = request.Address,
                Email = request.Email,
                Dob = DateTime.SpecifyKind(request.Dob, DateTimeKind.Utc),
                Phone = request.Phone,
                CreateDate = DateTime.UtcNow,
                ReaderUsername = request.Email,
                ReaderPassword = BCrypt.Net.BCrypt.HashPassword(request.ReaderPassword),
                RoleName = AppRoles.Reader
            };

            _context.Readers.Add(newReader);
            await _context.SaveChangesAsync();

            // Lưu avatar vào bảng image nếu có
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var image = new Image
                {
                    IdReader = newReader.IdReader,
                    Url = imageUrl,
                };
                _context.Images.Add(image);
                await _context.SaveChangesAsync();
            }

            var readerResponse = new ReaderResponse
            {
                IdReader = newReader.IdReader,
                IdTypeReader = newReader.IdTypeReader,
                NameReader = newReader.NameReader,
                Sex = newReader.Sex,
                Address = newReader.Address,
                Email = newReader.Email,
                Dob = newReader.Dob,
                Phone = newReader.Phone,
                CreateDate = newReader.CreateDate,
                ReaderAccount = newReader.ReaderUsername,
                TotalDebt = newReader.TotalDebt,
                UrlAvatar = imageUrl
            };
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

            // Chuỗi url ảnh từ cloudinary
            string imageUrl = null;
            if (request.AvatarImage != null)
            {
                imageUrl = await _upLoadImageFileRepository.UploadImageAsync(request.AvatarImage);
            }

            updateReader.IdTypeReader = request.IdTypeReader;
            updateReader.NameReader = request.NameReader;
            updateReader.Sex = request.Sex;
            updateReader.Address = request.Address;
            updateReader.Email = request.Email;
            updateReader.Dob = DateTime.SpecifyKind(request.Dob, DateTimeKind.Utc);
            updateReader.Phone = request.Phone;
            updateReader.ReaderUsername = request.Email;
            if (!string.IsNullOrEmpty(request.ReaderPassword))
            {
                updateReader.ReaderPassword = BCrypt.Net.BCrypt.HashPassword(request.ReaderPassword);
            }

            // Cập nhật hoặc thêm mới ảnh nếu có ảnh mới
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var existingAvatar = await _context.Images.FirstOrDefaultAsync(av => av.IdReader == updateReader.IdReader);
                if (existingAvatar != null)
                {
                    existingAvatar.Url = imageUrl;
                    _context.Images.Update(existingAvatar);
                }
                else
                {
                    var image = new Image
                    {
                        IdReader = updateReader.IdReader,
                        Url = imageUrl,
                    };
                    _context.Images.Add(image);
                }
            }

            var readerResponse = new ReaderResponse
            {
                IdReader = updateReader.IdReader,
                IdTypeReader = updateReader.IdTypeReader,
                NameReader = updateReader.NameReader,
                Sex = updateReader.Sex,
                Address = updateReader.Address,
                Email = updateReader.Email,
                Dob = updateReader.Dob,
                Phone = updateReader.Phone,
                CreateDate = updateReader.CreateDate,
                ReaderAccount = updateReader.ReaderUsername,
                TotalDebt = updateReader.TotalDebt,
                UrlAvatar = imageUrl
            };
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
