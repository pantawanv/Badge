using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Badge.Models
{
    [Table("GroupType")]
    public class GroupType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [DisplayName("Navn")]
        public string Name { get; set; }
    }

}
