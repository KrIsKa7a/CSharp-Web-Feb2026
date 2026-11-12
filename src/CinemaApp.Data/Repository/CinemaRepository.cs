namespace CinemaApp.Data.Repository
{
    using Common;
    using Contracts;
    using Models;

    public class CinemaRepository : BaseSoftDeleteableRepository<Cinema>, ICinemaRepository
    {
        public CinemaRepository(CinemaAppDbContext dbContext) 
            : base(dbContext)
        {

        }
    }
}
