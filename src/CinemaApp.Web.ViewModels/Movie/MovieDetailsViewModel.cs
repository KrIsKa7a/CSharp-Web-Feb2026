namespace CinemaApp.Web.ViewModels.Movie
{
    using Services.Mapping;
    using Services.Models.Movie;

    public class MovieDetailsViewModel : AllMoviesIndexViewModel, IMapFrom<MovieDetailsDto>
	{
		public string Description { get; set; } = null!;

		public int Duration { get; set; }
	}
}
