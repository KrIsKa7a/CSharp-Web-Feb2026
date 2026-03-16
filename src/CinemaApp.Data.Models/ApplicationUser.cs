namespace CinemaApp.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser<Guid>
    {
        public DateTime Birthdate { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
            = new List<Ticket>();

        public virtual ICollection<UserMovie> UserWatchlist { get; set; }
            = new List<UserMovie>();
    }
}
