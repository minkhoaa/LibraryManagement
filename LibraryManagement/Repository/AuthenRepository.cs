﻿using AutoMapper;
using FluentEmail.Core;
using LibraryManagement.Data;
using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;
using LibraryManagement.Helpers.Interface;
using LibraryManagement.Models;
using LibraryManagement.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryManagement.Repository
{
    public class AuthenRepository : IAuthenRepository
    {
        private readonly LibraryManagermentContext _context;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IMapper _mapper;
        private readonly IFluentEmail _fluentEmail;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _tempOtp;
        private readonly IReaderRepository _readerRepo; 


        private static readonly Guid DefaultTypeReaderId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");

        public AuthenRepository(LibraryManagermentContext context, ITokenGenerator tokenGenerator,
            IMapper mapper, IFluentEmail email, IMemoryCache memoryCache, IConfiguration configuration, IReaderRepository reader)
        {
            _configuration = configuration; 
            _tempOtp = memoryCache;
            _fluentEmail = email; 
            _context = context;
            _tokenGenerator = tokenGenerator;
            _mapper = mapper;
            _readerRepo = reader;
        }

        // Hàm đăng nhập
        public async Task<AuthenticationResponse> SignInAsync(AuthenticationRequest request)
        {
            var reader = await _context.Readers.FirstOrDefaultAsync(reader => reader.ReaderUsername == request.username);
            if (reader == null || !BCrypt.Net.BCrypt.Verify(request.password, reader.ReaderPassword))
                throw new Exception("Unauthenticated");
            var _token = _tokenGenerator.GenerateToken(reader);
            var _refreshToken = _tokenGenerator.GenerateRefreshToken(reader);
            return new AuthenticationResponse
            {
                Token = _token,
                refreshToken = _refreshToken
            };
        }




        // Hàm đăng ký
        public async Task<bool> SignUpWithOtpAsync(ConfirmOtp confirmOtp)
        {
            var newRole = await _context.Roles.FirstOrDefaultAsync(role => role.RoleName == AppRoles.Reader);

            if (!_tempOtp.TryGetValue($"OTP_{confirmOtp.Email}", out dynamic? cacheData)) return false;

            if (cacheData.Otp != confirmOtp.Otp) return false; 

            if (newRole == null) // Nếu role Reader chưa có trong csdl
            {
                newRole = new Role
                {
                    RoleName = AppRoles.Reader,
                    Description = "Reader Role"
                };
                _context.Roles.Add(newRole);
                await _context.SaveChangesAsync();
            }

            var reader = new Reader
            {

                ReaderUsername = confirmOtp.Email,
                ReaderPassword = BCrypt.Net.BCrypt.HashPassword(cacheData.Password),
                IdTypeReader = DefaultTypeReaderId,
                RoleName = newRole.RoleName
            };
           
            await _context.Readers.AddAsync(reader);
            await _readerRepo.addReaderAsync(reader); 
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SendEmailConfirmation(SignUpModel signup)
        {
            var user = await _context.Readers.FirstOrDefaultAsync(x => x.ReaderUsername == signup.Email);
            if (user != null)
            {
                throw new Exception("Người dùng đã tồn tại");
            }
            try
            {
                var otp = new Random().Next(100000, 999999).ToString();

                _tempOtp.Set($"OTP_{signup.Email}", new
                {
                    Otp = otp,
                    Password = signup.Password
                }, TimeSpan.FromMinutes(1));
                await _fluentEmail.To(signup.Email)
                    .SetFrom("noreply@gmail.com")
                    .Subject("Mã OTP xác thực của bạn là:")
                    .Body($"<p>Mã OTP của bạn là: <strong>{otp}</strong> (hiệu lực trong 1 phút).</p>", true)
                    .SendAsync();
                return true;
            }
            catch
            {
                return false;

            }
        }

        public async Task<Reader?> AuthenticationAsync(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken)) return null;

            var tokenHanlder = new JwtSecurityTokenHandler();
            var secretKey = Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]); 

            try
            {
                tokenHanlder.ValidateToken(accessToken, new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken  );
                var jwtToken = (JwtSecurityToken)validatedToken;
                var email = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email)) return null;
                var reader = await _context.Readers.FirstOrDefaultAsync(x => x.ReaderUsername == email);
                return reader;
            }
            catch
            {
                return null; 
            }
        }

        // Hàm refresh Token
        public async Task<RefreshTokenResponse> refreshTokenAsync(string Token)
        {
            var reader = await AuthenticationAsync(Token);
            if(reader == null)
            {
                throw new Exception("Invalid or Expired Token");
            }

            var accessTokenResponse = _tokenGenerator.GenerateToken(reader);

            return new RefreshTokenResponse
            {
                AccessToken = accessTokenResponse
            };
        }

        public async Task<int> UserRoleCheck(string token)
        {
            var user = await AuthenticationAsync(token);
            if (user == null) return -1;
            if (user.RoleName.ToLower() == "admin") return 0;
            if (user.RoleName.ToLower() == "reader") return 1;
            return -1;
        }
    }
}
