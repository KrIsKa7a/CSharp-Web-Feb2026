namespace CinemaApp.Web.Controllers
{
    using GCommon.Exceptions;
    using Services.Core.Contracts;
    using Services.Models.Watchlist;
    using ViewModels.Watchlist;
    using static GCommon.OutputMessages.Watchlist;

    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;

    public class WatchlistController : BaseController
    {
        private readonly IWatchlistService watchlistService;
        
        private readonly IMapper mapper;
        private readonly ILogger<WatchlistController> logger;

        public WatchlistController(IWatchlistService watchlistService, IMapper mapper, ILogger<WatchlistController> logger)
        {
            this.watchlistService = watchlistService;

            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string userId = GetUserId()!;

            IEnumerable<WatchlistMovieDto> watchlistMovieDtos = await watchlistService
                .GetUserWatchlistByIdAsync(userId);
            IEnumerable<WatchlistMovieViewModel> watchlistMovieViewModels = mapper
                .Map<IEnumerable<WatchlistMovieViewModel>>(watchlistMovieDtos);

            return View(watchlistMovieViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Add([FromRoute(Name = "id")] Guid movieId)
        {
            string userId = GetUserId()!;

            try
            {
                await watchlistService.AddMovieToUserWatchlistAsync(userId, movieId);
            }
            catch (EntityAlreadyExistsException eaee)
            {
                logger.LogError(eaee, string.Format(MovieAlreadyInWatchlistMessage, movieId, userId));

                return BadRequest();
            }
            catch (EntityNotFoundException enfe)
            {
                return NotFound();
            }
            catch (EntityPersistFailureException epfe)
            {
                logger.LogError(epfe, AddToWatchlistFailureMessage);

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
