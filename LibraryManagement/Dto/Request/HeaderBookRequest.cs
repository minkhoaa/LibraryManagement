namespace LibraryManagement.Dto.Request
{
    public class HeaderBookRequest
    {
        public Guid IdTypeBook { get; set; }
        public string NameHeaderBook { get; set; }
        public string DescribeBook { get; set; }
        public List<Guid> IdAuthors { get; set; }

    }
}
