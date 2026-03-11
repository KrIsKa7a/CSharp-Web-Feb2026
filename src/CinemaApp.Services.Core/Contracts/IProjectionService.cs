namespace CinemaApp.Services.Core.Contracts
{
    public interface IProjectionService
    {
        Task<IEnumerable<DateTime>> GetProjectionAvailableShowtimesAsync(Guid movieId, Guid cinemaId);

        Task<Guid?> GetProjectionIdByMovieCinemaAndShowtimeAsync(Guid movieId, Guid cinemaId, string showtime);
    }
}
