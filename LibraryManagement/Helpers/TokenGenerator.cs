using LibraryManagement.Data;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers.Interface;
using LibraryManagement.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryManagement.Helpers
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _config;
        public TokenGenerator(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateToken(Reader reader)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, reader.ReaderUsername),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (!string.IsNullOrEmpty(reader.RoleName))
            {
                claims.Add(new Claim(ClaimTypes.Role, reader.RoleName));
            }

            // Khóa để ký Token
            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]));

            // Tạo Access Token
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256)
                );

            var tokenHandle = new JwtSecurityTokenHandler();
            var accessTokenString = tokenHandle.WriteToken(token);
            return accessTokenString;
        }

        public string GenerateRefreshToken(Reader reader)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, reader.ReaderUsername),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (!string.IsNullOrEmpty(reader.RoleName))
            {
                claims.Add(new Claim(ClaimTypes.Role, reader.RoleName));
            }

            // Khóa để ký Token
            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]));

            // Tạo Refresh Token
            var refreshToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(1440),
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256)
                );
            var tokenHandle = new JwtSecurityTokenHandler();
            var refreshTokenString = tokenHandle.WriteToken(refreshToken);
            return refreshTokenString;
        }
    }
}
