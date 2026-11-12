namespace CinemaApp.Data.Repository
{
    using Common;
    using Contracts;
    using Models;

    public class UserTicketRepository : BaseRepository<UserTicket>, IUserTicketRepository
    {
        public UserTicketRepository(CinemaAppDbContext dbContext) 
            : base(dbContext)
        {

        }
    }
}
