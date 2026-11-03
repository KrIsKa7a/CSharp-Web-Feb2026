namespace CinemaApp.Services.Core.Data.Movie.Response
{
    using Enums;

    public class MovieServiceStatusResponseModel
    {
        public MovieServiceResponseStatus Status { get; set; }

        public IEnumerable<string?> ErrorMessages { get; set; }
            = new List<string?>();
    }
}
