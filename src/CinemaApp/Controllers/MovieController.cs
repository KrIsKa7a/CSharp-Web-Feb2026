namespace CinemaApp.Web.Controllers
{
    using System.Globalization;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Contracts;
    using Services.Core.Data.Movie.Enums;
    using Services.Core.Data.Movie.Request;
    using Services.Core.Data.Movie.Response;
    using ViewModels.Movie;
    using static GCommon.ApplicationConstants;
    using static GCommon.ErrorMessages;
    using static GCommon.ErrorMessages.Movie;

    public class MovieController : BaseController
    {
        private readonly IMovieService movieService;
        private readonly IWatchlistService watchlistService;
        private readonly UserManager<IdentityUser> userManager;

        public MovieController(IMovieService movieService, IWatchlistService watchlistService,
            UserManager<IdentityUser> userManager)
        {
            this.movieService = movieService;
            this.watchlistService = watchlistService;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index(MovieIndexViewModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            int totalMoviesCount = await 
                movieService.GetMovieCountAsync();
            int totalPagesCount = (int)Math.Ceiling((double)totalMoviesCount / EntitiesPerPage);
            if (inputModel.PageNumber > totalPagesCount)
            {
                return BadRequest();
            }

            GetAllMoviesRequestData requestData = new GetAllMoviesRequestData()
            {
                SearchQuery = inputModel.SearchQuery,
                GenreQuery = inputModel.GenreQuery,
                ReleaseYearQuery = inputModel.ReleaseYearQuery,
                SkipEntries = (inputModel.PageNumber - 1) * EntitiesPerPage,
                TakeEntries = EntitiesPerPage
            };
            IEnumerable<GetAllMoviesResponseModel> allMoviesResponseData = await movieService
                .GetAllMoviesAsync(requestData);

            bool hasFiltering = !(string.IsNullOrWhiteSpace(inputModel.SearchQuery) &&
                                string.IsNullOrWhiteSpace(inputModel.GenreQuery) &&
                                !inputModel.ReleaseYearQuery.HasValue);

            IEnumerable<MoviesIndexEntryViewModel> allMoviesViewModel = allMoviesResponseData
                .Select(m => new MoviesIndexEntryViewModel()
                {
                    Id = m.Id.ToString(),
                    Title = m.Title,
                    Genre = m.Genre,
                    Director = m.Director,
                    ReleaseDate = m.ReleaseDate.ToString(MovieReleaseDateFormat, CultureInfo.InvariantCulture),
                    ImageUrl = m.ImageUrl ?? DefaultImageUrl
                })
                .ToArray();

            MovieIndexViewModel indexViewModel = new MovieIndexViewModel()
            {
                SearchQuery = inputModel.SearchQuery,
                GenreQuery = inputModel.GenreQuery,
                ReleaseYearQuery = inputModel.ReleaseYearQuery,
                CurrentPage = inputModel.CurrentPage,
                TotalPages = totalPagesCount,
                PageNumber = inputModel.PageNumber,
            };
            if (hasFiltering)
            {
                totalMoviesCount = await movieService
                    .GetMovieCountAsync(inputModel.SearchQuery, inputModel.GenreQuery, inputModel.ReleaseYearQuery);
                
                /* Reset pagination */
                indexViewModel.TotalPages = (int)Math.Ceiling((double)totalMoviesCount / EntitiesPerPage);
            }

            if (IsUserAuthenticated)
            {
                IdentityUser? user = await userManager.FindByIdAsync(GetUserId()!);
                if (user == null)
                {
                    return BadRequest();
                }

                indexViewModel.IsUserManager = await userManager.IsInRoleAsync(user, ManagerRoleName);

                foreach (MoviesIndexEntryViewModel movie in allMoviesViewModel)
                {
                    movie.IsInUserWatchlist = await watchlistService
                        .IsMovieInUserWatchlistAsync(GetUserId()!, Guid.Parse(movie.Id));
                }
            }

            indexViewModel.Movies = allMoviesViewModel;

            return View(indexViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] MovieFormInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            bool isReleaseDateValid = DateTime.TryParseExact(inputModel.ReleaseDate,
                MovieReleaseDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime releaseDate);
            if (!isReleaseDateValid)
            {
                ModelState.AddModelError(nameof(MovieFormInputModel.ReleaseDate), ReleaseDateFormatMessage);
                return View(inputModel);
            }

            CreateMovieRequestData createMovieRequest = new CreateMovieRequestData()
            {
                Title = inputModel.Title,
                Genre = inputModel.Genre,
                ReleaseDate = releaseDate,
                Duration = inputModel.Duration,
                Director = inputModel.Director,
                Description = inputModel.Description,
                ImageUrl = inputModel.ImageUrl
            };

            MovieServiceStatusResponseModel opStatusResponse = await movieService
                .CreateMovieAsync(createMovieRequest);
            if (opStatusResponse.Status == MovieServiceResponseStatus.GeneralError)
            {
                ModelState.AddModelError(string.Empty, GeneralErrorMessage);
                return View(inputModel);
            }

            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details([FromRoute] Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            MovieExtendedDataResponseModel? movieExtendedData = await movieService
                .GetMovieExtendedDataByIdAsync(id.Value);
            if (movieExtendedData == null)
            {
                return NotFound();
            }

            MovieDetailsViewModel movieDetailsViewModel = new MovieDetailsViewModel()
            {
                Id = movieExtendedData.Id.ToString(),
                Title = movieExtendedData.Title,
                Genre = movieExtendedData.Genre,
                Director = movieExtendedData.Director,
                ReleaseDate = movieExtendedData.ReleaseDate.ToString(MovieReleaseDateFormat),
                Description = movieExtendedData.Description,
                Duration = movieExtendedData.Duration,
                ImageUrl = movieExtendedData.ImageUrl ?? DefaultImageUrl
            };

            return View(movieDetailsViewModel);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> DetailsPartial([FromRoute] Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            MovieExtendedDataResponseModel? movieExtendedData = await movieService
                .GetMovieExtendedDataByIdAsync(id.Value);
            if (movieExtendedData == null)
            {
                return NotFound();
            }

            MovieDetailsViewModel movieDetailsViewModel = new MovieDetailsViewModel()
            {
                Id = movieExtendedData.Id.ToString(),
                Title = movieExtendedData.Title,
                Genre = movieExtendedData.Genre,
                Director = movieExtendedData.Director,
                ReleaseDate = movieExtendedData.ReleaseDate.ToString(MovieReleaseDateFormat),
                Description = movieExtendedData.Description,
                Duration = movieExtendedData.Duration,
                ImageUrl = movieExtendedData.ImageUrl ?? DefaultImageUrl
            };

            return PartialView("_MovieDetailsPartial", movieDetailsViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            MovieExtendedDataResponseModel? movieExtendedDataResponse = await movieService
                .GetMovieExtendedDataByIdAsync(id.Value);
            if (movieExtendedDataResponse == null)
            {
                return NotFound();
            }

            MovieFormInputModel movieFormModel = new MovieFormInputModel()
            {
                Title = movieExtendedDataResponse.Title,
                Genre = movieExtendedDataResponse.Genre,
                Director = movieExtendedDataResponse.Director,
                Description = movieExtendedDataResponse.Description,
                Duration = movieExtendedDataResponse.Duration,
                ReleaseDate = movieExtendedDataResponse.ReleaseDate.ToString(MovieReleaseDateFormat),
                ImageUrl = movieExtendedDataResponse.ImageUrl
            };

            return View(movieFormModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] Guid? id, [FromForm] MovieFormInputModel inputModel)
        {
            if (id == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            bool isReleaseDateValid = DateTime.TryParseExact(inputModel.ReleaseDate,
                MovieReleaseDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime releaseDate);
            if (!isReleaseDateValid)
            {
                ModelState.AddModelError(nameof(MovieFormInputModel.ReleaseDate), ReleaseDateFormatMessage);
                return View(inputModel);
            }

            EditMovieRequestData editMovieRequestData = new EditMovieRequestData()
            {
                Id = id.Value,
                Title = inputModel.Title,
                Genre = inputModel.Genre,
                ReleaseDate = releaseDate,
                Duration = inputModel.Duration,
                Director = inputModel.Director,
                Description = inputModel.Description,
                ImageUrl = inputModel.ImageUrl
            };

            MovieServiceStatusResponseModel responseData = await movieService
                .EditMovieAsync(editMovieRequestData);

            if (responseData.Status == MovieServiceResponseStatus.InvalidRequest)
            {
                return BadRequest();
            }

            if (responseData.Status == MovieServiceResponseStatus.NotFound)
            {
                return NotFound();
            }

            if (responseData.Status == MovieServiceResponseStatus.GeneralError)
            {
                ModelState.AddModelError(string.Empty, GeneralErrorMessage);
                return View(inputModel);
            }

            return RedirectToAction(nameof(Details), new { id = id.Value.ToString() });
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            MovieBasicDataResponseModel? movieBasicData = await movieService
                .GetMovieBasicDataByIdAsync(id.Value);
            if (movieBasicData == null)
            {
                return NotFound();
            }

            MovieDeleteViewModel movieDeleteViewModel = new MovieDeleteViewModel()
            {
                Id = movieBasicData.Id.ToString(),
                Title = movieBasicData.Title,
                ImageUrl = movieBasicData.ImageUrl ?? DefaultImageUrl
            };

            return View(movieDeleteViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] Guid? id, [FromForm] MovieDeleteViewModel inputModel)
        {
            if (id == null)
            {
                return BadRequest();
            }

            MovieServiceStatusResponseModel deleteStatus = await movieService
                .SoftDeleteMovieAsync(id.Value);
            if (deleteStatus.Status == MovieServiceResponseStatus.NotFound)
            {
                return NotFound();
            }

            if (deleteStatus.Status == MovieServiceResponseStatus.GeneralError)
            {
                ModelState.AddModelError(string.Empty, GeneralErrorMessage);
                return View(inputModel);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
