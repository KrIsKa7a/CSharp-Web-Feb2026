namespace CinemaApp.Data.Repository
{
    using Common;
    using Contracts;
    using Models;

    public class UserMovieRepository : BaseRepository<UserMovie>, IUserMovieRepository
    {
        public UserMovieRepository(CinemaAppDbContext dbContext) 
            : base(dbContext)
        {

        }
    }
}
