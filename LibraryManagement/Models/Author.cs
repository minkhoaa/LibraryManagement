using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class Author
    {
        [Key]
        [Column("id_author")]
        public Guid IdAuthor { get; set; }

        [Column("id_typebook")]
        public Guid IdTypeBook { get; set; }

        [Column("name_author")]
        public string NameAuthor { get; set; }

        [Column("nationality")]
        public string Nationality { get; set; }

        [Column("biography")]
        public string Biography { get; set; }

        [ForeignKey("IdTypeBook")]
        public TypeBook TypeBook { get; set; }

        public ICollection<BookWriting> BookWritings { get; set; }
        public ICollection<Image> Images { get; set; }
    }

}

