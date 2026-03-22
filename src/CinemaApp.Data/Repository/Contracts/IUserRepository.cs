namespace CinemaApp.Data.Repository.Contracts
{
    using System.Linq.Expressions;

    using Models;

    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync(Expression<Func<ApplicationUser, bool>>? filterQuery = null,
            Expression<Func<ApplicationUser, ApplicationUser>>? projectionQuery = null);

        Task<IEnumerable<string>> GetUserRolesAsync(ApplicationUser appUser);

        Task<bool> UpdateUserRoleAsync(Guid userId, string role, bool removingRole = false);

        Task<bool> DeleteUserAsync(Guid userId);
    }
}
