namespace CinemaApp.Data.Configuration
{
    using Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProjectionConfiguration : IEntityTypeConfiguration<Projection>
    {
        public void Configure(EntityTypeBuilder<Projection> entity)
        {
            entity
                .HasIndex(cm => new { cm.CinemaId, cm.MovieId, cm.Showtime }, "IX_CinemaMovie_Mapping_Unique")
                .IsUnique();

            entity
                .HasQueryFilter(p => p.IsDeleted == false && p.Movie.IsDeleted == false && p.Cinema.IsDeleted == false);
        }
    }
}
