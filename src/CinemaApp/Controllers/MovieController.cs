namespace CinemaApp.Web.Controllers
{
    using Services.Core.Contracts;
    using Services.Models.Movie;
    using ViewModels.Movie;
    using static GCommon.ApplicationConstants;

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
        public async Task<IActionResult> Index(MoviesAllIndexViewModel movieAllInputModel)
        {
            string? userId = GetUserId();
            IEnumerable<MovieAllDto> movieAllDtos = await movieService
                .GetAllMoviesOrderedByTitleAsync(userId, movieAllInputModel.SearchQuery, movieAllInputModel.PageNumber);
            int moviesTotalCnt = await movieService
                .GetMoviesCountAsync(movieAllInputModel.SearchQuery);

            IEnumerable<MovieIndexViewModel> allMoviesIndexVms = mapper
                .Map<IEnumerable<MovieIndexViewModel>>(movieAllDtos);

            MoviesAllIndexViewModel movieAllVm = new MoviesAllIndexViewModel()
            {
                SearchQuery = movieAllInputModel.SearchQuery,
                PageNumber = movieAllInputModel.PageNumber,
                TotalPages = (int)Math.Ceiling(moviesTotalCnt / (double)DefaultEntitiesPerPage),
                ShowingPages = movieAllInputModel.ShowingPages,
                StartPageIndex = (movieAllInputModel.PageNumber / 10) * 10,
                Movies = allMoviesIndexVms.ToArray(),
            };

            if (movieAllVm.PageNumber > movieAllVm.TotalPages && movieAllVm.TotalPages != 0)
            {
                return RedirectToAction(nameof(Index), new MoviesAllIndexViewModel()
                {
                    SearchQuery = movieAllInputModel.SearchQuery,
                    PageNumber = movieAllVm.TotalPages,
                    ShowingPages = movieAllInputModel.ShowingPages,
                });
            }

            return View(movieAllVm);
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
    }
}
