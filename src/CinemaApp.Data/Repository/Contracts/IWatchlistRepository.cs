namespace CinemaApp.Data.Repository.Contracts
{
    using Models;

    public interface IWatchlistRepository
    {
        Task<IEnumerable<UserMovie>> GetAllUserMoviesAsync();
    }
}
