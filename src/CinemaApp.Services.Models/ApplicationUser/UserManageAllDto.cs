namespace CinemaApp.Services.Models.ApplicationUser
{
    using Data.Models;
    using Mapping;

    using AutoMapper;

    public class UserManageAllDto : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public IEnumerable<string> Roles { get; set; }
            = new List<string>();

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<ApplicationUser, UserManageAllDto>()
                .ForMember(d => d.Roles, opt => opt.Ignore());
        }
    }
}
