using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class Otp
    {
        [Key]
        [Column("id_otp")]
        public Guid IdOtp { get; set; }

        [Column("id_reader")]
        public string IdReader { get; set; }

        [Column("otp")]
        public int otp { get; set; }

        [Column("expiration_time")]
        public DateTime expirationTime { get; set; }

        [ForeignKey("IdReader")]
        public Reader Reader { get; set; }
    }
}
