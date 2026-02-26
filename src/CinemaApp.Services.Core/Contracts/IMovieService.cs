namespace CinemaApp.Services.Core.Contracts
{
    using Models.Movie;
    using Web.ViewModels.Movie;

    public interface IMovieService
    {
        Task<IEnumerable<MovieAllDto>> GetAllMoviesOrderedByTitleAsync();

        // TODO: Service to be refactored to work without coupling to ViewModels
        Task CreateMovieAsync(MovieFormModel movieFormModel);

        Task<MovieDetailsViewModel?> GetMovieDetailsByIdAsync(Guid id);

        Task<MovieFormModel?> GetMovieFormModelByIdAsync(Guid id);

        Task<bool> ExistsByIdAsync(Guid id);

        Task EditMovieAsync(Guid id, MovieFormModel movieFormModel);

        Task SoftDeleteMovieAsync(Guid id);

        Task HardDeleteMovieAsync(Guid id);
    }
}
