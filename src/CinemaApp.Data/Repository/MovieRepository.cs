namespace CinemaApp.Data.Repository
{
    using Contracts;
    using Models;

    using Microsoft.EntityFrameworkCore;

    public class MovieRepository : BaseRepository, IMovieRepository
    {
        public MovieRepository(CinemaAppDbContext dbContext)
            : base(dbContext)
        {

        }

        public async Task<IEnumerable<Movie>> GetAllMoviesNoTrackingWithProjectionAsync(Func<Movie, Movie>? projectFunc = null)
        {
            IQueryable<Movie> moviesFetchQuery = DbContext
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
            return await DbContext
                .Movies
                .AsNoTracking()
                .OrderBy(m => m.Title)
                .ToArrayAsync();
        }

        public async Task<Movie?> GetMovieByIdAsync(Guid id)
        {
	        return await DbContext
                .Movies
		        .FindAsync(id);
        }

        public async Task<bool> AddMovieAsync(Movie movie)
        {
            await DbContext.Movies.AddAsync(movie);
            int resultCount = await SaveChangesAsync();
            
            return resultCount == 1;
        }

        public async Task<bool> EditMovieAsync(Movie movie)
        {
            DbContext.Movies.Update(movie);
            int resultCount = await SaveChangesAsync();

            return resultCount == 1;
        }

        public async Task<bool> SoftDeleteMovieAsync(Movie movie)
        {
            movie.IsDeleted = true;
            DbContext.Movies.Update(movie);

            int resultCount = await SaveChangesAsync();

            return resultCount == 1;
        }

        public async Task<bool> HardDeleteMovieAsync(Movie movie)
        {
            DbContext.Movies.Remove(movie);
            int resultCount = await SaveChangesAsync();

            return resultCount == 1;
        }

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            return await DbContext
                .Movies
                .AnyAsync(m => m.Id == id);
        }

        private async Task<int> SaveChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }
    }
}
