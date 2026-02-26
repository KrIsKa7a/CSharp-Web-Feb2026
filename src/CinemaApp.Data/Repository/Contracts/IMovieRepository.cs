namespace CinemaApp.Data.Repository.Contracts
{
    using Models;

    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllMoviesNoTrackingWithProjectionAsync(Func<Movie, Movie>? projectFunc = null);

        Task<IEnumerable<Movie>> GetAllMoviesAsync();

        Task<Movie?> GetMovieByIdAsync(Guid id);

		Task<bool> AddMovieAsync(Movie movie);

        Task<bool> EditMovieAsync(Movie movie);

        Task<bool> ExistsByIdAsync(Guid id);
    }
}
