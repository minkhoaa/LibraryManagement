namespace LibraryManagement.Dto.Response
{
    public class HeadbookAndComments
    {
        public string idHeaderBook { get; set; }
        public string nameHeaderBook { get; set; }
        public string describe { get; set; }
        
        public List<EvaluationDetails> Evaluations { get;set; }
 
    }
}
