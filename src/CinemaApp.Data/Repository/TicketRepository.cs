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

        public async Task<bool> AddTicketAsync(Ticket ticket)
        {
            await DbContext!.Tickets.AddAsync(ticket);
            int resultCount = await SaveChangesAsync();

            return resultCount == 1;
        }
    }
}
