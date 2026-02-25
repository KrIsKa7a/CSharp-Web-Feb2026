namespace CinemaApp.Services.Core.Contracts
{
    using Web.ViewModels.Movie;

    public interface IMovieService
    {
        Task<IEnumerable<AllMoviesIndexViewModel>> GetAllMoviesOrderedByTitleAsync();
    }
}
