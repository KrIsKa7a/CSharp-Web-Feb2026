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
            IEnumerable<Movie> userWatchlist = watchlistRepository
                .GetAllUserMoviesAsync()
                .GetAwaiter()
                .GetResult()
                .Where(um => um.UserId.ToLower() == userId.ToLower())
                .Select(um => um.Movie)
                .ToArray();

            IEnumerable<WatchlistMovieDto> watchlistMoviesDto = mapper
                .Map<IEnumerable<WatchlistMovieDto>>(userWatchlist);

            return watchlistMoviesDto;
        }
        public async Task AddMovieToUserWatchlistAsync(string userId, Guid movieId)
        {
            bool userWatchlistEntryExists = await watchlistRepository
                .ExistsAsync(userId, movieId);
            if (userWatchlistEntryExists)
            {
                throw new EntityAlreadyExistsException();
            }

            bool movieExists = await movieRepository
                .ExistsByIdAsync(movieId);
            if (!movieExists)
            {
                throw new EntityNotFoundException();
            }

            UserMovie newUserMovie = new UserMovie()
            {
                UserId = userId,
                MovieId = movieId
            };

            bool successAdd = await watchlistRepository
                .AddUserMovieAsync(newUserMovie);
            if (!successAdd)
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
