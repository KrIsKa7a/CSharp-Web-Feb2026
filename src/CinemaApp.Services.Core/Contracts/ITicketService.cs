namespace CinemaApp.Services.Core.Contracts
{
    using Data.Ticket.Response;

    public interface ITicketService
    {
        Task<IEnumerable<GetUserAllTicketsResponseModel>> GetUserAllTicketsByUserIdAsync(string userId);
    }
}
