namespace LibraryManagement.Dto.Response
{
    public class SlipBookResponse
    {
        public Guid IdLoanSlipBook { get; set; }
        public string IdTheBook { get; set; }
        public string IdReader { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal FineAmount { get; set; }
    }
}
