namespace CinemaApp.Services.Core
{
    using System.Globalization;

    using Contracts;
    using Data;
    using Web.ViewModels.Movie;
    using static GCommon.ApplicationConstants;

    using Microsoft.EntityFrameworkCore;

    public class MovieService : IMovieService
    {
        private readonly CinemaAppDbContext dbContext;

        public MovieService(CinemaAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<AllMoviesIndexViewModel>> GetAllMoviesOrderedByTitleAsync()
        {
            IEnumerable<AllMoviesIndexViewModel> allMoviesViewModel = await dbContext
                .Movies
                .AsNoTracking()
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
    }
}
