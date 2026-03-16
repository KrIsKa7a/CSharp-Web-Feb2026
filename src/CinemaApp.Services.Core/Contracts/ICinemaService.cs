namespace CinemaApp.Services.Core.Contracts
{
    using Models.Cinema;

    public interface ICinemaService
    {
        Task<IEnumerable<CinemaAllDto>> GetAllCinemasOrderedByLocationAsync();

        Task<CinemaProgramDetailsDto?> GetCinemaProgramByIdAsync(Guid cinemaId);
    }
}
