using LibraryManagement.Dto.Response;
using LibraryManagement.Models;

namespace LibraryManagement.Helpers.Interface
{
    public interface ITokenGenerator
    {
        public string GenerateToken(Reader reader);
        public string GenerateRefreshToken(Reader reader);
    }
}
