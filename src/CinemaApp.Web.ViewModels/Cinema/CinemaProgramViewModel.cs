namespace CinemaApp.Web.ViewModels.Cinema
{
    using Services.Mapping;
    using Services.Models.Cinema;

    public class CinemaProgramViewModel : IMapFrom<CinemaProgramDetailsDto>
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public ICollection<CinemaProgramMoviesViewModel> ProjectionMovies { get; set; } 
            = new List<CinemaProgramMoviesViewModel>();
    }
}
