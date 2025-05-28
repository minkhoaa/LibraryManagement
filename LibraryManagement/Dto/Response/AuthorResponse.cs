namespace LibraryManagement.Dto.Response
{
    public class AuthorResponse
    {
        public Guid IdAuthor { get; set; }
        public Guid IdTypeBook { get; set; }
        public string NameAuthor { get; set; }
        public string Nationality { get; set; }
        public string Biography { get; set; }
    }
}
