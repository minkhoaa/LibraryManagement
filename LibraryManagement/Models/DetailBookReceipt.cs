using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class DetailBookReceipt
    {
        [Column("id_bookreceipt")]
        public Guid IdBookReceipt { get; set; }

        [Column("id_book")]
        public string IdBook { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("unitprice")]
        public decimal UnitPrice { get; set; }
    }
}
