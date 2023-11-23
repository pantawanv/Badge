using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Badge.Models
{
    [Table("Sale")]
    public class Sale
    {
        [Key]
        public int Id { get; set; }

        public string TicketId { get; set; }
        [ForeignKey(nameof(TicketId))]
        public Ticket Ticket { get; set; }

        public int SellerId { get; set; }
        [ForeignKey(nameof(SellerId))]
        public Member Seller { get; set; }

        public int ChannelId { get; set; }
        [ForeignKey(nameof(ChannelId))]
        public Channel Channel { get; set; }

        public bool PaymentCollected { get; set; }
        public DateTime SalesDate { get; set; }
        
    }
}
