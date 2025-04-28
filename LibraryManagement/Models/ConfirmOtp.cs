using Microsoft.IdentityModel.Abstractions;

namespace LibraryManagement.Models
{
    public class ConfirmOtp
    {
        public string Email { get; set; } = null!;
        public string Otp { get; set; } = null!;
    }
}
