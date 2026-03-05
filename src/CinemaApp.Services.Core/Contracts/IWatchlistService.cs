namespace CinemaApp.Services.Core.Contracts
{
    using Models.Watchlist;

    public interface IWatchlistService
    {
        Task<IEnumerable<WatchlistMovieDto>> GetUserWatchlistByIdAsync(string userId);

        Task AddMovieToUserWatchlistAsync(string userId, Guid movieId);

        Task<bool> MovieIsInUserWatchlistAsync(string userId, Guid movieId);
    }
}
