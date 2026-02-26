namespace CinemaApp.Web.ViewModels.Movie
{
    using Services.Mapping;
    using Services.Models.Movie;

    public class AllMoviesIndexViewModel : IMapFrom<MovieAllDto>
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string Genre { get; set; } = null!;

        public string ReleaseDate { get; set; } = null!;

        public string Director { get; set; } = null!;

        public string? ImageUrl { get; set; }
    }
}
