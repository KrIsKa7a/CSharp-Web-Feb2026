namespace CinemaApp.Services.Core.Contracts
{
    using Models.Ticket;

    public interface ITicketService
    {
        Task<bool> BuyTicketAsync(Guid projectionId, string userId, int quantity);

        Task<IEnumerable<UserTicketDto>> GetUserTicketsAsync(string userId);
    }
}
