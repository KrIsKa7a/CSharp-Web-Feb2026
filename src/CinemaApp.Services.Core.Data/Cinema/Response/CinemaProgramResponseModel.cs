namespace CinemaApp.Services.Core.Data.Cinema.Response
{
    public class CinemaProgramResponseModel
    {
        public IEnumerable<CinemaMovieResponseData> Movies { get; set; }
            = new List<CinemaMovieResponseData>();
    }
}
