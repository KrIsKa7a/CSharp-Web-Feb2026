namespace CinemaApp.Web.ViewModels.Movie.ApiModels
{
    using Services.Mapping;
    using Services.Models.Projection;

    using AutoMapper;

    public class MovieShowtimesApiResponseModel : IMapFrom<ProjectionShowtimeDto>, IHaveCustomMappings
    {
        public Guid Id { get; set; }

        public string Showtime { get; set; } = null!;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<ProjectionShowtimeDto, MovieShowtimesApiResponseModel>()
                .ForMember(d => d.Showtime, opt => opt.MapFrom(s => s.Showtime.ToString("g")));
        }
    }
}
