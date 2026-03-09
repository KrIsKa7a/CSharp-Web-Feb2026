namespace CinemaApp.Services.Models.Movie
{
    public class MovieDetailsDto : MovieAllDto
    {
        public string Description { get; set; } = null!;

        public int Duration { get; set; }
    }
}
