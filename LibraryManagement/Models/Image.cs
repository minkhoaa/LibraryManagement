using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class Image
    {
        [Key]
        [Column("id_img")]
        public Guid IdImg { get; set; }

        [Column("id_book")]
        public string? IdBook { get; set; }

        [Column("id_reader")]
        public string? IdReader { get; set; }

        [Column("id_author")]
        public Guid? IdAuthor { get; set; }

        [Column("url")]
        public string Url { get; set; }

        [ForeignKey("IdBook")]
        public Book Book { get; set; }

        [ForeignKey("IdAuthor")]
        public Author Author { get; set; }

        [ForeignKey("IdReader")]
        public Reader Reader { get; set; }
    }
}
