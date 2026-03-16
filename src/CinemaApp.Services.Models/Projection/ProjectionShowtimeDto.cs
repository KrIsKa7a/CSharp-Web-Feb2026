namespace CinemaApp.Services.Models.Projection
{
    using Data.Models;
    using Mapping;

    public class ProjectionShowtimeDto : IMapFrom<Projection>
    {
        public Guid Id { get; set; }

        public DateTime Showtime { get; set; }
    }
}
