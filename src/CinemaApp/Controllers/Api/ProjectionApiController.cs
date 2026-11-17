namespace CinemaApp.Web.Controllers.Api
{
    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Contracts;
    using Services.Core.Data.Projection.Response;

    [Route("api/[controller]")]
    [ApiController]
    public class ProjectionApiController : ControllerBase
    {
        private readonly IProjectionService projectionService;

        public ProjectionApiController(IProjectionService projectionService)
        {
            this.projectionService = projectionService;
        }

        [HttpGet("[action]")]
        [ProducesResponseType<GetProjectionDatesResponseModel[]>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetProjectionDatesResponseModel[]>> GetDates(Guid? cinemaId, Guid? movieId)
        {
            if (!cinemaId.HasValue ||
                !movieId.HasValue)
            {
                return BadRequest();
            }

            IEnumerable<GetProjectionDatesResponseModel> responseData = await projectionService
                .GetProjectionDatesAsync(cinemaId.Value, movieId.Value);

            return responseData.ToArray();
        }
    }
}
