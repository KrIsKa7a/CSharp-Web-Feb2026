namespace CinemaApp.Web.ViewModels.Ticket
{
    using Services.Mapping;
    using Services.Models.Ticket;
    using static GCommon.ApplicationConstants;

    using AutoMapper;

    public class UserTicketsViewModel : IMapFrom<UserTicketDto>, IHaveCustomMappings
    {
        public string MovieTitle { get; set; } = null!;

        public string MovieImageUrl { get; set; } = null!;

        public string CinemaName { get; set; } = null!;

        public string TicketPrice { get; set; } = null!;

        public int TicketCount { get; set; }

        public string Showtime { get; set; } = null!;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<UserTicketDto, UserTicketsViewModel>()
                .ForMember(d => d.MovieImageUrl, opt => opt.MapFrom(s => s.MovieImageUrl ?? DefaultImageUrl))
                .ForMember(d => d.TicketPrice, opt => opt.MapFrom(s => s.TicketPrice.ToString("F2")))
                .ForMember(d => d.Showtime, opt => opt.MapFrom(s => s.Showtime.ToString(DefaultShowtimeFormat)));
        }
    }
}
