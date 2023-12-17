using Badge.Areas.Identity.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable
namespace Badge.Models
{
    [Table("Member")]
    public partial class Member
    {
        public string Id { get; set; }
        [ForeignKey(nameof(Id))]
        public ApplicationUser User { get; set; }
        [DisplayName("Gruppe Id")]
        public int? GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        [DisplayName("Gruppe")]
        public Group? Group { get; set; }

        [InverseProperty(nameof(Sale.Seller))]
        public virtual ICollection<Sale> Sales { get; set; }

        [InverseProperty(nameof(MemberParent.Member))]
        public virtual ICollection<MemberParent> Parents { get; set; }

    }
}
