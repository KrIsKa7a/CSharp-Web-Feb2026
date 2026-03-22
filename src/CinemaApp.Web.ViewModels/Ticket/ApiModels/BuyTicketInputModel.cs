namespace CinemaApp.Web.ViewModels.Ticket.ApiModels
{
    public class BuyTicketInputModel
    {
        public Guid CinemaId { get; set; }

        public Guid MovieId { get; set; }

        public int Quantity { get; set; }

        public Guid ProjectionId { get; set; }
    }
}
