﻿namespace LibraryManagement.Dto.Request
{
    public class LoanSlipBookRequest
    {
        public string IdReader { get; set; }
        public string IdTheBook { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
