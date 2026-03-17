namespace CinemaApp.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    
    using Microsoft.EntityFrameworkCore;

    [PrimaryKey(nameof(UserId), nameof(MovieId))]
    public class UserMovie
    {
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;

        [ForeignKey(nameof(Movie))]
        public Guid MovieId { get; set; }

        public virtual Movie Movie { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}
