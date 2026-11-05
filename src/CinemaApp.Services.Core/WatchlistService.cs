namespace CinemaApp.Services.Core
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using CinemaApp.Data;
    using CinemaApp.Data.Models;
    using Contracts;
    using Data.Watchlist.Enums;
    using Data.Watchlist.Response;

    public class WatchlistService : IWatchlistService
    {
        private readonly CinemaAppDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;

        public WatchlistService(CinemaAppDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task<UserFullWatchlistResponseData> GetUserFullWatchlistDataAsync(string userId)
        {
            IEnumerable<WatchlistMovieDataModel> userWatchlistMoviesData = await dbContext.UsersMovies
                .Where(um => um.UserId.ToLower() == userId.ToLower())
                .Select(um => um.Movie)
                .Select(m => new WatchlistMovieDataModel()
                {
                    Id = m.Id,
                    Title = m.Title,
                    Genre = m.Genre,
                    ReleaseDate = m.ReleaseDate,
                    ImageUrl = m.ImageUrl,
                })
                .OrderBy(m => m.Title)
                .ThenBy(m => m.Genre)
                .ToArrayAsync();

            UserFullWatchlistResponseData userFullWatchlistResponseData = new UserFullWatchlistResponseData()
            {
                Movies = userWatchlistMoviesData
            };

            return userFullWatchlistResponseData;
        }

        public async Task<bool> IsMovieInUserWatchlistAsync(string userId, Guid movieId)
        {
            bool isMovieInWatchlistResponse = await dbContext.UsersMovies
                .AnyAsync(um => um.UserId.ToLower() == userId.ToLower() &&
                                um.MovieId == movieId);

            return isMovieInWatchlistResponse;
        }

        public async Task<WatchlistServiceStatusResponseModel> AddMovieToWatchlistAsync(string userId, Guid movieId)
        {
            WatchlistServiceStatusResponseModel responseData = new WatchlistServiceStatusResponseModel()
            {
                Status = WatchlistServiceResponseStatus.GeneralError,
                ErrorMessages = new List<string?>(),
            };

            IdentityUser? user = await userManager.FindByIdAsync(userId);
            bool movieExists = await dbContext.Movies
                .AnyAsync(m => m.Id == movieId);
            if (!movieExists)
            {
                responseData.Status = WatchlistServiceResponseStatus.InvalidReference;
                responseData.ErrorMessages.Add("Referenced movie does not exist!");
            }

            if (user == null)
            {
                responseData.Status = WatchlistServiceResponseStatus.InvalidReference;
                responseData.ErrorMessages.Add("Referenced user does not exist!");
            }

            bool watchlistEntryExists = await dbContext.UsersMovies
                .AnyAsync(um => um.UserId.ToLower() == userId.ToLower() &&
                                um.MovieId == movieId);
            if (watchlistEntryExists)
            {
                responseData.Status = WatchlistServiceResponseStatus.DuplicatedEntry;
            }
            else
            {
                try
                {
                    UserMovie userMovie = new UserMovie()
                    {
                        UserId = userId,
                        MovieId = movieId
                    };

                    await dbContext.UsersMovies.AddAsync(userMovie);
                    await dbContext.SaveChangesAsync();

                    responseData.Status = WatchlistServiceResponseStatus.Success;
                }
                catch (Exception ex)
                {
                    /* TODO: Log the exception details for further analysis and debugging. */
                    responseData.Status = WatchlistServiceResponseStatus.GeneralError;
                    responseData.ErrorMessages = new List<string?>() { ex.Message };
                }
            }

            return responseData;
        }

        public async Task<WatchlistServiceStatusResponseModel> RemoveMovieFromWatchlistAsync(string userId, Guid movieId)
        {
            WatchlistServiceStatusResponseModel responseData = new WatchlistServiceStatusResponseModel()
            {
                Status = WatchlistServiceResponseStatus.GeneralError,
                ErrorMessages = new List<string?>()
            };

            IdentityUser? dbUser = await userManager.FindByIdAsync(userId);
            bool movieExists = await dbContext.Movies
                .AnyAsync(m => m.Id == movieId);
            if (movieExists)
            {
                responseData.Status = WatchlistServiceResponseStatus.InvalidReference;
                responseData.ErrorMessages.Add("Referenced movie does not exist!");
            }

            if (dbUser == null)
            {
                responseData.Status = WatchlistServiceResponseStatus.InvalidReference;
                responseData.ErrorMessages.Add("Referenced user does not exist!");
            }

            UserMovie? dbWatchlistEntry = await dbContext.UsersMovies
                .SingleOrDefaultAsync(um => um.UserId.ToLower() == userId.ToLower() &&
                                um.MovieId == movieId);
            if (dbWatchlistEntry == null)
            {
                responseData.Status = WatchlistServiceResponseStatus.EntityNotFound;
            }
            else
            {
                try
                {
                    dbContext.UsersMovies.Remove(dbWatchlistEntry);
                    await dbContext.SaveChangesAsync();

                    responseData.Status = WatchlistServiceResponseStatus.Success;
                }
                catch (Exception ex)
                {
                    /* TODO: Log the exception details for further analysis and debugging. */
                    responseData.Status = WatchlistServiceResponseStatus.GeneralError;
                    responseData.ErrorMessages = new List<string?>() { ex.Message };
                }
            }

            return responseData;
        }
    }
}
