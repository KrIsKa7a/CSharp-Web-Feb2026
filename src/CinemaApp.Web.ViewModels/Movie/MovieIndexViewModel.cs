namespace CinemaApp.Web.ViewModels.Movie
{
    using System.ComponentModel.DataAnnotations;
    using static GCommon.InputValidationConstants.Movie;

    public class MovieIndexViewModel
    {
        public bool IsUserManager { get; set; } = false;

        [MaxLength(TitleMaxLength)]
        public string? SearchQuery { get; set; }

        public IEnumerable<MoviesIndexEntryViewModel> Movies { get; set; }
            = new List<MoviesIndexEntryViewModel>();
    }
}
