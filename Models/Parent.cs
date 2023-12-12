using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Badge.Models
{
    [Table("Parent")]
    public class Parent
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Barn")]
        public string MemberId { get; set; }
        [ForeignKey(nameof(MemberId))]
        [DisplayName("Medlem")]
        public Member Member { get; set; }
        [DisplayName("Fornavn")]
        public string FName { get; set; }
        [DisplayName("Efternavn")]
        public string LName { get; set; }
        [DisplayName("Tlf.")]
        public string Phone { get; set; }
        [DisplayName("E-mail")]
        public string Email { get; set; }
    }
}
