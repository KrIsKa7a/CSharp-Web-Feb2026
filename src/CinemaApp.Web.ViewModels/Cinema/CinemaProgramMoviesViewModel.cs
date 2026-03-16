namespace CinemaApp.Web.ViewModels.Cinema
{
    using Services.Mapping;
    using Services.Models.Cinema;

    public class CinemaProgramMoviesViewModel : IMapFrom<CinemaProgramMovieDto>
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string Director { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;
    }
}
