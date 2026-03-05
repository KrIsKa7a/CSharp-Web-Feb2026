namespace CinemaApp.Data.Configuration
{
    using Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserMovieConfiguration : IEntityTypeConfiguration<UserMovie>
    {
        public void Configure(EntityTypeBuilder<UserMovie> entity)
        {
            entity
                .HasQueryFilter(um => um.IsDeleted == false && um.Movie.IsDeleted == false);
        }
    }
}
