namespace CinemaApp.Services.Core
{
    using Microsoft.EntityFrameworkCore;

    using CinemaApp.Data;
    using Contracts;
    using Data.Movie.Response;

    public class MovieService : IMovieService
    {
        private readonly CinemaAppDbContext dbContext;

        public MovieService(CinemaAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<GetAllMoviesResponseModel>> GetAllMoviesAsync()
        {
            /* TODO: Implement Mapperly for auto generation of mapping code */
            IEnumerable<GetAllMoviesResponseModel> moviesResponseData = await dbContext.Movies
                .OrderBy(m => m.Title)
                .ThenBy(m => m.Genre)
                .Select(m => new GetAllMoviesResponseModel()
                {
                    Id = m.Id,
                    Title = m.Title,
                    Genre = m.Genre,
                    Director = m.Director,
                    ReleaseDate = m.ReleaseDate,
                    ImageUrl = m.ImageUrl
                })
                .ToArrayAsync();

            return moviesResponseData;
        }
    }
}
