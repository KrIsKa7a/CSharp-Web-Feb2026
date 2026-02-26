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
            return dbContext
                .Movies
                .AsNoTracking();
        }

        public async Task<IEnumerable<Movie>> GetAllMovies()
        {
            return await dbContext
                .Movies
                .AsNoTracking()
                .OrderBy(m => m.Title)
                .ToArrayAsync();
        }

        public async Task<bool> AddMovieAsync(Movie movie)
        {
            await dbContext.Movies.AddAsync(movie);
            int resultCount = await SaveChangesAsync();
            
            return resultCount == 1;
        }

        private async Task<int> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync();
        }
    }
}
