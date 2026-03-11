namespace CinemaApp.Services.Core
{
    using System.Globalization;
    using Contracts;
    using Data.Models;
    using Data.Repository.Contracts;
    using GCommon.Exceptions;

    public class ProjectionService : IProjectionService
    {
        private readonly IProjectionRepository projectionRepository;

        public ProjectionService(IProjectionRepository projectionRepository)
        {
            this.projectionRepository = projectionRepository;
        }

        public async Task<IEnumerable<DateTime>> GetProjectionAvailableShowtimesAsync(Guid movieId, Guid cinemaId)
        {
            IEnumerable<Projection> projections = await projectionRepository
                .GetAllProjectionsAsync(pr => pr.MovieId == movieId && pr.CinemaId == cinemaId && pr.AvailableTickets > 0);

            IEnumerable<DateTime> projectionShowtimes = projections
                .Select(pr => pr.Showtime)
                .Distinct()
                .ToArray();

            return projectionShowtimes;
        }

        public async Task<Guid?> GetProjectionIdByMovieCinemaAndShowtimeAsync(Guid movieId, Guid cinemaId, string showtime)
        {
            bool dateTimeValid = DateTime
                .TryParse(showtime, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out DateTime showTimeDm);
            if (!dateTimeValid)
            {
                throw new EntityInputDataFormatException();
            }

            Projection? projection = (await projectionRepository
                .GetAllProjectionsAsync(pr => pr.MovieId == movieId && pr.CinemaId == cinemaId))
                .SingleOrDefault(pr => pr.Showtime.ToString("g") == showTimeDm.ToString("g"));
            if (projection == null)
            {
                return null;
            }

            return projection.Id;
        }
    }
}
