namespace CinemaApp.Services.Core
{
    using System.Globalization;

    using Contracts;
    using Data.Models;
    using Data.Repository.Contracts;
    using GCommon.Exceptions;
    using Web.ViewModels.Movie;
    using static GCommon.ApplicationConstants;

    using Microsoft.EntityFrameworkCore;

    public class MovieService : IMovieService
    {
        private readonly IMovieRepository movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }

        public async Task<IEnumerable<AllMoviesIndexViewModel>> GetAllMoviesOrderedByTitleAsync()
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
            IEnumerable<AllMoviesIndexViewModel> allMoviesViewModel = allMoviesDb
                .Select(m => new AllMoviesIndexViewModel()
                {
                    Id = m.Id,
                    Title = m.Title,
                    Genre = m.Genre,
                    ReleaseDate = m.ReleaseDate.ToString(DefaultDateFormat, CultureInfo.InvariantCulture),
                    Director = m.Director,
                    ImageUrl = m.ImageUrl ?? DefaultImageUrl
                })
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
                ReleaseDate = DateOnly.FromDateTime(movieFormModel.ReleaseDate),
                Description = movieFormModel.Description,
                Duration = movieFormModel.Duration,
                Director = movieFormModel.Director,
                ImageUrl = movieFormModel.ImageUrl,
            };

            bool successAdd = await movieRepository.AddMovieAsync(newMovie);
            if (!successAdd)
            {
                throw new EntityCreatePersistFailureException();
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
    }
}
