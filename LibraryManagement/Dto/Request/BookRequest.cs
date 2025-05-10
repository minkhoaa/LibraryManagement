namespace LibraryManagement.Dto.Request
{
    public class BookRequest
    {  
        public string Publisher { get; set; }
        public int ReprintYear { get; set; }
        public decimal ValueOfBook { get; set; }
    }
}
