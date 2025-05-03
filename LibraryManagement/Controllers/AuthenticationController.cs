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
        private readonly IAuthenRepository _authenRepository;

        public AuthenticationController(IAuthenRepository authenRepository)
        {
            _authenRepository = authenRepository;
        }

        // Endpoint đăng ký
        [HttpPost("SignUpSendOtp")]
        public async Task<IActionResult> SendOtpSignUp(SignUpModel request)
        {
            var result =  await _authenRepository.SendEmailConfirmation(request);
            if (result == false) return BadRequest();
            return Ok(); 
        }


        [HttpPost("SignUpWithReceivedOtp")]
        public async Task<IActionResult> ConfirmOtpSignUp(ConfirmOtp confirmOtp)
        {
            var result = await _authenRepository.SignUpWithOtpAsync(confirmOtp);
            if (result == false) return BadRequest();
            return Ok();
        }
        // Endpoint đăng nhập
        [HttpPost("SignIn")]   
        public async Task<AuthenticationResponse> SignIn(AuthenticationRequest request)
        {
            return await _authenRepository.SignInAsync(request);
        }
        [HttpPost("Authentication")]
        public async Task<IActionResult> Authentication([FromBody]string token)
        {
            var reader = await _authenRepository.AuthenticationAsync(token);
            if (reader == null) return NotFound();

            return Ok(new
            {
                reader.ReaderUsername,
                reader.Email
            });
        }

        // Endpoint Refresh Token
        [HttpPost("RefreshToken")]
        public async Task<RefreshTokenResponse> RefreshToken([FromBody] string token)
        {
            return await _authenRepository.refreshTokenAsync(token);
        }
    } 
}
