namespace CinemaApp.Services.Core
{
    using Contracts;
    using Data.Models;
    using Data.Repository.Contracts;

    public class TicketService : ITicketService
    {
        private readonly ITicketRepository ticketRepository;
        private readonly IProjectionRepository projectionRepository;

        public TicketService(ITicketRepository ticketRepository, IProjectionRepository projectionRepository)
        {
            this.ticketRepository = ticketRepository;
            this.projectionRepository = projectionRepository;
        }

        public async Task<bool> BuyTicketAsync(Guid projectionId, string userId, int quantity)
        {
            Projection? projection = await projectionRepository
                .FindByIdAsync(projectionId);
            if (projection == null)
            {
                return false;
            }

            if (quantity <= 0 || projection.AvailableTickets < quantity)
            {
                return false;
            }

            Ticket newTicket = new Ticket()
            {
                ProjectionId = projectionId,
                UserId = Guid.Parse(userId),
                Quantity = quantity,
            };

            bool success = await ticketRepository.AddTicketAsync(newTicket, projectionId);

            return success;
        }
    }
}
