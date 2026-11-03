namespace CinemaApp.Services.Core.Contracts
{
    using Data.Movie.Request;
    using Data.Movie.Response;

    public interface IMovieService
    {
        Task<IEnumerable<GetAllMoviesResponseModel>> GetAllMoviesAsync();

        Task<MovieServiceStatusResponseModel> CreateMovieAsync(CreateMovieRequestData requestData);

        Task<MovieExtendedDataResponseModel?> GetMovieExtendedDataByIdAsync(Guid id);

        Task<MovieServiceStatusResponseModel> EditMovieAsync(EditMovieRequestData requestData);
    }
}
