namespace CinemaApp.Web.Controllers
{
    using System.Globalization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Contracts;
    using Services.Core.Data.Movie.Response;
    using ViewModels.Movie;
    using static GCommon.ApplicationConstants;

    public class MovieController : Controller
    {
        private readonly IMovieService movieService;

        public MovieController(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<GetAllMoviesResponseModel> allMoviesResponseData = await movieService
                .GetAllMoviesAsync();
            IEnumerable<AllMoviesIndexViewModel> allMoviesViewModel = allMoviesResponseData
                .Select(m => new AllMoviesIndexViewModel()
                {
                    Id = m.Id.ToString(),
                    Title = m.Title,
                    Genre = m.Genre,
                    Director = m.Director,
                    ReleaseDate = m.ReleaseDate.ToString(MovieReleaseDateFormat, CultureInfo.InvariantCulture),
                    ImageUrl = m.ImageUrl ?? DefaultImageUrl
                });

            return View(allMoviesViewModel);
        }
    }
}
