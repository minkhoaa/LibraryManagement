﻿namespace LibraryManagement.Dto.Response
{
    public class LoanSlipBookResponse
    {
        public Guid IdLoanSlipBook { get; set; }
        public string IdTheBook { get; set; }
        public string IdReader { get; set; }
        public string IdBook { get; set; }
        public string NameBook { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int LoanPeriod {get; set; }
        public decimal FineAmount { get; set; }
        public bool IsReturned { get; set; }
    }
}
