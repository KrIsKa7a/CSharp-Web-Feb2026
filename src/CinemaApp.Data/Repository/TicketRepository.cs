namespace CinemaApp.Data.Repository
{
    using Common;
    using Contracts;
    using Models;

    public class TicketRepository : BaseRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(CinemaAppDbContext dbContext) 
            : base(dbContext)
        {

        }
    }
}
