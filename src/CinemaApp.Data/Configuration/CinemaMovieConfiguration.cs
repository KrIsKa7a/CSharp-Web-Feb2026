namespace CinemaApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class CinemaMovieConfiguration : IEntityTypeConfiguration<CinemaMovie>
    {
        public void Configure(EntityTypeBuilder<CinemaMovie> builder)
        {
            builder
                .HasQueryFilter(cm => cm.IsDeleted == false &&
                                      cm.Movie.IsDeleted == false &&
                                      cm.Cinema.IsDeleted == false);
        }
    }
}
