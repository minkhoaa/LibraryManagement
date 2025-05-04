namespace LibraryManagement.Dto.Response
{
    public class BookResponse
    {
        public Guid IdHeaderBook { get; set; }
        public string Publisher { get; set; }
        public int ReprintYear { get; set; }
        public decimal ValueOfBook { get; set; }
    }
}
