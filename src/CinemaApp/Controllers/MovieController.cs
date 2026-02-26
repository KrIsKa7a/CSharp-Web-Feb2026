namespace CinemaApp.Web.Controllers
{
    using GCommon.Exceptions;
    using Services.Core.Contracts;
    using ViewModels.Movie;
    using static GCommon.OutputMessages.Movie;
    using static GCommon.ApplicationConstants;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class MovieController : BaseController
    {
        private readonly IMovieService movieService;

        private readonly ILogger<MovieController> logger;

        public MovieController(IMovieService movieService, ILogger<MovieController> logger)
        {
            this.movieService = movieService;
            this.logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AllMoviesIndexViewModel> allMoviesViewModel = await movieService
                .GetAllMoviesOrderedByTitleAsync();

            return View(allMoviesViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MovieFormModel formModel)
        {
            if (!ModelState.IsValid)
            {
                return View(formModel);
            }

            try
            {
                await movieService.CreateMovieAsync(formModel);
            }
            catch (EntityCreatePersistFailureException ecpfe)
            {
                logger.LogError(ecpfe, CreateMovieFailureMessage);
                ModelState.AddModelError(string.Empty, CreateMovieFailureMessage);

                return View(formModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, UnexpectedErrorMessage);
                ModelState.AddModelError(string.Empty, UnexpectedErrorMessage);

                // TODO: Redirect after implementing JS Notifications
                return View(formModel);
            }

            // TODO: Redirect to Manage after implementing Roles
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id)
        {
	        MovieDetailsViewModel? movieDetailsVm = await movieService
		        .GetMovieDetailsByIdAsync(id);
	        if (movieDetailsVm == null)
	        {
                return NotFound();
			}

	        return View(movieDetailsVm);
        }
    }
}
