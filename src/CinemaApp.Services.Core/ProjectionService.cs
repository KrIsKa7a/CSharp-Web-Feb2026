namespace CinemaApp.Services.Core
{
    using Contracts;
    using Data.Models;
    using Data.Repository.Contracts;
    using Models.Projection;

    using AutoMapper;

    public class ProjectionService : IProjectionService
    {
        private readonly IProjectionRepository projectionRepository;

        private readonly IMapper mapper;

        public ProjectionService(IProjectionRepository projectionRepository, IMapper mapper)
        {
            this.projectionRepository = projectionRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ProjectionShowtimeDto>> GetProjectionAvailableShowtimesAsync(Guid movieId, Guid cinemaId)
        {
            IEnumerable<Projection> projections = await projectionRepository
                .GetAllProjectionsAsync(pr => pr.MovieId == movieId && pr.CinemaId == cinemaId && pr.AvailableTickets > 0);

            IEnumerable<ProjectionShowtimeDto> projectionShowtimes = mapper
                .Map<IEnumerable<ProjectionShowtimeDto>>(projections);

            return projectionShowtimes;
        }
    }
}
