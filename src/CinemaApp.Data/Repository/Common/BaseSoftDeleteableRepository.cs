namespace CinemaApp.Data.Repository.Common
{
    using Microsoft.EntityFrameworkCore;
    
    using Contracts.Common;
    using Data.Common.Models;

    public abstract class BaseSoftDeleteableRepository<TEntity> 
        : BaseRepository<TEntity>, ISoftDeleteableRepository<TEntity> 
        where TEntity : class, ISoftDeleteableEntity
    {
        protected BaseSoftDeleteableRepository(CinemaAppDbContext dbContext) 
            : base(dbContext)
        {

        }

        public IQueryable<TEntity> AllWithDeleted()
        {
            return DbSet.IgnoreQueryFilters()
                .AsQueryable();
        }

        public IQueryable<TEntity> AllWithDeletedAsNoTracking()
        {
            return DbSet.IgnoreQueryFilters()
                .AsNoTracking()
                .AsQueryable();
        }

        public void SoftDelete(TEntity entity)
        {
            entity.IsDeleted = true;
        }

        public void Recover(TEntity entity)
        {
            entity.IsDeleted = false;
        }
    }
}
