using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Badge.Models
{
    [Table("Group")]
    public class Group
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public int GroupTypeId { get; set; }
        [ForeignKey(nameof(GroupTypeId))]
        public GroupType GroupType { get; set; }
        public int LeaderId { get; set; }
        [ForeignKey(nameof(LeaderId))]
        public Leader Leader { get; set;}
    }
}
