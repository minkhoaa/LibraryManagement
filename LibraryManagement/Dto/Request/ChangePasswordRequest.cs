namespace LibraryManagement.Dto.Request
{
    public class ChangePasswordRequest
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string RepeatPassword { get; set; }
    }
}
