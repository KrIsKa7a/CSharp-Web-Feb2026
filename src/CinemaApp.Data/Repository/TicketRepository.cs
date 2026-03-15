namespace CinemaApp.Data.Repository
{
    using Contracts;
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
                throw new ArgumentException();
            }

            projection.AvailableTickets -= ticket.Quantity;

            int resultCount = await SaveChangesAsync();

            return resultCount >= 1;
        }
    }
}
