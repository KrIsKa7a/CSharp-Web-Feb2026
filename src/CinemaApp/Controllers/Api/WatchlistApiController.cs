namespace CinemaApp.Web.Controllers.Api
{
    using Microsoft.AspNetCore.Mvc;

    using System.Security.Claims;
    using Services.Core.Contracts;

    [Route("api/[controller]")]
    [ApiController]
    public class WatchlistApiController : ControllerBase
    {
        private readonly IWatchlistService watchlistService;

        public WatchlistApiController(IWatchlistService watchlistService)
        {
            this.watchlistService = watchlistService;
        }
        
        [ProducesResponseType<bool>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("[action]/{movieId}")]
        public async Task<ActionResult<bool>> IsInWatchlist(Guid? movieId)
        {
            if (movieId == null)
            {
                return BadRequest();
            }

            string? userId = GetUserId();
            if (string.IsNullOrWhiteSpace(userId))
            {
                /* Return true to hide the "Add to Watchlist" button for unauthenticated users */
                return true;
            }

            bool isInWatchlist = await watchlistService
                .IsMovieInUserWatchlistAsync(userId, movieId.Value);

            return isInWatchlist;
        }

        private string? GetUserId()
            => User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
