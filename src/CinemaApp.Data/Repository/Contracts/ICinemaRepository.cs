namespace CinemaApp.Data.Repository.Contracts
{
    using System.Linq.Expressions;
    using Models;

    public interface ICinemaRepository
    {
        Task<IEnumerable<Cinema>> GetAllCinemas(Expression<Func<Cinema, bool>>? filterQuery = null,
            Expression<Func<Cinema, Cinema>>? projectionQuery = null, bool includeProjections = false);
    }
}
