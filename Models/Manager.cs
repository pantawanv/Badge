using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Badge.Models
{
    [Table("Manager")]
    public class Manager
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
        public IdentityUser identityUser { get; set; }
    }
}
