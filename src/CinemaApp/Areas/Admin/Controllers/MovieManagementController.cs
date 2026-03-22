namespace CinemaApp.Web.Areas.Admin.Controllers
{
    using GCommon.Exceptions;
    using Services.Core.Contracts;
    using Services.Models.Movie;
    using ViewModels.Admin.Movie;
    using static GCommon.ApplicationConstants;
    using static GCommon.OutputMessages.Movie;

    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

    public class MovieManagementController : BaseAdminController
    {
        private readonly IMovieService movieService;
        private readonly IMapper mapper;

        private readonly ILogger<MovieManagementController> logger;

        public MovieManagementController(IMovieService movieService, IMapper mapper, 
            ILogger<MovieManagementController> logger)
        {
            this.movieService = movieService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            IEnumerable<MovieAllDto> movieAllDtos = await movieService
                .GetAllMoviesOrderedByTitleAsync(moviesPerPage: null, includeDeleted: true);
            IEnumerable<AdminManageMovieViewModel> manageMovieVms = mapper
                .Map<IEnumerable<AdminManageMovieViewModel>>(movieAllDtos);

            return View(manageMovieVms);
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

                return View(formModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, UnexpectedErrorMessage);
                TempData[ErrorTempDataKey] = UnexpectedErrorMessage;

                return View(formModel);
            }
            
            return RedirectToAction(nameof(Manage));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            MovieDetailsDto? movieDetailsDto = await movieService
                .GetMovieFormModelByIdAsync(id: id, includeDeleted: true);
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
                await movieService.EditMovieAsync(id: id, 
                    movieDetailsDto: movieDetailsDto, includeDeleted: true);
            }
            catch (EntityNotFoundException enfe)
            {
                return NotFound();
            }
            catch (EntityPersistFailureException epfe)
            {
                logger.LogError(epfe, string.Format(CrudMovieFailureMessage, nameof(Edit)));
                TempData[ErrorTempDataKey] = string.Format(CrudMovieFailureMessage, "editing");

                return View(formModel);
            }

            return RedirectToAction(nameof(Manage));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleDelete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            try
            {
                MovieDetailsDto? movieDetailsDto = await movieService
                    .GetMovieFormModelByIdAsync(id, includeDeleted: true);
                if (movieDetailsDto == null)
                {
                    return NotFound();
                }

                movieDetailsDto.IsDeleted = !movieDetailsDto.IsDeleted;

                await movieService.EditMovieAsync(id: id,
                    movieDetailsDto: movieDetailsDto, includeDeleted: true);
            }
            catch (EntityNotFoundException enfe)
            {
                return NotFound();
            }
            catch (EntityPersistFailureException epfe)
            {
                logger.LogError(epfe, string.Format(CrudMovieFailureMessage, nameof(ToggleDelete)));
                TempData[ErrorTempDataKey] = string.Format(CrudMovieFailureMessage, "deleting");

                return RedirectToAction(nameof(Manage));
            }

            return RedirectToAction(nameof(Manage));
        }
    }
}
