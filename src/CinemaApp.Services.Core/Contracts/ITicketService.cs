namespace CinemaApp.Services.Core.Contracts
{
    using CinemaApp.Services.Core.Data.Ticket.Request;

    using Data.Ticket.Response;

    public interface ITicketService
    {
        Task<IEnumerable<GetUserAllTicketsResponseModel>> GetUserAllTicketsByUserIdAsync(string userId);

        Task<bool> BuyTicketsByUserIdAndProjectionIdAsync(BuyTicketsRequestModel requestData);

        Task<int> GetBoughtTicketsPerProjectionAsync(Guid projectionId);
    }
}
