namespace CinemaApp.Services.Core
{
    using Contracts;
    using Data.Models;
    using Data.Repository.Contracts;
    using Models.Ticket;

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

        public async Task<IEnumerable<UserTicketDto>> GetUserTicketsAsync(string userId)
        {
            IEnumerable<Ticket> allUserTickets = await ticketRepository
                .GetAllTicketsAsync(
                    filterQuery: t => t.UserId.ToString() == userId,
                    projectionQuery: ticket => new Ticket()
                    {
                        Projection = ticket.Projection,
                        Quantity = ticket.Quantity,
                    },
                    includeMovies: true,
                    includeCinemas: true);

            ICollection<UserTicketDto> userTicketDtos = new List<UserTicketDto>();
            foreach (Ticket userTicket in allUserTickets.Where(t => t.Projection != null))
            {
                // AutoMapper not used since most properties require manual configuration
                UserTicketDto userTicketDto = new UserTicketDto()
                {
                    MovieTitle = userTicket.Projection!.Movie.Title,
                    MovieImageUrl = userTicket.Projection.Movie.ImageUrl,
                    CinemaName = userTicket.Projection.Cinema.Name,
                    Showtime = userTicket.Projection.Showtime,
                    TicketCount = userTicket.Quantity,
                    TicketPrice = userTicket.Projection.TicketPrice,
                };

                userTicketDtos.Add(userTicketDto);
            }

            return userTicketDtos;
        }
    }
}
