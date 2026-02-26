namespace CinemaApp.Data.Repository
{
    using Contracts;
    using Models;

    using Microsoft.EntityFrameworkCore;

    public class MovieRepository : IMovieRepository, IDisposable
    {
        private bool isDisposed = false;
        private readonly CinemaAppDbContext dbContext;

        public MovieRepository(CinemaAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesNoTrackingWithProjectionAsync(Func<Movie, Movie>? projectFunc = null)
        {
            IQueryable<Movie> moviesFetchQuery = dbContext
                .Movies
                .AsNoTracking()
                .OrderBy(m => m.Title);
            if (projectFunc != null)
            {
                moviesFetchQuery = moviesFetchQuery
                    .Select(m => projectFunc(m))
                    .AsQueryable();
            }

            return await moviesFetchQuery.ToArrayAsync();
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            return await dbContext
                .Movies
                .AsNoTracking()
                .OrderBy(m => m.Title)
                .ToArrayAsync();
        }

        public async Task<Movie?> GetMovieByIdAsync(Guid id)
        {
	        return await dbContext
		        .Movies
		        .FindAsync(id);
        }

        public async Task<bool> AddMovieAsync(Movie movie)
        {
            await dbContext.Movies.AddAsync(movie);
            int resultCount = await SaveChangesAsync();
            
            return resultCount == 1;
        }

        public async Task<bool> EditMovieAsync(Movie movie)
        {
            dbContext.Movies.Update(movie);
            int resultCount = await SaveChangesAsync();

            return resultCount == 1;
        }

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            return await dbContext
                .Movies
                .AnyAsync(m => m.Id == id);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
            isDisposed = true;
        }

        private async Task<int> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync();
        }
    }
}
