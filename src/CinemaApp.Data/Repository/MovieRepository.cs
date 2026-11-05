namespace CinemaApp.Data.Repository
{
    using Common;
    using Contracts;
    using Models;

    public class MovieRepository : BaseSoftDeleteableRepository<Movie>, IMovieRepository
    {
        public MovieRepository(CinemaAppDbContext dbContext) 
            : base(dbContext)
        {

        }

        public async Task<Movie?> FindByIdAsync(Guid id)
        {
            return await DbSet.FindAsync(id);
        }
    }
}
