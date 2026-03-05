namespace CinemaApp.Services.Core
{
    using Contracts;
    using Data.Models;
    using Data.Repository.Contracts;
    using Models.Watchlist;

    using AutoMapper;

    public class WatchlistService : IWatchlistService
    {
        private readonly IWatchlistRepository watchlistRepository;
        private readonly IMapper mapper;

        public WatchlistService(IWatchlistRepository watchlistRepository, IMapper mapper)
        {
            this.watchlistRepository = watchlistRepository;
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
    }
}
