namespace CinemaApp.Data.Repository.Contracts
{
    using Models;

    public interface ITicketRepository
    {
        Task<bool> AddTicketAsync(Ticket ticket, Guid projectionId);
    }
}
