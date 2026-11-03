namespace CinemaApp.Services.Core.Contracts
{
    using Data.Movie.Request;
    using Data.Movie.Response;

    public interface IMovieService
    {
        Task<IEnumerable<GetAllMoviesResponseModel>> GetAllMoviesAsync();

        Task<MovieServiceStatusResponseModel> CreateMovieAsync(CreateMovieRequestData requestData);

        Task<MovieBasicDataResponseModel?> GetMovieBasicDataByIdAsync(Guid id);

        Task<MovieExtendedDataResponseModel?> GetMovieExtendedDataByIdAsync(Guid id);

        Task<MovieServiceStatusResponseModel> EditMovieAsync(EditMovieRequestData requestData);

        Task<MovieServiceStatusResponseModel> SoftDeleteMovieAsync(Guid id);

        Task<MovieServiceStatusResponseModel> HardDeleteMovieAsync(Guid id);
    }
}
