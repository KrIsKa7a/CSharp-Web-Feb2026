namespace CinemaApp.Data.Repository.Contracts
{
    using Models;

    public interface IWatchlistRepository
    {
        Task<IEnumerable<UserMovie>> GetAllUserMoviesAsync();

        Task<bool> ExistsAsync(string userId, Guid movieId);

        Task<bool> AddUserMovieAsync(UserMovie userMovie);
    }
}
