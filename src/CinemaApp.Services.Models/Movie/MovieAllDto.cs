namespace CinemaApp.Services.Models.Movie
{
    using System.Globalization;
    using AutoMapper;
    using Data.Models;
    using Mapping;

    using static GCommon.ApplicationConstants;

    public class MovieAllDto : IMapFrom<Movie>, IHaveCustomMappings
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string Genre { get; set; } = null!;

        public string ReleaseDate { get; set; } = null!;

        public string Director { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Movie, MovieAllDto>()
                .ForMember(d => d.ReleaseDate, 
                    y => y.MapFrom(s => s.ReleaseDate.ToString(DefaultDateFormat, CultureInfo.InvariantCulture)));
        }
    }
}
