namespace CinemaApp.Services.Core.Contracts
{
    using Models.ApplicationUser;

    public interface IUserService
    {
        Task<IEnumerable<UserManageAllDto>> GetAllManageableUsersAsync(string adminUserId);
    }
}
