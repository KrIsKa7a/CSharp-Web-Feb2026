namespace CinemaApp.Data.Repository
{
    using Contracts;
    using GCommon.Exceptions;
    using Models;

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
    }
}
