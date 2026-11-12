namespace CinemaApp.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    [PrimaryKey(nameof(UserId), nameof(TicketId))]
    public class UserTicket
    {
        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;

        public virtual IdentityUser User { get; set; } = null!;

        [ForeignKey(nameof(Ticket))]
        public Guid TicketId { get; set; }

        public virtual Ticket Ticket { get; set; } = null!;

        public int Quantity { get; set; }
    }
}
