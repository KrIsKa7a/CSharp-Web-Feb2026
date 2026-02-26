namespace CinemaApp.Web.ViewModels.Movie
{
	public class MovieDetailsViewModel : AllMoviesIndexViewModel
	{
		public string Description { get; set; } = null!;

		public int Duration { get; set; }
	}
}
