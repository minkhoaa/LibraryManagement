namespace LibraryManagement.Dto.Response
{
    public class PenaltyTicketResponse
    {
        public Guid IdPenalty { get; set; }

        public ReaderInPenaltyTicket ReaderResponse { get; set; }

        public DateTime CreatedDate { get; set; }
        public decimal AmountCollected { get; set; }
        public decimal AmountRemaining { get; set; }
    }
    public class ReaderInPenaltyTicket
    {
        public string IdReader { get; set; }
        public string NameReader { get; set; }
    }
}
