using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable
namespace Badge.Models
{
    [Table("Member")]
    public partial class Member
    {

        public Member()
        {
            //Sales = new HashSet<Sale>();
        }

        [Key]
        public int Id { get; set; }
        [DisplayName("Fornavn")]
        public string FName { get; set; }
        [DisplayName("Efternavn")]
        public string LName { get; set; }
        [DisplayName("Gruppe Id")]
        public int GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        [DisplayName("Gruppe")]
        public Group Group { get; set; }



        //[InverseProperty(nameof(Sale.Seller))]
        //public virtual ICollection<Sale> Sales { get; set; }
    }
}
