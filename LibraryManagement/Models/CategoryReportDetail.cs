using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class CategoryReportDetail
    {
        [Column("id_categoryreport")]
        public Guid IdCategoryReport { get; set; }

        [Column("id_typebook")]
        public Guid IdTypeBook { get; set; }

        [Column("borrow_count")]
        public int BorrowCount { get; set; }

        [Column("borrow_ratio")]
        public float BorrowRatio { get; set; }

        [ForeignKey("IdCategoryReport")]
        public CategoryReport CategoryReport { get; set; }

        [ForeignKey("IdTypeBook")]
        public TypeBook TypeBook { get; set; }
    }

}
