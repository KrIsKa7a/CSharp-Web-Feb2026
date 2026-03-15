namespace CinemaApp.Services.Core.Contracts
{
    using Models.Projection;

    public interface IProjectionService
    {
        Task<IEnumerable<ProjectionShowtimeDto>> GetProjectionAvailableShowtimesAsync(Guid movieId, Guid cinemaId);
    }
}
