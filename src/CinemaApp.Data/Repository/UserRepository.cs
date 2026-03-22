namespace CinemaApp.Data.Repository
{
    using System.Linq.Expressions;

    using Contracts;
    using Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity;

    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;

        public UserRepository(CinemaAppDbContext dbContext, 
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager) 
            : base(dbContext)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync(Expression<Func<ApplicationUser, bool>>? filterQuery = null, Expression<Func<ApplicationUser, ApplicationUser>>? projectionQuery = null)
        {
            IQueryable<ApplicationUser> applicationUsers = DbContext!
                .Users
                .AsNoTracking();

            if (filterQuery != null)
            {
                applicationUsers = applicationUsers
                    .Where(filterQuery);
            }

            if (projectionQuery != null)
            {
                applicationUsers = applicationUsers
                    .Select(projectionQuery);
            }

            IEnumerable<ApplicationUser> appUsers = await applicationUsers
                .OrderBy(u => u.Email)
                .ToArrayAsync();
            
            return appUsers;
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(ApplicationUser appUser)
        {
            IEnumerable<string> userRoles = await userManager.GetRolesAsync(appUser);

            return userRoles;
        }

        public async Task<bool> UpdateUserRoleAsync(Guid userId, string role, bool removingRole = false)
        {
            ApplicationUser? appUser = await userManager
                .FindByIdAsync(userId.ToString());
            if (appUser == null)
            {
                return false;
            }

            bool roleExists = await roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                return false;
            }

            bool alreadyInRole = await userManager.IsInRoleAsync(appUser, role);
            if (!removingRole && alreadyInRole)
            {
                return false;
            }
            
            if (removingRole && !alreadyInRole)
            {
                return false;
            }

            IdentityResult roleOperationResult;
            if (removingRole)
            {
                roleOperationResult = await userManager
                    .RemoveFromRoleAsync(appUser, role);
            }
            else
            {
                roleOperationResult = await userManager
                    .AddToRoleAsync(appUser, role);
            }

            if (roleOperationResult != IdentityResult.Success)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            ApplicationUser? appUser = await userManager
                .FindByIdAsync(userId.ToString());
            if (appUser == null)
            {
                return false;
            }

            IdentityResult deleteResult = await userManager
                .DeleteAsync(appUser);
            if (deleteResult != IdentityResult.Success)
            {
                return false;
            }

            return true;
        }
    }
}
