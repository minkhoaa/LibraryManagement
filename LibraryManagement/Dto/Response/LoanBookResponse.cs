namespace LibraryManagement.Dto.Response
{
    public class LoanBookResponse
    {
        public Guid IdLoanSlipBook { get; set; }
        public TheBookInLoanBookResponse BookResponse { get; set; }
        public ReaderInLoanBookResponse readerResponse { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }

    public class ReaderInLoanBookResponse
    {
        public string IdReader { get; set; }
        public string NameReader { get; set; }
    }
    public class TheBookInLoanBookResponse
    {
        public string IdTheBook { get; set; }
        public string NameHeaderBook { get; set; }
        public TypeBookResponse TypeBook { get; set; }
        public List<AuthorInLoanBookResponse> Authors { get; set; }
    }
    public class AuthorInLoanBookResponse
    {
        public Guid IdAuthor { get; set; }
        public string NameAuthor { get; set; }
    }
}
