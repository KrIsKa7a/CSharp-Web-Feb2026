namespace CinemaApp.Services.Core
{
    using System.Globalization;

    using Contracts;
    using Data.Models;
    using Data.Repository.Contracts;
    using GCommon.Exceptions;
    using Models.Movie;
    using Web.ViewModels.Movie;
    using static GCommon.ApplicationConstants;

    using AutoMapper;

    public class MovieService : IMovieService
    {
        private readonly IMapper mapper;
        private readonly IMovieRepository movieRepository;

        public MovieService(IMovieRepository movieRepository, IMapper mapper)
        {
            this.movieRepository = movieRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<MovieAllDto>> GetAllMoviesOrderedByTitleAsync()
        {
			// TODO: Use DTOs for data transfers between Data-Service-Controller layers instead of coupling Service to ViewModels
			// Fetch data
			IEnumerable<Movie> allMoviesDb = await movieRepository
                .GetAllMoviesNoTrackingWithProjectionAsync(m => new Movie()
                {
                    Id = m.Id,
                    Title = m.Title,
                    Genre = m.Genre,
                    ReleaseDate = m.ReleaseDate,
                    Director = m.Director,
                    ImageUrl = m.ImageUrl
                });

            // Process data
            IEnumerable<MovieAllDto> allMoviesViewModel = mapper
                .Map<IEnumerable<MovieAllDto>>(allMoviesDb)
                .OrderBy(m => m.Title)
                .ThenBy(m => m.Genre)
                .ThenBy(m => m.Director)
                .ToArray();

            // Return processed data
            return allMoviesViewModel;
        }

        public async Task CreateMovieAsync(MovieFormModel movieFormModel)
        {
            Movie newMovie = new Movie()
            {
                Title = movieFormModel.Title,
                Genre = movieFormModel.Genre,
                ReleaseDate = movieFormModel.ReleaseDate,
                Description = movieFormModel.Description,
                Duration = movieFormModel.Duration,
                Director = movieFormModel.Director,
                ImageUrl = movieFormModel.ImageUrl,
            };

            bool successAdd = await movieRepository.AddMovieAsync(newMovie);
            if (!successAdd)
            {
                throw new EntityPersistFailureException();
            }
        }

        public async Task<MovieDetailsViewModel?> GetMovieDetailsByIdAsync(Guid id)
        {
	        Movie? movieDb = await movieRepository
		        .GetMovieByIdAsync(id);

	        if (movieDb == null)
	        {
		        return null;
	        }

            return new MovieDetailsViewModel()
            {
				Id = movieDb.Id,
				Title = movieDb.Title,
				Genre = movieDb.Genre,
				ReleaseDate = movieDb.ReleaseDate.ToString(DefaultDateFormat, CultureInfo.InvariantCulture),
				Description = movieDb.Description,
				Duration = movieDb.Duration,
				Director = movieDb.Director,
				ImageUrl = movieDb.ImageUrl ?? DefaultImageUrl
			};
		}

        public async Task<MovieFormModel?> GetMovieFormModelByIdAsync(Guid id)
        {
            Movie? movieDb = await movieRepository
                .GetMovieByIdAsync(id);

            if (movieDb == null)
            {
                return null;
            }

            return new MovieFormModel()
            {
                Title = movieDb.Title,
                Genre = movieDb.Genre,
                ReleaseDate = movieDb.ReleaseDate,
                Description = movieDb.Description,
                Duration = movieDb.Duration,
                Director = movieDb.Director,
                ImageUrl = movieDb.ImageUrl ?? DefaultImageUrl
            };
        }

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            return await movieRepository.ExistsByIdAsync(id);
        }

        public async Task EditMovieAsync(Guid id, MovieFormModel movieFormModel)
        {
            Movie? movieDb = await movieRepository
                .GetMovieByIdAsync(id);
            if (movieDb == null)
            {
                throw new EntityNotFoundException();
            }

            movieDb.Title = movieFormModel.Title;
            movieDb.Genre = movieFormModel.Genre;
            movieDb.ReleaseDate = movieFormModel.ReleaseDate;
            movieDb.Description = movieFormModel.Description;
            movieDb.Duration = movieFormModel.Duration;
            movieDb.Director = movieFormModel.Director;
            movieDb.ImageUrl = movieFormModel.ImageUrl;
            
            bool editSuccess = await movieRepository.EditMovieAsync(movieDb);
            if (!editSuccess)
            {
                throw new EntityPersistFailureException();
            }
        }

        public async Task SoftDeleteMovieAsync(Guid id)
        {
            Movie? movieDb = await movieRepository
                .GetMovieByIdAsync(id);
            if (movieDb == null)
            {
                throw new EntityNotFoundException();
            }

            bool deleteSuccess = await movieRepository.SoftDeleteMovieAsync(movieDb);
            if (!deleteSuccess)
            {
                throw new EntityPersistFailureException();
            }
        }

        public async Task HardDeleteMovieAsync(Guid id)
        {
            Movie? movieDb = await movieRepository
                .GetMovieByIdAsync(id);
            if (movieDb == null)
            {
                throw new EntityNotFoundException();
            }

            bool deleteSuccess = await movieRepository.HardDeleteMovieAsync(movieDb);
            if (!deleteSuccess)
            {
                throw new EntityPersistFailureException();
            }
        }
    }
}
