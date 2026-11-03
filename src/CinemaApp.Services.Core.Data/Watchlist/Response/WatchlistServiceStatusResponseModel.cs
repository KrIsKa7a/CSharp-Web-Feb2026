namespace CinemaApp.Services.Core.Data.Watchlist.Response
{
    using Enums;

    public class WatchlistServiceStatusResponseModel
    {
        public WatchlistServiceResponseStatus Status { get; set; }

        public ICollection<string?> ErrorMessages { get; set; }
            = new List<string?>();
    }
}
