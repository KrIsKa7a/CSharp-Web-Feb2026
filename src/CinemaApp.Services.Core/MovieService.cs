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
            IEnumerable<AllMoviesIndexViewModel> allMoviesViewModel = await movieRepository
                .GetAllMoviesNoTracking()
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
                .ToArrayAsync();

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
    }
}
