namespace CinemaApp.Web.ViewModels.Admin.Movie
{
    using CinemaApp.Web.ViewModels.Movie;

    public class AdminManageMovieViewModel : MovieIndexViewModel
    {
        public bool IsDeleted { get; set; }

        public int Duration { get; set; }
    }
}
