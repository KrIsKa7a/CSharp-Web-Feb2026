namespace CinemaApp.Web.Controllers
{
    using GCommon.Exceptions;
    using Services.Core.Contracts;
    using Services.Models.Movie;
    using ViewModels.Movie;
    using static GCommon.ApplicationConstants;
    using static GCommon.OutputMessages.Movie;

    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

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
            string? userId = GetUserId();
            IEnumerable<MovieAllDto> movieAllDtos = await movieService
                .GetAllMoviesOrderedByTitleAsync(userId);
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
                MovieDetailsDto movieDetailsDto = mapper
                    .Map<MovieDetailsDto>(formModel);

                await movieService.CreateMovieAsync(movieDetailsDto);
            }
            catch (EntityPersistFailureException ecpfe)
            {
                logger.LogError(ecpfe, string.Format(CrudMovieFailureMessage, nameof(Create)));
                TempData[ErrorTempDataKey] = string.Format(CrudMovieFailureMessage, "creating");

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, UnexpectedErrorMessage);
                TempData[ErrorTempDataKey] = UnexpectedErrorMessage;

                // TODO: Redirect after implementing JS Notifications
                return RedirectToAction(nameof(Index));
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

            MovieDetailsDto? movieDetailsDto = await movieService
		        .GetMovieDetailsByIdAsync(id);
	        if (movieDetailsDto == null)
	        {
                return NotFound();
			}

            MovieDetailsViewModel movieDetailsVm = mapper
                .Map<MovieDetailsViewModel>(movieDetailsDto);

            return View(movieDetailsVm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            MovieDetailsDto? movieDetailsDto = await movieService
                .GetMovieFormModelByIdAsync(id);
            if (movieDetailsDto == null)
            {
                return NotFound();
            }

            MovieFormModel movieFormModel = mapper
                .Map<MovieFormModel>(movieDetailsDto);
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
                MovieDetailsDto movieDetailsDto = mapper
                    .Map<MovieDetailsDto>(formModel);
                await movieService.EditMovieAsync(id, movieDetailsDto);
            }
            catch (EntityNotFoundException enfe)
            {
                return NotFound();
            }
            catch (EntityPersistFailureException epfe)
            {
                logger.LogError(epfe, string.Format(CrudMovieFailureMessage, nameof(Edit)));
                TempData[ErrorTempDataKey] = string.Format(CrudMovieFailureMessage, "editing");

                return RedirectToAction(nameof(Index));
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

            MovieDetailsDto? movieDetailsDto = await movieService
                .GetMovieDetailsByIdAsync(id);
            if (movieDetailsDto == null)
            {
                return NotFound();
            }

            MovieDeleteViewModel movieDeleteVm = mapper
                .Map<MovieDeleteViewModel>(movieDetailsDto);
            return View(movieDeleteVm);
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
                TempData[ErrorTempDataKey] = string.Format(CrudMovieFailureMessage, "deleting");

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
