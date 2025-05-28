namespace LibraryManagement.Dto.Request
{
    public class HeaderBookCreationRequest
    {
        
        public Guid IdTypeBook { get; set; }
        public string NameHeaderBook { get; set; }
        public string DescribeBook { get; set; }
        public List<Guid> Authors { get; set; }
        public IFormFile BookImage { get; set; }
        public BookCreateRequest bookCreateRequest { get; set; }

    }
    public class BookCreateRequest
    {
        public string Publisher { get; set; }
        public int ReprintYear { get; set; }
        public decimal ValueOfBook { get; set; }
    }
}
