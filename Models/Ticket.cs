﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Badge.Models
{
    [Table("Ticket")]
    public class Ticket
    {
        [Key]
        [DisplayName("Stregkode")]
        [Required(ErrorMessage = "Stregkode skal udfyldes.")]
        public string Id { get; set; }

        [InverseProperty(nameof(TicketGroupAssign.Ticket))]
        public TicketGroupAssign? TicketGroupAssign { get; set; }

        [InverseProperty(nameof(TicketMemberAssign.Ticket))]
        public TicketMemberAssign? TicketMemberAssign { get; set; }

        [InverseProperty(nameof(Sale.Ticket))]
        public Sale? Sale { get; set; }
    }
}
