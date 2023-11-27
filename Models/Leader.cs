using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Badge.Models
{
    [Table("Leader")]
    public class Leader
    {
        [Key]
        public int Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public IdentityUser IdentityUser { get; set; }
    }
}
