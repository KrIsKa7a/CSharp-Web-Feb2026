namespace CinemaApp.Services.Models.Cinema
{
    using Data.Models;
    using Mapping;

    using AutoMapper;

    public class CinemaProgramDetailsDto : IMapFrom<Cinema>, IHaveCustomMappings
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public IEnumerable<CinemaProgramMovieDto> ProjectionMovies { get; set; } = null!;
        
        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<Cinema, CinemaProgramDetailsDto>()
                .ForMember(d => d.ProjectionMovies, opt => opt.MapFrom(s => s.Projections.Select(p => p.Movie)));
        }
    }
}
