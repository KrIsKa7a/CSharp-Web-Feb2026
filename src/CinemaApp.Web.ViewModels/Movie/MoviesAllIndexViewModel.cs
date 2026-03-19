namespace CinemaApp.Web.ViewModels.Movie
{
    public class MoviesAllIndexViewModel
    {
        public string? SearchQuery { get; set; }

        public int PageNumber { get; set; } = 1;

        public int TotalPages { get; set; } = 1;

        public int ShowingPages { get; set; } = 9;

        public int StartPageIndex { get; set; } = 1;

        public ICollection<MovieIndexViewModel> Movies { get; set; }
            = new List<MovieIndexViewModel>();
    }
}
