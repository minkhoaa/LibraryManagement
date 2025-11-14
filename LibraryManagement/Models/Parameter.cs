using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class Parameter
    {
        [Key]
        [Column("id_parameter")]
        public Guid IdParameter { get; set; }
        [Column("name_parameter")]
        public string NameParameter { get; set; }
        [Column("value_parameter")]
        public int ValueParameter { get; set; }
    }
}
