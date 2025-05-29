using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Dto.Response
{
    public class CategoryReportResponse
    {
        public Guid IdCategoryReport { get; set; }
        public int MonthReport { get; set; }
        public int TotalBorrowCount { get; set; }
        public List<CategoryDetailReportResponse> categoryDetailReportResponse { get; set; }
    }
    public class CategoryDetailReportResponse
    {
        [Column("id_categoryreport")]
        public Guid IdCategoryReport { get; set; }

        [Column("id_typebook")]
        public TypeBookResponse typeBookResponse { get; set; }

        [Column("borrow_count")]
        public int BorrowCount { get; set; }

        [Column("borrow_ratio")]
        public float BorrowRatio { get; set; }
    }
}
