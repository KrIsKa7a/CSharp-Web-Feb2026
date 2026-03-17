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

        public UserRepository(CinemaAppDbContext dbContext, UserManager<ApplicationUser> userManager) 
            : base(dbContext)
        {
            this.userManager = userManager;
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
    }
}
