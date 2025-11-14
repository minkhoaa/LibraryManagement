using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class PenaltyTicket
    {
        [Key]
        [Column("id_penalty")]
        public Guid IdPenalty { get; set; }

        [Column("id_reader")]
        public string IdReader { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("amount_collected")]
        public decimal AmountCollected { get; set; }

        [Column("amount_remaining")]
        public decimal AmountRemaining { get; set; }

        [ForeignKey("IdReader")]
        public Reader Reader { get; set; }
    }

}
