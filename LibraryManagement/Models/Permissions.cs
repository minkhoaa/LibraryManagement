using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class Permission
    {
        [Key]
        [Column("permission_name")]
        public string PermissionName { get; set; }

        [Column("description")]
        public string Description { get; set; }
    }

}
