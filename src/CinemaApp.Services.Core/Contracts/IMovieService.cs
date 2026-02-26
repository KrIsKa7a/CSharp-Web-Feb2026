namespace CinemaApp.Services.Core.Contracts
{
    using Web.ViewModels.Movie;

    public interface IMovieService
    {
        Task<IEnumerable<AllMoviesIndexViewModel>> GetAllMoviesOrderedByTitleAsync();

        // TODO: Service to be refactored to work without coupling to ViewModels
        Task CreateMovieAsync(MovieFormModel movieFormModel);

        Task<MovieDetailsViewModel?> GetMovieDetailsByIdAsync(Guid id);

        Task<MovieFormModel?> GetMovieFormModelByIdAsync(Guid id);

        Task<bool> ExistsByIdAsync(Guid id);

        Task EditMovieAsync(Guid id, MovieFormModel movieFormModel);
    }
}
