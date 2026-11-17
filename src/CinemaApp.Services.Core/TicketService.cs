namespace CinemaApp.Services.Core
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using CinemaApp.Data.Models;
    using CinemaApp.Data.Repository.Contracts;
    using Contracts;
    using Data.Ticket.Request;
    using Data.Ticket.Response;
    using static GCommon.InputValidationConstants.Ticket;

    public class TicketService : ITicketService
    {
        private readonly IUserTicketRepository userTicketRepository;
        private readonly ITicketRepository ticketRepository;
        private readonly UserManager<IdentityUser> userManager;

        public TicketService(IUserTicketRepository userTicketRepository, ITicketRepository ticketRepository,
            UserManager<IdentityUser> userManager)
        {
            this.userTicketRepository = userTicketRepository;
            this.ticketRepository = ticketRepository;
            this.userManager = userManager;
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

        public async Task<bool> BuyTicketsByUserIdAndProjectionIdAsync(BuyTicketsRequestModel requestData)
        {
            Ticket? dbTicket = await ticketRepository
                .AllAsNoTracking()
                .SingleOrDefaultAsync(t => t.ProjectionId == requestData.ProjectionId);
            if (dbTicket == null)
            {
                return false;
            }

            IdentityUser? dbUser = await userManager.FindByIdAsync(requestData.UserId);
            if (dbUser == null)
            {
                return false;
            }

            if (requestData.Quantity < MinimumPurchaseQuantity)
            {
                return false;
            }

            try
            {
                UserTicket newUserTicket = new UserTicket()
                {
                    TicketId = dbTicket.Id,
                    UserId = requestData.UserId,
                    Quantity = requestData.Quantity
                };

                await userTicketRepository.AddAsync(newUserTicket);
                await userTicketRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<int> GetBoughtTicketsPerProjectionAsync(Guid projectionId)
        {
            Ticket? dbTicket = await ticketRepository
                .AllAsNoTracking()
                .Include(t => t.TicketUsers)
                .SingleOrDefaultAsync(t => t.ProjectionId == projectionId);
            if (dbTicket == null)
            {
                return 0;
            }

            int ticketsBought = dbTicket.TicketUsers
                .Sum(ut => ut.Quantity);

            return ticketsBought;
        }
    }
}
