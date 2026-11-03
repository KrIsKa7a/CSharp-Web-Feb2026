namespace CinemaApp.Services.Core.Contracts
{
    using CinemaApp.Services.Core.Data.Watchlist.Response;

    using Data.Watchlist;

    public interface IWatchlistService
    {
        Task<UserFullWatchlistResponseData> GetUserFullWatchlistDataAsync(string userId);

        Task<bool> IsMovieInUserWatchlistAsync(string userId, Guid movieId);

        Task<WatchlistServiceStatusResponseModel> AddMovieToWatchlist(string userId, Guid movieId);
    }
}
