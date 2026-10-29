namespace CinemaApp.Services.Core.Contracts
{
    using Data.Movie.Request;
    using Data.Movie.Response;

    public interface IMovieService
    {
        Task<IEnumerable<GetAllMoviesResponseModel>> GetAllMoviesAsync();

        Task<MovieServiceStatusResponseModel> CreateMovieAsync(CreateMovieRequestData requestData);
    }
}
