namespace CinemaApp.Services.Models.Ticket
{
    public class UserTicketDto
    {
        public string MovieTitle { get; set; } = null!;

        public string? MovieImageUrl { get; set; }

        public string CinemaName { get; set; } = null!;

        public decimal TicketPrice { get; set; }

        public int TicketCount { get; set; }

        public DateTime Showtime { get; set; }
    }
}
