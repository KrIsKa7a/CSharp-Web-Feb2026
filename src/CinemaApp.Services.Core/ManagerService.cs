namespace CinemaApp.Services.Core
{
    using Contracts;
    using Data.Models;
    using Data.Repository.Contracts;

    public class ManagerService : IManagerService
    {
        private readonly IManagerRepository managerRepository;

        public ManagerService(IManagerRepository managerRepository)
        {
            this.managerRepository = managerRepository;
        }

        public async Task<bool> IsUserManagerAsync(string userId)
        {
            bool guidValid = Guid.TryParse(userId, out Guid userGuid);
            if (!guidValid)
            {
                return false;
            }

            Manager? manager = await managerRepository
                .GetManagerByUserIdAsync(userGuid);

            return manager != null;
        }
    }
}
