using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{

    public class BookWriting
    {
        [Column("id_headerbook")]
        public Guid IdHeaderBook { get; set; }

        [Column("id_author")]
        public Guid IdAuthor { get; set; }

        [ForeignKey("IdHeaderBook")]
        public HeaderBook HeaderBook { get; set; }

        [ForeignKey("IdAuthor")]
        public Author Author { get; set; }

        
    }

}
