using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagement.Repository.IRepository
{
    public interface IAuthenRepository
    {
        // Interface đăng ký
        public Task<bool> SignUpWithOtpAsync(ConfirmOtp confirmOtp);

        // Interface đăng nhập
        public Task<AuthenticationResponse> SignInAsync(AuthenticationRequest request);

        public Task<bool> SendEmailConfirmation(SignUpModel signup);

        public Task<Reader?> AuthenticationAsync(string accessToken);

        public Task<int> UserRoleCheck(string token); 

        public Task<RefreshTokenResponse> refreshTokenAsync(string Token);

    }
}
