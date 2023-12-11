using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Badge.Models
{
    [Table("Sale")]
    public class Sale
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Lodseddel Id")]
        public string TicketId { get; set; }
        [ForeignKey(nameof(TicketId))]
        [DisplayName("Lodseddel")]
        public Ticket Ticket { get; set; }
        [DisplayName("Sælger Id")]
        public int? SellerId { get; set; }
        [ForeignKey(nameof(SellerId))]
        [DisplayName("Sælger")]
        public Member? Seller { get; set; }
        [DisplayName("Betalingskanal Id")]
        public int ChannelId { get; set; }
        [ForeignKey(nameof(ChannelId))]
        [DisplayName("Betalingskanal")]
        public Channel Channel { get; set; }
        [DisplayName("Betalt")]
        public bool PaymentCollected { get; set; }
        [DisplayName("Salgsdato")]
        public DateTime SalesDate { get; set; }

    }
}
