namespace CinemaApp.Web.ViewModels.Movie
{
    using System.Globalization;

    using Services.Mapping;
    using Services.Models.Movie;
    using static GCommon.ApplicationConstants;

    using AutoMapper;

    public class MovieDetailsViewModel : MovieIndexViewModel, IMapFrom<MovieDetailsDto>, IHaveCustomMappings
	{
		public string Description { get; set; } = null!;

		public int Duration { get; set; }

        public new void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<MovieDetailsDto, MovieDetailsViewModel>()
                .ForMember(d => d.ReleaseDate,
                    y => y.MapFrom(s => s.ReleaseDate.ToString(DefaultDateFormat, CultureInfo.InvariantCulture)))
                .ForMember(d => d.ImageUrl, opt => opt.MapFrom(s => s.ImageUrl ?? DefaultImageUrl));
        }
    }
}
