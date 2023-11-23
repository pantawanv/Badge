using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Badge.Models
{
    [Table("Ticket")]
    public class Ticket
    {
        [Key]
        string Id { get; set; }
    }
}
