namespace CinemaApp.Services.Core.Contracts
{
    using Data.Movie.Response;

    public interface IMovieService
    {
        Task<IEnumerable<GetAllMoviesResponseModel>> GetAllMoviesAsync();
    }
}
