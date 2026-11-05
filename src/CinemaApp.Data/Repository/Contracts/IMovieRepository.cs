namespace CinemaApp.Data.Repository.Contracts
{
    using Common;
    using Models;

    public interface IMovieRepository : ISoftDeleteableRepository<Movie>
    {
        Task<Movie?> FindByIdAsync(Guid id);
    }
}
