using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class TypeReader
    {
        [Key]
        [Column("id_typereader")]
        public Guid IdTypeReader { get; set; }

        [Column("name_typereader")]
        public string NameTypeReader { get; set; }

        public ICollection<Reader> Readers { get; set; }
    }

}
