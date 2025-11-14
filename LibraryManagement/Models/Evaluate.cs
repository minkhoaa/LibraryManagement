using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{

    public class Evaluate
    {
        [Key]
        [Column(name:"id_evaluate")]
        public Guid IdEvaluate { get; set; }
        [Column(name:"id_reader")]
        public string IdReader { get; set; }
        [Column("id_book")]
        public string IdBook { get; set; }
        [Column("eva_comment")]

        public string EvaComment { get; set; }
        [Column("eva_star")]

        public int EvaStar { get; set; }
        [Column("create_date")]

        public DateTime CreateDate { get; set; }

        [ForeignKey("IdReader")]

        public Reader Reader { get; set; }

        [ForeignKey("IdBook")]
        public Book Book { get; set; }
    }
}
