namespace CinemaApp.Data.Repository.Contracts
{
    using Models;

    public interface IMovieRepository
    {
        IQueryable<Movie> GetAllMoviesNoTracking();

        Task<IEnumerable<Movie>> GetAllMovies();
    }
}
