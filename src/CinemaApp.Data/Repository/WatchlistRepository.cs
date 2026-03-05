namespace CinemaApp.Data.Repository
{
    using Contracts;
    using Models;

    using Microsoft.EntityFrameworkCore;

    public class WatchlistRepository : BaseRepository, IWatchlistRepository
    {
        public WatchlistRepository(CinemaAppDbContext dbContext)
            : base(dbContext)
        {
            
        }

        public async Task<IEnumerable<UserMovie>> GetAllUserMoviesAsync()
        {
            IEnumerable<UserMovie> userMovies = await DbContext
                .UsersMovies
                .AsNoTracking()
                .Include(um => um.Movie)
                .ToArrayAsync();

            return userMovies;
        }

        public async Task<UserMovie?> GetUserMovieAsync(string userId, Guid movieId)
        {
            UserMovie? userMovie = await DbContext
                .UsersMovies
                .SingleOrDefaultAsync(um => um.UserId.ToLower() == userId.ToLower() &&
                                            um.MovieId == movieId);

            return userMovie;
        }

        public async Task<UserMovie?> GetUserMovieIncludeDeletedAsync(string userId, Guid movieId)
        {
            UserMovie? userMovie = await DbContext
                .UsersMovies
                .IgnoreQueryFilters()
                .SingleOrDefaultAsync(um => um.UserId.ToLower() == userId.ToLower() &&
                                            um.MovieId == movieId);

            return userMovie;
        }

        public async Task<bool> ExistsAsync(string userId, Guid movieId)
        {
            bool watchListEntryExist = await DbContext
                .UsersMovies
                
                .AnyAsync(um => um.UserId.ToLower() == userId.ToLower() && um.MovieId == movieId);

            return watchListEntryExist;
        }

        public async Task<bool> AddUserMovieAsync(UserMovie userMovie)
        {
            await DbContext.UsersMovies.AddAsync(userMovie);
            int resultCount = await SaveChangesAsync();

            return resultCount == 1;
        }

        public async Task<bool> UpdateUserMovieAsync(UserMovie userMovie)
        {
            DbContext.UsersMovies.Update(userMovie);

            int resultCount = await SaveChangesAsync();

            return resultCount == 1;
        }

        public async Task<bool> SoftDeleteUserMovieAsync(UserMovie userMovie)
        {
            userMovie.IsDeleted = true;
            DbContext.UsersMovies.Update(userMovie);

            int resultCount = await SaveChangesAsync();

            return resultCount == 1;
        }
    }
}
