namespace CinemaApp.Services.Core
{
    using Azure;

    using CinemaApp.Data;
    using CinemaApp.Data.Models;

    using Common.Utilities;

    using Contracts;

    using Data.Movie.Enums;
    using Data.Movie.Request;
    using Data.Movie.Response;

    using Microsoft.EntityFrameworkCore;

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
                response.Status = MovieServiceResponseStatus.InvalidRequest;
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

                    response.Status = MovieServiceResponseStatus.Success;
                }
                catch (DbUpdateException ex)
                {
                    /* TODO: Log the exception details for further analysis and debugging. */
                    response.Status = MovieServiceResponseStatus.GeneralError;
                    response.ErrorMessages = new List<string?>() { ex.Message };
                }
            }

            return response;
        }

        public async Task<MovieExtendedDataResponseModel?> GetMovieExtendedDataByIdAsync(Guid id)
        {
            MovieExtendedDataResponseModel? responseData = null;
            Movie? dbMovie = await dbContext.Movies
                .FindAsync(id);
            if (dbMovie != null)
            {
                responseData = new MovieExtendedDataResponseModel()
                {
                    Id = dbMovie.Id,
                    Title = dbMovie.Title,
                    Genre = dbMovie.Genre,
                    ReleaseDate = dbMovie.ReleaseDate,
                    Duration = dbMovie.Duration,
                    Director = dbMovie.Director,
                    Description = dbMovie.Description,
                    ImageUrl = dbMovie.ImageUrl
                };
            }

            return responseData;
        }

        public async Task<MovieServiceStatusResponseModel> EditMovieAsync(EditMovieRequestData requestData)
        {
            MovieServiceStatusResponseModel response = new MovieServiceStatusResponseModel()
            {
                Status = MovieServiceResponseStatus.GeneralError,
                ErrorMessages = new List<string?>()
            };

            Movie? dbMovie = await dbContext.Movies
                .FindAsync(requestData.Id);
            if (dbMovie == null)
            {
                response.Status = MovieServiceResponseStatus.NotFound;
            }

            bool isRequestDataValid = modelValidator.IsValid(requestData);
            if (!isRequestDataValid)
            {
                response.Status = MovieServiceResponseStatus.InvalidRequest;
                response.ErrorMessages = modelValidator.ValidationResults.Select(vr => vr.ErrorMessage);
            }

            try
            {
                if (dbMovie != null)
                {
                    dbMovie.Title = requestData.Title;
                    dbMovie.Description = requestData.Description;
                    dbMovie.ImageUrl = requestData.ImageUrl;
                    dbMovie.Director = requestData.Director;
                    dbMovie.Genre = requestData.Genre;
                    dbMovie.ReleaseDate = requestData.ReleaseDate;
                    dbMovie.Duration = requestData.Duration;

                    await dbContext.SaveChangesAsync();

                    response.Status = MovieServiceResponseStatus.Success;
                }
            }
            catch (Exception ex)
            {
                /* TODO: Log the exception details for further analysis and debugging. */
                response.Status = MovieServiceResponseStatus.GeneralError;
                response.ErrorMessages = new List<string?>() { ex.Message };
            }

            return response;
        }
    }
}
