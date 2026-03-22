namespace CinemaApp.Data.Repository
{
    using System.Linq.Expressions;

    using Contracts;
    using GCommon.Exceptions;
    using Models;

    using Microsoft.EntityFrameworkCore;

    public class TicketRepository : BaseRepository, ITicketRepository
    {
        public TicketRepository(CinemaAppDbContext dbContext) 
            : base(dbContext)
        {

        }

        public async Task<bool> AddTicketAsync(Ticket ticket, Guid projectionId)
        {
            await DbContext!.Tickets.AddAsync(ticket);
            
            Projection? projection = await DbContext
                .Projections
                .FindAsync(projectionId);
            if (projection == null)
            {
                throw new EntityPersistFailureException();
            }

            projection.AvailableTickets -= ticket.Quantity;

            int resultCount = await SaveChangesAsync();

            return resultCount >= 1;
        }

        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync(Expression<Func<Ticket, bool>>? filterQuery = null, 
            Expression<Func<Ticket, Ticket>>? projectionQuery = null, bool includeMovies = false, bool includeCinemas = false)
        {
            IQueryable<Ticket> ticketsQuery = DbContext!
                .Tickets
                .AsNoTracking();

            if (includeMovies)
            {
                ticketsQuery = ticketsQuery
                    .Include(t => t.Projection)
                    .ThenInclude(p => p!.Movie);
            }

            if (includeCinemas)
            {
                ticketsQuery = ticketsQuery
                    .Include(t => t.Projection)
                    .ThenInclude(p => p!.Cinema);
            }

            if (filterQuery != null)
            {
                ticketsQuery = ticketsQuery
                    .Where(filterQuery);
            }

            if (projectionQuery != null)
            {
                ticketsQuery = ticketsQuery
                    .Select(projectionQuery);
            }

            IEnumerable<Ticket> allTickets = await ticketsQuery
                .ToArrayAsync();

            return allTickets;
        }
    }
}
