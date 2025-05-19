using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{

    public class Evaluate
    {
        [Key]
        [Column(name:"id_evaluate")]
        public Guid IdEvaluate { get; set; }
        public string IdReader { get; set; }
        [Column("id_headerbook")]
        public Guid IdHeaderBook { get; set; }
        public string EvaComment { get; set; }
        public int EvaStar { get; set; }
        public DateTime CreateDate { get; set; }

        public Reader Reader { get; set; }
        [ForeignKey("IdHeaderBook")]
        public HeaderBook HeaderBook { get; set; }
    }
}
