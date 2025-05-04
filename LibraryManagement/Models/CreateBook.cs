namespace LibraryManagement.Models
{

    public class CreateBook
    {
        public Guid IdHeaderBook { get; set; }
        public Guid IdAuthor { get; set; }

        public HeaderBook HeaderBook { get; set; }
        public Author Author { get; set; }
    }

}
