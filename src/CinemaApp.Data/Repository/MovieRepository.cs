namespace CinemaApp.Data.Repository
{
    using Contracts;
    using Models;

    using Microsoft.EntityFrameworkCore;

    public class MovieRepository : IMovieRepository
    {
        private readonly CinemaAppDbContext dbContext;

        public MovieRepository(CinemaAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<Movie> GetAllMoviesNoTracking()
        {
            return this.dbContext
                .Movies
                .AsNoTracking();
        }

        public async Task<IEnumerable<Movie>> GetAllMovies()
        {
            return await this.dbContext
                .Movies
                .AsNoTracking()
                .OrderBy(m => m.Title)
                .ToArrayAsync();
        }
    }
}
