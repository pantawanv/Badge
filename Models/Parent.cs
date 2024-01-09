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
        [DisplayName("Fornavn")]
        [Required (ErrorMessage = "Fornavn skal udfyldes.")]
        public string FName { get; set; }
        [DisplayName("Efternavn")]
        [Required (ErrorMessage = "Efternavn skal udfyldes.")]
        public string LName { get; set; }
        [DisplayName("Tlf.")]
        [Required(ErrorMessage = "Tlf. skal udfyldes.")]
        public string Phone { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "Email skal udfyldes.")]
        public string Email { get; set; }

        [InverseProperty(nameof(MemberParent.Parent))]
        public virtual ICollection<MemberParent> Members { get; set; }
    }
}
