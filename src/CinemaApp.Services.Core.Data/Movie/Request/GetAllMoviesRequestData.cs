namespace CinemaApp.Services.Core.Data.Movie.Request
{
    using System.ComponentModel.DataAnnotations;
    using static GCommon.InputValidationConstants.Movie;

    public class GetAllMoviesRequestData
    {
        [MaxLength(TitleMaxLength)]
        public string? SearchQuery { get; set; }

        [MaxLength(GenreMaxLength)]
        public string? GenreQuery { get; set; }

        [Range(ReleaseYearMinValue, ReleaseYearMaxValue)]
        public int? ReleaseYearQuery { get; set; }

        public int SkipEntries { get; set; }

        public int TakeEntries { get; set; }
    }
}
