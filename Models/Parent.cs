using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Badge.Models
{
    [Table("Parent")]
    public class Parent
    {
        [Key]
        public int Id { get; set; }
        public int MemberId { get; set; }
        [ForeignKey(nameof(MemberId))]
        public Member Member { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
