using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class Image
    {
        [Key]
        [Column("id_img")]
        public Guid IdImg { get; set; }

        [Column("id_headerbook")]
        public Guid? IdHeaderBook { get; set; }

        [Column("id_reader")]
        public string IdReader { get; set; }

        [Column("id_author")]
        public Guid? IdAuthor { get; set; }

        [Column("url")]
        public string Url { get; set; }
    }
}
