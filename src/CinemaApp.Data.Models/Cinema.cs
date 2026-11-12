namespace CinemaApp.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Common.Models;
    using static Common.EntityValidation.Cinema;

    public class Cinema : ISoftDeleteableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(LocationMaxLength)]
        public string Location { get; set; } = null!;

        public bool IsDeleted { get; set; }

        public virtual ICollection<CinemaMovie> CinemaProjections { get; set; }
            = new HashSet<CinemaMovie>();
    }
}
