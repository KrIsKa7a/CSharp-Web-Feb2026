namespace CinemaApp.Services.Core.Contracts
{
    using Models.Watchlist;

    public interface IWatchlistService
    {
        Task<IEnumerable<WatchlistMovieDto>> GetUserWatchlistByIdAsync(string userId);
    }
}
