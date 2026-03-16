namespace CinemaApp.Services.Models.Cinema
{
    using Data.Models;
    using Mapping;

    public class CinemaProgramMovieDto : IMapFrom<Movie>
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string Director { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;
    }
}
