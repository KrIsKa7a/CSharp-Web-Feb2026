namespace CinemaApp.Data.Repository.Contracts
{
    using System.Linq.Expressions;
    using Models;

    public interface IProjectionRepository
    {
        Task<IEnumerable<Projection>> GetAllProjectionsAsync(Expression<Func<Projection, bool>>? filterQuery = null);

        Task<Projection?> FindByIdAsync(Guid id);

        Task<bool> EditProjectionAsync(Projection projection);
    }
}
