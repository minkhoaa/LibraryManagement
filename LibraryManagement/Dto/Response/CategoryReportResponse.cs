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
        public Guid IdCategoryReport { get; set; }
        public TypeBookResponse typeBookResponse { get; set; }
        public int BorrowCount { get; set; }
        public float BorrowRatio { get; set; }
    }
}
