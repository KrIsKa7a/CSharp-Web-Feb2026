namespace CinemaApp.Web.Controllers
{
    using AutoMapper;
    using GCommon.Exceptions;
    using Services.Core.Contracts;
    using ViewModels.Movie;
    using static GCommon.OutputMessages.Movie;
    using static GCommon.ApplicationConstants;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Services.Models.Movie;

    public class MovieController : BaseController
    {
        private readonly IMovieService movieService;
        private readonly IMapper mapper;

        private readonly ILogger<MovieController> logger;

        public MovieController(IMovieService movieService, IMapper mapper, ILogger<MovieController> logger)
        {
            this.movieService = movieService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<MovieAllDto> movieAllDtos = await movieService
                .GetAllMoviesOrderedByTitleAsync();
            IEnumerable<AllMoviesIndexViewModel> allMoviesIndexVms = mapper
                .Map<IEnumerable<AllMoviesIndexViewModel>>(movieAllDtos);

            return View(allMoviesIndexVms);
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
            catch (EntityPersistFailureException ecpfe)
            {
                logger.LogError(ecpfe, string.Format(CrudMovieFailureMessage, nameof(Create)));
                ModelState.AddModelError(string.Empty, string.Format(CrudMovieFailureMessage, "creating"));

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
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            MovieDetailsViewModel? movieDetailsVm = await movieService
		        .GetMovieDetailsByIdAsync(id);
	        if (movieDetailsVm == null)
	        {
                return NotFound();
			}

	        return View(movieDetailsVm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            MovieFormModel? movieFormModel = await movieService
                .GetMovieFormModelByIdAsync(id);
            if (movieFormModel == null)
            {
                return NotFound();
            }

            return View(movieFormModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] Guid id, MovieFormModel formModel)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(formModel);
            }

            try
            {
                await movieService.EditMovieAsync(id, formModel);
            }
            catch (EntityNotFoundException enfe)
            {
                return NotFound();
            }
            catch (EntityPersistFailureException epfe)
            {
                logger.LogError(epfe, string.Format(CrudMovieFailureMessage, nameof(Edit)));
                ModelState.AddModelError(string.Empty, string.Format(CrudMovieFailureMessage, "editing"));

                return View(formModel);
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            MovieDetailsViewModel? movieDetailsVm = await movieService
                .GetMovieDetailsByIdAsync(id);
            if (movieDetailsVm == null)
            {
                return NotFound();
            }

            return View(movieDetailsVm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] Guid id, MovieDetailsViewModel? deleteDetailsVm)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            try
            {
                await movieService.SoftDeleteMovieAsync(id);
            }
            catch (EntityNotFoundException enfe)
            {
                return NotFound();
            }
            catch (EntityPersistFailureException epfe)
            {
                logger.LogError(epfe, string.Format(CrudMovieFailureMessage, nameof(Delete)));
                ModelState.AddModelError(string.Empty, string.Format(CrudMovieFailureMessage, "deleting"));

                return View(deleteDetailsVm);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
