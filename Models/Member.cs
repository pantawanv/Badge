using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Badge.Models
{
    [Table("Member")]
    public class Member
    {
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
    }
}
