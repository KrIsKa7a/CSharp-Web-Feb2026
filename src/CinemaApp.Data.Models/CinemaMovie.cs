namespace CinemaApp.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.EntityFrameworkCore;

    using Common.Models;
    using static Common.EntityValidation.CinemaMovie;

    [PrimaryKey(nameof(MovieId), nameof(CinemaId))]
    public class CinemaMovie : ISoftDeleteableEntity
    {
        [ForeignKey(nameof(Movie))]
        public Guid MovieId { get; set; }

        public virtual Movie Movie { get; set; } = null!;

        [ForeignKey(nameof(Cinema))]
        public Guid CinemaId { get; set; }

        public virtual Cinema Cinema { get; set; } = null!;

        public int AvailableTickets { get; set; }

        public bool IsDeleted { get; set; }

        [Column(TypeName = ShowtimeColumnType)]
        public DateTime Showtime { get; set; }
    }
}
