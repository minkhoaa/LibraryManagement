namespace LibraryManagement.Dto.Response
{
    public class LoanSlipBookResponse
    {
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal FineAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
