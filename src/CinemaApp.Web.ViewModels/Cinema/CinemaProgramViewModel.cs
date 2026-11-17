namespace CinemaApp.Web.ViewModels.Cinema
{
    using CinemaMovie;

    public class CinemaProgramViewModel
    {
        public Guid CinemaId { get; set; }

        public string CinemaName { get; set; } = null!;

        public IEnumerable<CinemaMovieViewModel> Movies { get; set; }
            = new List<CinemaMovieViewModel>();
    }
}
