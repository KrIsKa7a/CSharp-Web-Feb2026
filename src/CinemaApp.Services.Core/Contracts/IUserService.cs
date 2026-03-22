namespace CinemaApp.Services.Core.Contracts
{
    using Models.ApplicationUser;

    public interface IUserService
    {
        Task<IEnumerable<UserManageAllDto>> GetAllManageableUsersAsync(string adminUserId);

        Task<bool> AssignRoleToUserAsync(Guid userId, string role);

        Task<bool> RemoveRoleFromUserAsync(Guid userId, string role);

        Task<bool> DeleteUserAsync(Guid userId);
    }
}
