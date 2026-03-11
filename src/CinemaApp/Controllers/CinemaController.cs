namespace CinemaApp.Web.Controllers
{
    using Services.Core.Contracts;
    using Services.Models.Cinema;
    using ViewModels.Cinema;

    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class CinemaController : Controller
    {
        private readonly ICinemaService cinemaService;

        private readonly IMapper mapper;

        public CinemaController(ICinemaService cinemaService, IMapper mapper)
        {
            this.cinemaService = cinemaService;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<CinemaAllDto> cinemaAllDtos = await cinemaService
                .GetAllCinemasOrderedByLocationAsync();
            IEnumerable<CinemaIndexViewModel> cinemaIndexViewModels = mapper
                .Map<IEnumerable<CinemaIndexViewModel>>(cinemaAllDtos);

            return View(cinemaIndexViewModels);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Program([FromRoute(Name = "id")] Guid cinemaId)
        {
            CinemaProgramDetailsDto? cinemaProgramDetailsDto = await cinemaService
                .GetCinemaProgramByIdAsync(cinemaId);
            if (cinemaProgramDetailsDto == null)
            {
                return NotFound();
            }

            CinemaProgramViewModel cinemaProgramViewModel = mapper
                .Map<CinemaProgramViewModel>(cinemaProgramDetailsDto);

            return View(cinemaProgramViewModel);
        }
    }
}
