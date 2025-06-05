using LibraryManagement.Dto.Request;

namespace LibraryManagement.Service.InterFace
{
    public interface IForgotPasswordService
    {
        public Task<bool> SendForgotPasswordOtpAsync(string email);
        public Task<bool> ResendForgotPasswordOtpAsync(string email);
        public Task<bool> VerifyForgotPasswordOtpAsync(VerifyOtpRequest request);
        public Task<bool> ChangePasswordAsync(ChangePasswordRequest request);
    }
}
