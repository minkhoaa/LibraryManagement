namespace LibraryManagement.Dto.Response
{
    public class LoanBookResponse
    {
        public Guid IdLoanSlipBook { get; set; }
        public string IdTheBook { get; set; }
        public string IdReader { get; set; }
        public DateTime BorrowDate { get; set; }
    }
}
