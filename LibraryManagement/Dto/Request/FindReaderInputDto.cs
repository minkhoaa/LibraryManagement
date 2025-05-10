namespace LibraryManagement.Dto.Request
{
    public class FindReaderInputDto
    {
        public string token { get; set; } = null!;
        public string username { get; set; }= null!;
    }
}
