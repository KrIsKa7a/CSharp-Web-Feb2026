namespace CinemaApp.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Manager
    {
        [Key]
        public Guid Id { get; set; }

        public int Level { get; set; }

        public Guid UserId { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;

        public virtual ICollection<Cinema> ManagedCinemas { get; set; }
            = new HashSet<Cinema>();
    }
}
