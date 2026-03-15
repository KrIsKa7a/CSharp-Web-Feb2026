namespace CinemaApp.Web.Controllers
{
    using System.Security.Claims;

    using GCommon.Exceptions;
    using Services.Core.Contracts;
    using ViewModels.Ticket;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TicketApiController : ControllerBase
    {
        private readonly ITicketService ticketService;
        private readonly IProjectionService projectionService;

        public TicketApiController(ITicketService ticketService, IProjectionService projectionService)
        {
            this.ticketService = ticketService;
            this.projectionService = projectionService;
        }

        [HttpPost("BuyTicket")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> BuyTicket([FromBody] BuyTicketInputModel inputModel)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                bool isBought = await ticketService
                    .BuyTicketAsync(inputModel.ProjectionId, userId, inputModel.Quantity);
                if (!isBought)
                {
                    return BadRequest();
                }

                return Ok();
            }
            catch (EntityInputDataFormatException)
            {
                return BadRequest();
            }
        }
    }
}
