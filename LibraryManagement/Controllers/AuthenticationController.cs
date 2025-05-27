using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Models;
using LibraryManagement.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenService _authenService;

        public AuthenticationController(IAuthenService authenService)
        {
            _authenService = authenService;
        }

        // Endpoint đăng ký
        [HttpPost("SignUpSendOtp")]
        public async Task<IActionResult> SendOtpSignUp(SignUpModel request)
        {
            var result =  await _authenService.SendEmailConfirmation(request);
            if (result == false) return BadRequest("Người dùng này đã tồn tại");
            return Ok(); 
        }


        [HttpPost("SignUpWithReceivedOtp")]
        public async Task<IActionResult> ConfirmOtpSignUp(ConfirmOtp confirmOtp)
        {
            var result = await _authenService.SignUpWithOtpAsync(confirmOtp);
            if (result == false) return BadRequest();
            return Ok("Đăng kí thành công");
        }
        // Endpoint đăng nhập
        [HttpPost("SignIn")]   
        public async Task<AuthenticationResponse> SignIn(AuthenticationRequest request)
        {
            return await _authenService.SignInAsync(request);
        }
        [HttpPost("Authentication")]
        public async Task<IActionResult> Authentication([FromBody]string token)
        {
            var reader = await _authenService.AuthenticationAsync(token);
            if (reader == null) return NotFound();

            return Ok(reader);
                
        }

        // Endpoint Refresh Token
        [HttpPost("RefreshToken")]
        public async Task<RefreshTokenResponse> RefreshToken([FromBody] string token)
        {
            return await _authenService.refreshTokenAsync(token);
        }
    } 
}
