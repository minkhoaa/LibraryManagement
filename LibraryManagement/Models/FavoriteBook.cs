using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class FavoriteBook
    {
        [Key]
        [Column("id_book")]
        public string IdBook { get; set; }

        [Column("id_reader")]
        public string IdReader { get; set; }

        [Column("create_day")]
        public DateTime createDay { get; set; }

        [ForeignKey("IdBook")]
        public Book book { get; set; }

        [ForeignKey("IdReader")]
        public Reader reader { get; set; }
    }
}
