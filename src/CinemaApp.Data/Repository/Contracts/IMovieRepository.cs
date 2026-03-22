namespace CinemaApp.Data.Repository.Contracts
{
    using System.Linq.Expressions;

    using Models;

    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllMoviesNoTrackingWithProjectionAsync(Expression<Func<Movie, Movie>>? projectionQuery = null, Expression<Func<Movie, bool>>? filterQuery = null, int? skipCnt = null, int? takeCnt = null, bool ignoreQueryFilters = false);

        Task<IEnumerable<Movie>> GetAllMoviesAsync();

        Task<Movie?> GetMovieByIdAsync(Guid id, bool ignoreQueryFilters = false);

		Task<bool> AddMovieAsync(Movie movie);

        Task<bool> EditMovieAsync(Movie movie);

        Task<bool> SoftDeleteMovieAsync(Movie movie);

        Task<bool> HardDeleteMovieAsync(Movie movie);

        Task<bool> ExistsByIdAsync(Guid id);

        Task<int> CountAsync(Expression<Func<Movie, bool>>? filterQuery);
    }
}
