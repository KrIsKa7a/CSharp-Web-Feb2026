namespace CinemaApp.Services.Core
{
    using Microsoft.EntityFrameworkCore;

    using CinemaApp.Data.Repository.Contracts;
    using Contracts;
    using Data.Ticket.Response;

    public class TicketService : ITicketService
    {
        private readonly IUserTicketRepository userTicketRepository;

        public TicketService(IUserTicketRepository userTicketRepository)
        {
            this.userTicketRepository = userTicketRepository;
        }

        public async Task<IEnumerable<GetUserAllTicketsResponseModel>> GetUserAllTicketsByUserIdAsync(string userId)
        {
            IEnumerable<GetUserAllTicketsResponseModel> responseData = await userTicketRepository
                .AllAsNoTracking()
                .Select(ut => new GetUserAllTicketsResponseModel()
                {
                    CinemaName = ut.Ticket.Projection.Cinema.Name,
                    MovieTitle = ut.Ticket.Projection.Movie.Title,
                    MovieImageUrl = ut.Ticket.Projection.Movie.ImageUrl,
                    Price = ut.Ticket.Price,
                    Quantity = ut.Quantity
                })
                .OrderBy(t => t.MovieTitle)
                .ThenByDescending(t => t.Quantity)
                .ToArrayAsync();

            return responseData;
        }
    }
}
