namespace CinemaApp.Web.ViewModels.Ticket
{
    public class BuyTicketInputModel
    {
        public Guid CinemaId { get; set; }

        public Guid MovieId { get; set; }

        public int Quantity { get; set; }

        public string Showtime { get; set; } = null!;
    }
}
