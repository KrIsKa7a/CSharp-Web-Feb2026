namespace CinemaApp.Web.ViewModels.Admin.Movie
{
    using System.Globalization;

    using Services.Mapping;
    using Services.Models.Movie;
    using Web.ViewModels.Movie;
    using static GCommon.ApplicationConstants;

    using AutoMapper;

    public class AdminManageMovieViewModel : MovieIndexViewModel, IHaveCustomMappings
    {
        public bool IsDeleted { get; set; }

        public int Duration { get; set; }

        public new void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<MovieAllDto, AdminManageMovieViewModel>()
                .ForMember(d => d.ReleaseDate,
                    y => y.MapFrom(s => s.ReleaseDate.ToString(DefaultDateFormat, CultureInfo.InvariantCulture)))
                .ForMember(d => d.ImageUrl, opt => opt.MapFrom(s => s.ImageUrl ?? DefaultImageUrl));
        }
    }
}
