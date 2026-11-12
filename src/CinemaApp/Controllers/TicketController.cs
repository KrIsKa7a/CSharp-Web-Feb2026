namespace CinemaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Contracts;
    using Services.Core.Data.Ticket.Response;
    using ViewModels.UserTicket;
    using static GCommon.ApplicationConstants;

    public class TicketController : BaseController
    {
        private readonly ITicketService ticketService;

        public TicketController(ITicketService ticketService)
        {
            this.ticketService = ticketService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string userId = GetUserId()!;

            IEnumerable<GetUserAllTicketsResponseModel> responseData = await ticketService
                .GetUserAllTicketsByUserIdAsync(userId);
            IEnumerable<UserTicketIndexViewModel> indexViewModel = responseData
                .Select(t => new UserTicketIndexViewModel()
                {
                    CinemaName = t.CinemaName,
                    MovieTitle = t.MovieTitle,
                    MovieImageUrl = t.MovieImageUrl ?? DefaultImageUrl,
                    Price = t.Price.ToString(PricesFormat),
                    TicketsCount = t.Quantity.ToString()
                })
                .ToArray();

            return View(indexViewModel);
        }
    }
}
