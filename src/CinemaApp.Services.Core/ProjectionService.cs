namespace CinemaApp.Services.Core
{
    using Microsoft.EntityFrameworkCore;

    using CinemaApp.Data.Models;
    using CinemaApp.Data.Repository.Contracts;
    using Contracts;
    using Data.Projection.Response;

    public class ProjectionService : IProjectionService
    {
        private readonly ICinemaMovieRepository cinemaMovieRepository;

        public ProjectionService(ICinemaMovieRepository cinemaMovieRepository)
        {
            this.cinemaMovieRepository = cinemaMovieRepository;
        }

        public async Task<IEnumerable<GetProjectionDatesResponseModel>> GetProjectionDatesAsync(Guid cinemaId, Guid movieId)
        {
            IEnumerable<GetProjectionDatesResponseModel> responseData = await cinemaMovieRepository
                .AllAsNoTracking()
                .Where(cm => cm.CinemaId == cinemaId && cm.MovieId == movieId)
                .Select(cm => new GetProjectionDatesResponseModel()
                {
                    Id = cm.Id,
                    CinemaId = cm.CinemaId,
                    MovieId = cm.MovieId,
                    Showtime = cm.Showtime
                })
                .ToArrayAsync();

            return responseData;
        }

        public async Task<int> GetProjectionAvailabilityAsync(Guid projectionId)
        {
            CinemaMovie? dbProjection = await cinemaMovieRepository
                .AllAsNoTracking()
                .SingleOrDefaultAsync(cm => cm.Id == projectionId);
            if (dbProjection == null)
            {
                return 0;
            }

            return dbProjection.AvailableTickets;
        }
    }
}
