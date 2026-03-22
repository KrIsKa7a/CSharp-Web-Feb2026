namespace CinemaApp.Services.Core
{
    using System.Linq.Expressions;

    using Contracts;
    using Data.Models;
    using Data.Repository.Contracts;
    using GCommon.Exceptions;
    using Models.Movie;
    using static GCommon.ApplicationConstants;

    using AutoMapper;

    public class MovieService : IMovieService
    {
        private readonly IMovieRepository movieRepository;
        private readonly IWatchlistRepository watchlistRepository;

        private readonly IMapper mapper;

        public MovieService(IMovieRepository movieRepository, IWatchlistRepository watchlistRepository, IMapper mapper)
        {
            this.movieRepository = movieRepository;
            this.watchlistRepository = watchlistRepository;

            this.mapper = mapper;
        }

        public async Task<IEnumerable<MovieAllDto>> GetAllMoviesOrderedByTitleAsync(string? userId = null, 
            string? searchQuery = null, int pageNumber = 1, int? moviesPerPage = DefaultEntitiesPerPage,
            bool includeDeleted = false)
        {
            Expression<Func<Movie, bool>>? filterQuery = null;
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim().ToLowerInvariant();
                filterQuery = m => (m.Title.ToLower().Contains(searchQuery)) ||
                                   (m.Genre.ToLower().Contains(searchQuery)) ||
                                    (m.Director.ToLower().Contains(searchQuery));
            }

            int? skipCnt = (pageNumber - 1) * moviesPerPage;

            // Fetch data
			IEnumerable<Movie> allMoviesDb = await movieRepository
                .GetAllMoviesNoTrackingWithProjectionAsync(
                    projectionQuery: m => new Movie()
                    {
                        Id = m.Id,
                        Title = m.Title,
                        Genre = m.Genre,
                        ReleaseDate = m.ReleaseDate,
                        Director = m.Director,
                        ImageUrl = m.ImageUrl ?? DefaultImageUrl,
                        Duration = m.Duration,
                        IsDeleted = m.IsDeleted,
                    },
                    filterQuery: filterQuery,
                    skipCnt: skipCnt,
                    takeCnt: moviesPerPage,
                    ignoreQueryFilters: includeDeleted);
            IEnumerable<UserMovie> allUserMovies = (await watchlistRepository
                .GetAllUserMoviesAsync(um => um.UserId.ToString() == userId))
                .ToHashSet();

            // Process data
            IEnumerable<MovieAllDto> allMoviesDtos = mapper
                .Map<IEnumerable<MovieAllDto>>(allMoviesDb)
                .OrderBy(m => m.Title)
                .ThenBy(m => m.Genre)
                .ThenBy(m => m.Director)
                .ToArray();
            if (!string.IsNullOrWhiteSpace(userId))
            {
                foreach (MovieAllDto movieDto in allMoviesDtos)
                {
                    movieDto.IsInUserWatchlist = allUserMovies
                        .Any(um => um.MovieId == movieDto.Id && 
                                   um.UserId.ToString().ToLowerInvariant() == userId.ToLowerInvariant());
                }
            }

            // Return processed data
            return allMoviesDtos;
        }

        public async Task<int> GetMoviesCountAsync(string? searchQuery = null)
        {
            Expression<Func<Movie, bool>>? filterQuery = null;
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim().ToLowerInvariant();
                filterQuery = m => (m.Title.ToLower().Contains(searchQuery)) ||
                                   (m.Genre.ToLower().Contains(searchQuery)) ||
                                   (m.Director.ToLower().Contains(searchQuery));
            }

            int moviesCnt = await movieRepository
                .CountAsync(filterQuery);

            return moviesCnt;
        }

        public async Task CreateMovieAsync(MovieDetailsDto movieDetailsDto)
        {
            Movie newMovie = mapper
                .Map<Movie>(movieDetailsDto);

            bool successAdd = await movieRepository.AddMovieAsync(newMovie);
            if (!successAdd)
            {
                throw new EntityPersistFailureException();
            }
        }

        public async Task<MovieDetailsDto?> GetMovieDetailsByIdAsync(Guid id)
        {
	        Movie? movieDb = await movieRepository
		        .GetMovieByIdAsync(id);

	        if (movieDb == null)
	        {
		        return null;
	        }

            return mapper.Map<MovieDetailsDto>(movieDb);
        }

        public async Task<MovieDetailsDto?> GetMovieFormModelByIdAsync(Guid id, bool includeDeleted = false)
        {
            Movie? movieDb = await movieRepository
                .GetMovieByIdAsync(id, includeDeleted);

            if (movieDb == null)
            {
                return null;
            }

            return mapper
                .Map<MovieDetailsDto>(movieDb);
        }

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            return await movieRepository.ExistsByIdAsync(id);
        }

        public async Task EditMovieAsync(Guid id, MovieDetailsDto movieDetailsDto, bool includeDeleted = false)
        {
            Movie? movieDb = await movieRepository
                .GetMovieByIdAsync(id, includeDeleted);
            if (movieDb == null)
            {
                throw new EntityNotFoundException();
            }

            movieDb.Title = movieDetailsDto.Title;
            movieDb.Genre = movieDetailsDto.Genre;
            movieDb.ReleaseDate = movieDetailsDto.ReleaseDate;
            movieDb.Description = movieDetailsDto.Description;
            movieDb.Duration = movieDetailsDto.Duration;
            movieDb.Director = movieDetailsDto.Director;
            movieDb.ImageUrl = movieDetailsDto.ImageUrl;
            movieDb.IsDeleted = movieDetailsDto.IsDeleted;
            
            bool editSuccess = await movieRepository.EditMovieAsync(movieDb);
            if (!editSuccess)
            {
                throw new EntityPersistFailureException();
            }
        }

        public async Task SoftDeleteMovieAsync(Guid id)
        {
            Movie? movieDb = await movieRepository
                .GetMovieByIdAsync(id);
            if (movieDb == null)
            {
                throw new EntityNotFoundException();
            }

            bool deleteSuccess = await movieRepository.SoftDeleteMovieAsync(movieDb);
            if (!deleteSuccess)
            {
                throw new EntityPersistFailureException();
            }
        }

        public async Task HardDeleteMovieAsync(Guid id)
        {
            Movie? movieDb = await movieRepository
                .GetMovieByIdAsync(id);
            if (movieDb == null)
            {
                throw new EntityNotFoundException();
            }

            bool deleteSuccess = await movieRepository.HardDeleteMovieAsync(movieDb);
            if (!deleteSuccess)
            {
                throw new EntityPersistFailureException();
            }
        }
    }
}
