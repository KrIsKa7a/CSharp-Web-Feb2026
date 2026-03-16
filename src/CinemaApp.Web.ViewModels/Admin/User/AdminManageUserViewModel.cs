namespace CinemaApp.Web.ViewModels.Admin.User
{
    using Services.Mapping;
    using Services.Models.ApplicationUser;

    public class AdminManageUserViewModel : IMapFrom<UserManageAllDto>
    {
        public Guid Id { get; set; }

        public string Email { get; set; } = null!;

        public IEnumerable<string> Roles { get; set; }
            = new List<string>();
    }
}
