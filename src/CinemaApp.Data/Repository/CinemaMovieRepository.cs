namespace CinemaApp.Data.Repository
{
    using Common;
    using Contracts;
    using Models;

    public class CinemaMovieRepository : BaseSoftDeleteableRepository<CinemaMovie>, ICinemaMovieRepository
    {
        public CinemaMovieRepository(CinemaAppDbContext dbContext) 
            : base(dbContext)
        {

        }
    }
}
