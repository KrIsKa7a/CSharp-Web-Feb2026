namespace CinemaApp.Web.ViewModels.Cinema
{
    using Services.Mapping;
    using Services.Models.Cinema;

    public class CinemaIndexViewModel : IMapFrom<CinemaAllDto>
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Location { get; set; } = null!;
    }
}
