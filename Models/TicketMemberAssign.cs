using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Badge.Models
{
    [PrimaryKey(nameof(TicketId), nameof(MemberId))]
    public class TicketMemberAssign
    {
        [Required]
        public string TicketId { get; set; }
        [ForeignKey(nameof(TicketId))]
        
        public Ticket Ticket { get; set; }
        [Required]
        public int MemberId { get; set; }
        [ForeignKey(nameof(MemberId))]
        public Member Member { get; set; }
    }
}
