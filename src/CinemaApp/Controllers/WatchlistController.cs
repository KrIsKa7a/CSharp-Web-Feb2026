namespace CinemaApp.Web.Controllers
{
    using Services.Core.Contracts;
    using Services.Models.Watchlist;
    using ViewModels.Watchlist;

    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;

    public class WatchlistController : BaseController
    {
        private readonly IWatchlistService watchlistService;
        private readonly IMapper mapper;

        public WatchlistController(IWatchlistService watchlistService, IMapper mapper)
        {
            this.watchlistService = watchlistService;
            this.mapper = mapper;
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
    }
}
