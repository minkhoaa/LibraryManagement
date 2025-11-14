using LibraryManagement.Dto.Request;
using LibraryManagement.Service.InterFace;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForgotPasswordController : ControllerBase
    {
        private readonly IForgotPasswordService _forgotPasswordService;

        public ForgotPasswordController(IForgotPasswordService forgotPasswordService)
        {
            _forgotPasswordService = forgotPasswordService;
        }

        // Endpoint gửi otp
        [HttpPost("send_otp")]
        public async Task<IActionResult> sendOTP([FromBody] EmailRequest request)
        {
            var result = await _forgotPasswordService.SendForgotPasswordOtpAsync(request.Email);
            if (result) return Created("", new { success = true, message = "OTP đã được gửi" });
            return BadRequest(new { success = false, message = "Gửi OTP thất bại" });
        }

        // Endpoint gửi lại otp
        [HttpPost("resend_otp")]
        public async Task<IActionResult> resendOTP([FromBody] EmailRequest request)
        {
            var result = await _forgotPasswordService.ResendForgotPasswordOtpAsync(request.Email);
            if (result) return Created("", new { success = true, message = "OTP đã được gửi lại" });
            return BadRequest(new { success = false, message = "Gửi OTP thất bại" });
        }

        // Endpoint xác thực otp
        [HttpPost("verify_otp")]
        public async Task<IActionResult> verifyOTP([FromBody] VerifyOtpRequest request)
        {
            var result = await _forgotPasswordService.VerifyForgotPasswordOtpAsync(request);
            if (result) return Created("", new { success = true, message = "Xác thực OTP thành công" });
            return BadRequest(new { success = false, message = "OTP không thuộc Email" });
        }

        // Endpoint thay đổi mật khẩu
        [HttpPost("change_password")]
        public async Task<IActionResult> changePassword([FromBody] ChangePasswordRequest request)
        {
            var result = await _forgotPasswordService.ChangePasswordAsync(request);
            if (result) return Created("", new { success = true, message = "Thay đổi mật khẩu thành công" });
            return BadRequest(new { success = false, message = "Thay đổi mật khẩu thất bại" });
        }
    }
}
