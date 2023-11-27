using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace Badge.Models
{
    [Table("Leader")]
    public class Leader
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Fornavn")]
        public string FName { get; set; }
        [DisplayName("Efternavn")]
        public string LName { get; set; }
        [DisplayName("Bruger Id")]
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        [DisplayName("Identity User")]
        public IdentityUser IdentityUser { get; set; }
    }
}
