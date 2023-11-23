using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Badge.Models
{
    [Table("Member")]
    public class Member
    {
        [Key]
        public int Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public int GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        public Group Group { get; set; }
    }
}
