namespace CinemaApp.Web.Controllers
{
    using CinemaApp.Services.Core.Data.Watchlist.Response;

    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Contracts;
    using Services.Core.Data.Watchlist.Enums;
    using ViewModels.Watchlist;
    using static GCommon.ApplicationConstants;

    public class WatchlistController : BaseController
    {
        private readonly IWatchlistService watchlistService;

        public WatchlistController(IWatchlistService watchlistService)
        {
            this.watchlistService = watchlistService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string userId = GetUserId()!;
            UserFullWatchlistResponseData watchlistResponseData = await watchlistService
                .GetUserFullWatchlistDataAsync(userId);

            IEnumerable<WatchlistMovieViewModel> watchlistMoviesViewModel = watchlistResponseData
                .Movies
                .Select(m => new WatchlistMovieViewModel()
                {
                    Id = m.Id.ToString(),
                    Title = m.Title,
                    Genre = m.Genre,
                    ReleaseDate = m.ReleaseDate.ToString(MovieReleaseDateFormat),
                    ImageUrl = m.ImageUrl ?? DefaultImageUrl
                })
                .ToArray();

            return View(watchlistMoviesViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            string userId = GetUserId()!;
            WatchlistServiceStatusResponseModel responseData = await watchlistService
                .AddMovieToWatchlistAsync(userId, id.Value);
            if (responseData.Status == WatchlistServiceResponseStatus.InvalidReference)
            {
                return NotFound();
            }

            if (responseData.Status == WatchlistServiceResponseStatus.GeneralError)
            {
                /* TODO: Add JS notification about the error */
            }

            return RedirectToAction(nameof(Index), "Movie");
        }

        [HttpPost]
        public async Task<IActionResult> Remove([FromForm] Guid? id, [FromQuery] string? returnUrl)
        {
            if (id == null)
            {
                return BadRequest();
            }

            string userId = GetUserId()!;
            WatchlistServiceStatusResponseModel responseData = await watchlistService
                .RemoveMovieFromWatchlistAsync(userId, id.Value);
            if (responseData.Status == WatchlistServiceResponseStatus.InvalidReference)
            {
                return NotFound();
            }

            if (responseData.Status == WatchlistServiceResponseStatus.GeneralError)
            {
                /* TODO: Add JS notification about the error */
            }

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
