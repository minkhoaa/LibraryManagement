namespace LibraryManagement.Dto.Request
{
    public class BookReceiptRequest
    {
        public HeaderBookCreationRequest headerBook { get; set; }
        public BookCreateRequest book { get; set; }
        public Guid IdReader { get; set; }
        public List<DetailBookReceiptRequest> listDetailsRequest { get; set; }
    }

    public class DetailBookReceiptRequest
    {
        public int Quantity { get; set; }
    }
}
