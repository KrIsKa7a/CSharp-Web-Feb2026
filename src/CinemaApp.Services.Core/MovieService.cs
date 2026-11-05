namespace CinemaApp.Services.Core
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    
    using CinemaApp.Data.Models;
    using CinemaApp.Data.Repository.Contracts;
    using Common.Utilities;
    using Contracts;
    using Data.Movie.Enums;
    using Data.Movie.Request;
    using Data.Movie.Response;

    public class MovieService : IMovieService
    {
        private readonly IMovieRepository movieRepository;

        private readonly BasicModelValidator modelValidator;
        private readonly ILogger<MovieService> logger;

        public MovieService(IMovieRepository movieRepository, ILogger<MovieService> logger)
        {
            this.movieRepository = movieRepository;
            this.modelValidator = new BasicModelValidator();
            this.logger = logger;
        }

        public async Task<IEnumerable<GetAllMoviesResponseModel>> GetAllMoviesAsync()
        {
            /* TODO: Implement Mapperly for auto generation of mapping code */
            IEnumerable<GetAllMoviesResponseModel> moviesResponseData = await movieRepository
                .AllAsNoTracking()
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
                    await movieRepository.AddAsync(movie);
                    await movieRepository.SaveChangesAsync();

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

        public async Task<MovieBasicDataResponseModel?> GetMovieBasicDataByIdAsync(Guid id)
        {
            MovieBasicDataResponseModel? responseData = null;
            Movie? dbMovie = await movieRepository
                .FindByIdAsync(id);
            if (dbMovie != null)
            {
                responseData = new MovieBasicDataResponseModel()
                {
                    Id = dbMovie.Id,
                    Title = dbMovie.Title,
                    Genre = dbMovie.Genre,
                    ImageUrl = dbMovie.ImageUrl,
                };
            }

            return responseData;
        }

        public async Task<MovieExtendedDataResponseModel?> GetMovieExtendedDataByIdAsync(Guid id)
        {
            MovieExtendedDataResponseModel? responseData = null;
            Movie? dbMovie = await movieRepository
                .FindByIdAsync(id);
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

            Movie? dbMovie = await movieRepository
                .FindByIdAsync(requestData.Id);
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
                    
                    await movieRepository.SaveChangesAsync();

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

        public async Task<MovieServiceStatusResponseModel> SoftDeleteMovieAsync(Guid id)
        {
            MovieServiceStatusResponseModel response = new MovieServiceStatusResponseModel()
            {
                Status = MovieServiceResponseStatus.GeneralError,
                ErrorMessages = new List<string?>()
            };

            Movie? dbMovie = await movieRepository
                .FindByIdAsync(id);
            if (dbMovie == null)
            {
                response.Status = MovieServiceResponseStatus.NotFound;

                return response;
            }

            if (dbMovie.IsDeleted)
            {
                this.logger.LogWarning($"Soft delete was performed for already deleted entity! Entity Id: {id}");
                response.Status = MovieServiceResponseStatus.Success;
            }
            else
            {
                try
                {
                    movieRepository.SoftDelete(dbMovie);
                    await movieRepository.SaveChangesAsync();

                    response.Status = MovieServiceResponseStatus.Success;
                }
                catch (Exception ex)
                {
                    /* TODO: Log the exception details for further analysis and debugging. */
                    response.Status = MovieServiceResponseStatus.GeneralError;
                    response.ErrorMessages = new List<string?>() { ex.Message };
                }
            }

            return response;
        }

        public async Task<MovieServiceStatusResponseModel> HardDeleteMovieAsync(Guid id)
        {
            MovieServiceStatusResponseModel response = new MovieServiceStatusResponseModel()
            {
                Status = MovieServiceResponseStatus.GeneralError,
                ErrorMessages = new List<string?>()
            };

            Movie? dbMovie = await movieRepository
                .FindByIdAsync(id);
            if (dbMovie == null)
            {
                response.Status = MovieServiceResponseStatus.NotFound;

                return response;
            }

            if (dbMovie.IsDeleted)
            {
                this.logger.LogWarning($"Hard delete was performed for soft-deleted entity! Entity Id: {id}");
            }

            try
            {
                movieRepository.Remove(dbMovie);
                await movieRepository.SaveChangesAsync();

                response.Status = MovieServiceResponseStatus.Success;
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
