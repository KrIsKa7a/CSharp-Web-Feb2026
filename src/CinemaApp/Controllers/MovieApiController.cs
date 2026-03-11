namespace CinemaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Core.Contracts;

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class MovieApiController : ControllerBase
    {
        private readonly IProjectionService projectionService;

        public MovieApiController(IProjectionService projectionService)
        {
            this.projectionService = projectionService;
        }

        [Route("GetShowTimes")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        public async Task<ActionResult<IEnumerable<string>>> GetShowTimes(Guid movieId, Guid cinemaId)
        {
            IEnumerable<DateTime> showTimes = await projectionService
                .GetProjectionAvailableShowtimesAsync(movieId, cinemaId);
            IEnumerable<string> showTimesResult = showTimes
                .Select(st => st.ToString("g"))
                .ToArray();

            return this.Ok(showTimesResult);
        }
    }
}
