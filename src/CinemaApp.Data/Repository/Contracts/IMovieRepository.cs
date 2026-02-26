namespace CinemaApp.Data.Repository.Contracts
{
    using Models;

    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllMoviesNoTrackingWithProjectionAsync(Func<Movie, Movie>? projectFunc = null);

        Task<IEnumerable<Movie>> GetAllMoviesAsync();

        Task<bool> AddMovieAsync(Movie movie);
    }
}
