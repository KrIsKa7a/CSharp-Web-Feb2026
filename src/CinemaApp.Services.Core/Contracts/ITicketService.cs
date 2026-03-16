namespace CinemaApp.Services.Core.Contracts
{
    public interface ITicketService
    {
        public Task<bool> BuyTicketAsync(Guid projectionId, string userId, int quantity);
    }
}
