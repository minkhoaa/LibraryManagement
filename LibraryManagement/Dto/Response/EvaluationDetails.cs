namespace LibraryManagement.Dto.Response
{
    public class EvaluationDetails
    {
        public Guid IdEvaluation { get; set; }
        public string IdReader { get; set; }

        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime Create_Date { get; set; }
    }
}
