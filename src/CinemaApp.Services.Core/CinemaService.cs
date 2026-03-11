namespace CinemaApp.Services.Core
{
    using Contracts;
    using Data.Models;
    using Data.Repository.Contracts;
    using Models.Cinema;

    using AutoMapper;

    public class CinemaService : ICinemaService
    {
        private readonly ICinemaRepository cinemaRepository;

        private readonly IMapper mapper;

        public CinemaService(ICinemaRepository cinemaRepository, IMapper mapper)
        {
            this.cinemaRepository = cinemaRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<CinemaAllDto>> GetAllCinemasOrderedByLocationAsync()
        {
            IEnumerable<Cinema> allCinemas = (await cinemaRepository
                .GetAllCinemas(
                    filterQuery: null,
                    projectionQuery: c => new Cinema
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Location = c.Location
                    }))
                .OrderBy(c => c.Location)
                .ToArray();
            IEnumerable<CinemaAllDto> cinemaDtos = mapper
                .Map<IEnumerable<CinemaAllDto>>(allCinemas);

            return cinemaDtos;
        }

        public async Task<CinemaProgramDetailsDto?> GetCinemaProgramByIdAsync(Guid cinemaId)
        {
            Cinema? cinema = await cinemaRepository
                .GetCinemaByIdIncludeMovies(cinemaId);
            if (cinema == null)
            {
                return null;
            }

            CinemaProgramDetailsDto cinemaProgramDto = mapper
                .Map<CinemaProgramDetailsDto>(cinema);
            
            return cinemaProgramDto;
        }
    }
}
