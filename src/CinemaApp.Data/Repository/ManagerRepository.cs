namespace CinemaApp.Data.Repository
{
    using Contracts;
    using Models;

    using Microsoft.EntityFrameworkCore;

    public class ManagerRepository : BaseRepository, IManagerRepository
    {
        public ManagerRepository(CinemaAppDbContext dbContext) 
            : base(dbContext)
        {

        }

        public async Task<Manager?> GetManagerByUserIdAsync(Guid userId)
        {
            Manager? manager = await DbContext!
                .Managers
                .SingleOrDefaultAsync(m => m.UserId == userId);

            return manager;
        }
    }
}
