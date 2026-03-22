namespace CinemaApp.Data.Repository
{
    using System.Linq;
    using System.Linq.Expressions;

    using Contracts;
    using Models;

    using Microsoft.EntityFrameworkCore;

    public class MovieRepository : BaseRepository, IMovieRepository
    {
        public MovieRepository(CinemaAppDbContext dbContext)
            : base(dbContext)
        {

        }

        public async Task<IEnumerable<Movie>> GetAllMoviesNoTrackingWithProjectionAsync(Expression<Func<Movie, Movie>>? projectionQuery = null, 
            Expression<Func<Movie, bool>>? filterQuery = null, int? skipCnt = null, int? takeCnt = null, bool ignoreQueryFilters = false)
        {
            IQueryable<Movie> moviesFetchQuery = DbContext!
                .Movies
                .AsNoTracking()
                .OrderBy(m => m.Title)
                .ThenBy(m => m.Id);

            if (ignoreQueryFilters)
            {
                moviesFetchQuery = moviesFetchQuery
                    .IgnoreQueryFilters();
            }

            if (filterQuery != null)
            {
                moviesFetchQuery = moviesFetchQuery
                    .Where(filterQuery)
                    .AsQueryable();
            }

            if (projectionQuery != null)
            {
                moviesFetchQuery = moviesFetchQuery
                    .Select(projectionQuery)
                    .AsQueryable();
            }

            if (skipCnt.HasValue && skipCnt > 0)
            {
                moviesFetchQuery = moviesFetchQuery
                    .Skip(skipCnt.Value)
                    .AsQueryable();
            }

            if (takeCnt.HasValue && takeCnt > 0)
            {
                moviesFetchQuery = moviesFetchQuery
                    .Take(takeCnt.Value)
                    .AsQueryable();
            }

            return await moviesFetchQuery.ToArrayAsync();
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            return await DbContext!
                .Movies
                .AsNoTracking()
                .OrderBy(m => m.Title)
                .ToArrayAsync();
        }

        public async Task<Movie?> GetMovieByIdAsync(Guid id, bool ignoreQueryFilters = false)
        {
            IQueryable<Movie> getMovieQuery = DbContext!
                .Movies
                .AsQueryable();
            if (ignoreQueryFilters)
            {
                getMovieQuery = getMovieQuery
                    .IgnoreQueryFilters();
            }

            Movie? movie = await getMovieQuery
                .SingleOrDefaultAsync(m => m.Id == id);

            return movie;
        }

        public async Task<bool> AddMovieAsync(Movie movie)
        {
            await DbContext!.Movies.AddAsync(movie);
            int resultCount = await SaveChangesAsync();
            
            return resultCount == 1;
        }

        public async Task<bool> EditMovieAsync(Movie movie)
        {
            DbContext!.Movies.Update(movie);
            int resultCount = await SaveChangesAsync();

            return resultCount == 1;
        }

        public async Task<bool> SoftDeleteMovieAsync(Movie movie)
        {
            movie.IsDeleted = true;
            DbContext!.Movies.Update(movie);

            int resultCount = await SaveChangesAsync();

            return resultCount == 1;
        }

        public async Task<bool> HardDeleteMovieAsync(Movie movie)
        {
            DbContext!.Movies.Remove(movie);
            int resultCount = await SaveChangesAsync();

            return resultCount == 1;
        }

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            return await DbContext!
                .Movies
                .AnyAsync(m => m.Id == id);
        }

        public async Task<int> CountAsync(Expression<Func<Movie, bool>>? filterQuery)
        {
            IQueryable<Movie> moviesFetchQuery = DbContext!
                .Movies
                .AsNoTracking();
            if (filterQuery != null)
            {
                moviesFetchQuery = moviesFetchQuery
                    .Where(filterQuery)
                    .AsQueryable();
            }

            int moviesCnt = await moviesFetchQuery.CountAsync();

            return moviesCnt;
        }
    }
}
