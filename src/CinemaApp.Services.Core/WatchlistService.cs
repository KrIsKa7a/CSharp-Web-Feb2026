namespace CinemaApp.Services.Core
{
    using Contracts;
    using Data.Models;
    using Data.Repository.Contracts;
    using GCommon.Exceptions;
    using Models.Watchlist;

    using AutoMapper;

    public class WatchlistService : IWatchlistService
    {
        private readonly IWatchlistRepository watchlistRepository;
        private readonly IMovieRepository movieRepository;

        private readonly IMapper mapper;

        public WatchlistService(IWatchlistRepository watchlistRepository, IMovieRepository movieRepository, IMapper mapper)
        {
            this.watchlistRepository = watchlistRepository;
            this.movieRepository = movieRepository;

            this.mapper = mapper;
        }

        public async Task<IEnumerable<WatchlistMovieDto>> GetUserWatchlistByIdAsync(string userId)
        {
            IEnumerable<Movie> userWatchlist = (await watchlistRepository
                .GetAllUserMoviesAsync(um => um.UserId.ToString() == userId))
                .Select(um => um.Movie)
                .ToArray();

            IEnumerable<WatchlistMovieDto> watchlistMoviesDto = mapper
                .Map<IEnumerable<WatchlistMovieDto>>(userWatchlist);

            return watchlistMoviesDto;
        }

        public async Task AddMovieToUserWatchlistAsync(string userId, Guid movieId)
        {
            UserMovie? userMovie = await watchlistRepository
                .GetUserMovieIncludeDeletedAsync(userId, movieId);
            if (userMovie != null && userMovie.IsDeleted == false)
            {
                throw new EntityAlreadyExistsException();
            }

            bool movieExists = await movieRepository
                .ExistsByIdAsync(movieId);
            if (!movieExists)
            {
                throw new EntityNotFoundException();
            }

            bool successPersist = false;
            if (userMovie == null)
            {
                UserMovie newUserMovie = new UserMovie()
                {
                    UserId = Guid.Parse(userId),
                    MovieId = movieId
                };

                successPersist = await watchlistRepository
                    .AddUserMovieAsync(newUserMovie);
            }
            else
            {
                // Recover soft-deleted entry
                userMovie.IsDeleted = false;

                successPersist = await watchlistRepository
                    .UpdateUserMovieAsync(userMovie);
            }

            if (!successPersist)
            {
                throw new EntityPersistFailureException();
            }
        }

        public async Task RemoveMovieFromUserWatchlistAsync(string userId, Guid movieId)
        {
            UserMovie? userMovie = await watchlistRepository
                .GetUserMovieAsync(userId, movieId);
            if (userMovie == null)
            {
                throw new EntityNotFoundException();
            }

            bool successDelete = await watchlistRepository
                .SoftDeleteUserMovieAsync(userMovie);
            if (!successDelete)
            {
                throw new EntityPersistFailureException();
            }
        }

        public async Task<bool> MovieIsInUserWatchlistAsync(string userId, Guid movieId)
        {
            try
            {
                bool userWatchlistEntryExists = await watchlistRepository
                    .ExistsAsync(userId, movieId);

                return userWatchlistEntryExists;
            }
            catch (NullReferenceException nre)
            {
                throw new EntityKeyNullOrEmptyException(nre.Message);
            }
        }
    }
}
