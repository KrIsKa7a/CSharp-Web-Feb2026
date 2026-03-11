namespace CinemaApp.Services.Models.Cinema
{
    using Data.Models;
    using Mapping;

    public class CinemaAllDto : IMapFrom<Cinema>
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Location { get; set; } = null!;
    }
}
