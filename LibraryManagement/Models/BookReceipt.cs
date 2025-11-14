using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class BookReceipt
    {
        [Key]
        [Column("id_bookreceipt")]
        public Guid IdBookReceipt { get; set; }

        [Column("id_reader")]
        public string IdReader { get; set; }

        [Column("received_date")]
        public DateTime ReceivedDate { get; set; } = DateTime.UtcNow;
    }
}
