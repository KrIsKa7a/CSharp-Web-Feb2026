namespace CinemaApp.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static Common.EntityValidation.Cinema;

    public class Cinema
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(LocationMaxLength)]
        public string Location { get; set; } = null!;

        public bool IsDeleted { get; set; }

        [ForeignKey(nameof(Manager))]
        public Guid? ManagerId { get; set; }

        public virtual Manager? Manager { get; set; }

        public virtual ICollection<Projection> Projections { get; set; }
            = new HashSet<Projection>();
    }
}
