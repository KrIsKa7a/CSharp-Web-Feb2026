namespace CinemaApp.Services.Core
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;

    using CinemaApp.Data;
    using CinemaApp.Data.Models;
    using Common.Utilities;
    using Contracts;
    using Data.Movie.Request;
    using Data.Movie.Response;

    public class MovieService : IMovieService
    {
        private readonly CinemaAppDbContext dbContext;

        private readonly BasicModelValidator modelValidator;

        public MovieService(CinemaAppDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.modelValidator = new BasicModelValidator();
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

        public async Task<MovieServiceStatusResponseModel> CreateMovieAsync(CreateMovieRequestData requestData)
        {
            MovieServiceStatusResponseModel response = new MovieServiceStatusResponseModel();

            bool isInputDataValid = modelValidator.IsValid(requestData);
            if (!isInputDataValid)
            {
                response.Status = false;
                response.ErrorMessages = modelValidator.ValidationResults.Select(vr => vr.ErrorMessage);
            }
            else
            {
                Movie movie = new Movie()
                {
                    Title = requestData.Title,
                    Genre = requestData.Genre,
                    ReleaseDate = requestData.ReleaseDate,
                    Duration = requestData.Duration,
                    Director = requestData.Director,
                    Description = requestData.Description,
                    ImageUrl = requestData.ImageUrl
                };

                try
                {
                    await dbContext.Movies.AddAsync(movie);
                    await dbContext.SaveChangesAsync();

                    response.Status = true;
                }
                catch (DbUpdateException ex)
                {
                    /* TODO: Log the exception details for further analysis and debugging. */
                    response.Status = false;
                    response.ErrorMessages = new List<string?>() { ex.Message };
                }
            }

            return response;
        }
    }
}
