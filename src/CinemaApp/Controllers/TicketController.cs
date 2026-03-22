namespace CinemaApp.Web.Controllers
{
    using Services.Core.Contracts;
    using Services.Models.Ticket;
    using ViewModels.Ticket;

    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;

    public class TicketController : BaseController
    {
        private readonly ITicketService ticketService;
        private readonly IMapper mapper;

        public TicketController(ITicketService ticketService, IMapper mapper)
        {
            this.ticketService = ticketService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            string userId = this.GetUserId()!;
            
            IEnumerable<UserTicketDto> userTickets = await ticketService
                .GetUserTicketsAsync(userId);
            IEnumerable<UserTicketsViewModel> userTicketsVms = mapper
                .Map<IEnumerable<UserTicketsViewModel>>(userTickets);

            return View(userTicketsVms);
        }
    }
}
