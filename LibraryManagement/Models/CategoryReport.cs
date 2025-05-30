using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class CategoryReport
    {
        [Key]
        [Column("id_categoryreport")]
        public Guid IdCategoryReport { get; set; }

        [Column("month_report")]
        public int MonthReport { get; set; }

        [Column("year_report")]
        public int YearReport { get; set; }

        [Column("total_borrowcount")]
        public int TotalBorrowCount { get; set; }

        public ICollection<CategoryReportDetail> CategoryReportDetails { get; set; }
    }
}
