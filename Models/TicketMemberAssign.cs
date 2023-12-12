using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string MemberId { get; set; }
        [ForeignKey(nameof(MemberId))]
        public Member Member { get; set; }
    }
}
