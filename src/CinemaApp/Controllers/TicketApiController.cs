namespace CinemaApp.Web.Controllers
{
    using System.Security.Claims;

    using GCommon.Exceptions;
    using Services.Core.Contracts;
    using ViewModels.Ticket;
    using static GCommon.ApplicationConstants;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [EnableCors(SoftuniDomainPolicyName)]
    public class TicketApiController : ControllerBase
    {
        private readonly ITicketService ticketService;

        public TicketApiController(ITicketService ticketService)
        {
            this.ticketService = ticketService;
        }

        [HttpPost("BuyTicket")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
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
            catch (EntityPersistFailureException)
            {
                return BadRequest();
            }
        }
    }
}
