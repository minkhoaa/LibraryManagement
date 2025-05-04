using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class TypeBook
    {
        [Key]
        public Guid IdTypeBook { get; set; }
        public string NameTypeBook { get; set; }

        public ICollection<HeaderBook> HeaderBooks { get; set; }
        public ICollection<Author> Authors { get; set; }
    }

}
