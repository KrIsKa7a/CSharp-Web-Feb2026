namespace CinemaApp.Data.Repository.Contracts
{
    using System.Linq.Expressions;
    using Models;

    public interface ITicketRepository
    {
        Task<bool> AddTicketAsync(Ticket ticket, Guid projectionId);

        Task<IEnumerable<Ticket>> GetAllTicketsAsync(Expression<Func<Ticket, bool>>? filterQuery = null,
            Expression<Func<Ticket, Ticket>>? projectionQuery = null, bool includeMovies = false, bool includeCinemas = false);
    }
}
