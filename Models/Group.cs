using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Badge.Models
{
    [Table("Group")]
    public class Group
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [DisplayName("Gruppe")]
        public string Name { get; set; }
        [DisplayName("Gruppetype Id")]
        public int GroupTypeId { get; set; }
        [ForeignKey(nameof(GroupTypeId))]
        [DisplayName("Gruppetype")]
        public GroupType GroupType { get; set; }
        [DisplayName("Leder Id")]
        public int LeaderId { get; set; }
        [ForeignKey(nameof(LeaderId))]
        [DisplayName("Leder")]
        public Leader Leader { get; set;}
    }
}
