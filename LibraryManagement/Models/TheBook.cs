using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class TheBook
    {
        [Key]
        [Column("id_thebook")]
        public string IdTheBook { get; set; }

        [Column("id_book")]
        public string IdBook { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [ForeignKey("IdBook")]
        public Book Book { get; set; }
    }
}
