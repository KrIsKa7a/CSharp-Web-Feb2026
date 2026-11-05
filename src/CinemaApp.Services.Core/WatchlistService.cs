namespace CinemaApp.Services.Core
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using CinemaApp.Data.Models;
    using CinemaApp.Data.Repository.Contracts;
    using Contracts;
    using Data.Watchlist.Enums;
    using Data.Watchlist.Response;

    public class WatchlistService : IWatchlistService
    {
        private readonly IUserMovieRepository userMovieRepository;
        private readonly IMovieRepository movieRepository;

        private readonly UserManager<IdentityUser> userManager;

        public WatchlistService(IUserMovieRepository userMovieRepository, 
            IMovieRepository movieRepository, UserManager<IdentityUser> userManager)
        {
            this.userMovieRepository = userMovieRepository;
            this.movieRepository = movieRepository;
            this.userManager = userManager;
        }

        public async Task<UserFullWatchlistResponseData> GetUserFullWatchlistDataAsync(string userId)
        {
            IEnumerable<WatchlistMovieDataModel> userWatchlistMoviesData = await userMovieRepository
                .AllAsNoTracking()
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
            bool isMovieInWatchlistResponse = await userMovieRepository
                .AllAsNoTracking()
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
            bool movieExists = await movieRepository
                .AllAsNoTracking()
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

            bool watchlistEntryExists = await userMovieRepository
                .AllAsNoTracking()
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

                    await userMovieRepository.AddAsync(userMovie);
                    await userMovieRepository.SaveChangesAsync();

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
            bool movieExists = await movieRepository
                .AllAsNoTracking()
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

            UserMovie? dbWatchlistEntry = await userMovieRepository
                .All()
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
                    userMovieRepository.Remove(dbWatchlistEntry);
                    await userMovieRepository.SaveChangesAsync();

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
