namespace CinemaApp.Services.Core.Contracts
{
    using Models.Movie;
    using static GCommon.ApplicationConstants;

    public interface IMovieService
    {
        Task<IEnumerable<MovieAllDto>> GetAllMoviesOrderedByTitleAsync(string? userId = null, string? searchQuery = null, int pageNumber = 1, int? moviesPerPage = DefaultEntitiesPerPage, bool includeDeleted = false);

        Task<int> GetMoviesCountAsync(string? searchQuery = null);
        
        Task CreateMovieAsync(MovieDetailsDto movieDetailsDto);

        Task<MovieDetailsDto?> GetMovieDetailsByIdAsync(Guid id);

        Task<MovieDetailsDto?> GetMovieFormModelByIdAsync(Guid id, bool includeDeleted = false);

        Task<bool> ExistsByIdAsync(Guid id);

        Task EditMovieAsync(Guid id, MovieDetailsDto movieDetailsDto, bool includeDeleted = false);

        Task SoftDeleteMovieAsync(Guid id);

        Task HardDeleteMovieAsync(Guid id);
    }
}
