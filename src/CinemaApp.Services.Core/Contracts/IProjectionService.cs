namespace CinemaApp.Services.Core.Contracts
{
    using Data.Projection.Response;

    public interface IProjectionService
    {
        Task<IEnumerable<GetProjectionDatesResponseModel>> GetProjectionDatesAsync(Guid cinemaId, Guid movieId);

        Task<int> GetProjectionAvailabilityAsync(Guid projectionId);
    }
}
