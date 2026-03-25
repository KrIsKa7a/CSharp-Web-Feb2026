namespace CinemaApp.Data.Repository.Contracts
{
    using Models;

    public interface IManagerRepository
    {
        Task<Manager?> GetManagerByUserIdAsync(Guid userId);
    }
}
