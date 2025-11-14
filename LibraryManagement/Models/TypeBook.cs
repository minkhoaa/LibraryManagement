using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class TypeBook
    {
        [Key]
        [Column("id_typebook")]
        public Guid IdTypeBook { get; set; }

        [Column("name_typebook")]
        public string NameTypeBook { get; set; }

        public ICollection<HeaderBook> HeaderBooks { get; set; }
        public ICollection<Author> Authors { get; set; }
        public ICollection<CategoryReportDetail> CategoryReportDetails { get; set; }
    }

}
