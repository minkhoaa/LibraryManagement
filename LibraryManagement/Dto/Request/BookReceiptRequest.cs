﻿namespace LibraryManagement.Dto.Request
{
    public class BookReceiptRequest
    {
        public HeaderBookCreationRequest headerBook { get; set; }
        public string IdReader { get; set; }
        public List<DetailBookReceiptRequest> listDetailsRequest { get; set; }
    }

    public class DetailBookReceiptRequest
    {
        public int Quantity { get; set; }
    }
}
