using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Badge.Models
{
    [PrimaryKey(nameof(TicketId), nameof(GroupId))]
    public class TicketGroupAssign
    {
        [Required]
        public string TicketId { get; set; }
        [ForeignKey(nameof(TicketId))]
        public Ticket Ticket { get; set; }
        [Required]
        public int GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        public Group Group { get; set; }
    }
}
