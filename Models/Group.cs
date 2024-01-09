using Badge.Areas.Identity.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Badge.Models
{
    [Table("Group")]
    public class Group
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Gruppenavn skal udfyldes.")]
        [StringLength(50)]
        [DisplayName("Gruppenavn")]

        public string Name { get; set; }
        [DisplayName("Gruppetype")]
        public int GroupTypeId { get; set; }
        [ForeignKey(nameof(GroupTypeId))]
        [DisplayName("Gruppetype")]
        public GroupType GroupType { get; set; }
        [DisplayName("Leder Id")]
        public string LeaderId { get; set; }
        [ForeignKey(nameof(LeaderId))]
        [DisplayName("Leder")]
        public ApplicationUser Leader { get; set; }

        [InverseProperty(nameof(Member.Group))]
        public virtual ICollection<Member> Members { get; set; }


    }
}
