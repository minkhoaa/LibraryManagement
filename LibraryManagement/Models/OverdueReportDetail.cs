using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class OverdueReportDetail
    {
        [Column("id_overduereport")]
        public Guid IdOverdueReport { get; set; }

        [Column("id_thebook")]
        public string IdTheBook { get; set; }

        [Column("borrow_date")]
        public DateTime BorrowDate { get; set; }

        [Column("late_days")]
        public int LateDays { get; set; }

        [ForeignKey("IdTheBook")]
        public TheBook TheBook { get; set; }

        [ForeignKey("IdOverdueReport")]
        public OverdueReport OverdueReport { get; set; }
    }
}
