namespace CinemaApp.Web.Controllers
{
    using Services.Core.Contracts;
    using Services.Models.Projection;
    using ViewModels.Movie.ApiModels;

    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class MovieApiController : ControllerBase
    {
        private readonly IProjectionService projectionService;

        private readonly IMapper mapper;

        public MovieApiController(IProjectionService projectionService, IMapper mapper)
        {
            this.projectionService = projectionService;
            this.mapper = mapper;
        }

        [HttpGet("GetShowTimes")]
        [ProducesResponseType(typeof(IEnumerable<MovieShowtimesApiResponseModel>), 200)]
        public async Task<ActionResult<IEnumerable<MovieShowtimesApiResponseModel>>> GetShowTimes(Guid movieId, Guid cinemaId)
        {
            IEnumerable<ProjectionShowtimeDto> showTimes = await projectionService
                .GetProjectionAvailableShowtimesAsync(movieId, cinemaId);
            IEnumerable<MovieShowtimesApiResponseModel> showTimesResult = mapper
                .Map<IEnumerable<MovieShowtimesApiResponseModel>>(showTimes);

            return this.Ok(showTimesResult);
        }
    }
}
