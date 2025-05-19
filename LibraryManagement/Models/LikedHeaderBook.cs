using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace LibraryManagement.Models
{
    public class LikedHeaderBook
    {
        [Key]
        [Column(name: "id_headerbook")]
        public Guid IdHeaderBook { get; set; }
        [Column("id_reader")]
        public string IdReader { get; set; }

        [Column(name:"liked_day")]
        public DateTime LikedDay { get; set; }

        [ForeignKey("IdHeaderBook")]
        public HeaderBook headerBook { get; set; }
        [ForeignKey("IdReader")]
        public Reader reader { get; set; }
    }
}
