namespace CinemaApp.Web.Controllers.Api
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Contracts;
    using Services.Core.Data.Ticket.Request;

    using System.Security.Claims;
    using static GCommon.InputValidationConstants.Ticket;

    [Route("api/[controller]")]
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

        [Authorize]
        [AutoValidateAntiforgeryToken]
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> BuyTicket([FromForm] Guid? projectionId, [FromForm] int quantity)
        {
            if (!projectionId.HasValue || quantity < MinimumPurchaseQuantity)
            {
                return BadRequest();
            }

            int ticketsAvailable = await projectionService
                .GetProjectionAvailabilityAsync(projectionId.Value);
            int ticketsBought = await ticketService
                .GetBoughtTicketsPerProjectionAsync(projectionId.Value);
            if (ticketsBought + quantity > ticketsAvailable)
            {
                return BadRequest("The available tickets are less than selected quantity!");
            }

            BuyTicketsRequestModel requestData = new BuyTicketsRequestModel()
            {
                ProjectionId = projectionId.Value,
                UserId = GetUserId()!,
                Quantity = quantity
            };

            bool ticketBuySuccess = await ticketService
                .BuyTicketsByUserIdAndProjectionIdAsync(requestData);
            if (!ticketBuySuccess)
            {
                return NotFound();
            }

            return Ok();
        }

        private string? GetUserId()
            => User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
