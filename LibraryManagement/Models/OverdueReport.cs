using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class OverdueReport
    {
        [Key]
        [Column("id_overduereport")]
        public Guid IdOverdueReport { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
        public ICollection<OverdueReportDetail> OverdueReportDetails { get; set; }
    }
}
