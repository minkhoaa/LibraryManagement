namespace LibraryManagement.Dto.Response
{
    public class HeaderBookResponse
    {
        public Guid IdTypeBook { get; set; }
        public string NameHeaderBook { get; set; }
        public string DescribeBook { get; set; }
        public List<Guid> IdAuthors { get; set; }
        public string BookImage { get; set; }
        public BookResponse bookResponse { get; set; }
        public TheBookResponse thebookReponse { get; set; }

    }
    public class BookResponse
    {
        public string Publisher { get; set; }
        public int ReprintYear { get; set; }
        public decimal ValueOfBook { get; set; }
    }
    public class TheBookResponse
    {
        public string Status { get; set; }
    }
}
