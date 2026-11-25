namespace CinemaApp.Web.ViewModels.Movie
{
    using System.ComponentModel.DataAnnotations;
    using static GCommon.InputValidationConstants.Movie;
    using static GCommon.ApplicationConstants;

    public class MovieIndexViewModel
    {
        public bool IsUserManager { get; set; } = false;

        [MaxLength(TitleMaxLength)]
        public string? SearchQuery { get; set; }

        [MaxLength(GenreMaxLength)]
        public string? GenreQuery { get; set; }

        [Range(ReleaseYearMinValue, ReleaseYearMaxValue)]
        public int? ReleaseYearQuery { get; set; }

        [Range(MinPageValue, int.MaxValue)]
        public int CurrentPage { get; set; } = DefaultPageValue;

        [Range(MinPageValue, int.MaxValue)]
        public int TotalPages { get; set; } = DefaultPageValue;

        [Range(MinPageValue, int.MaxValue)]
        public int PageNumber { get; set; } = DefaultPageValue;

        public IEnumerable<MoviesIndexEntryViewModel> Movies { get; set; }
            = new List<MoviesIndexEntryViewModel>();
    }
}
