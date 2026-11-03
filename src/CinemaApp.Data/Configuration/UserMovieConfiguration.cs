namespace CinemaApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class UserMovieConfiguration : IEntityTypeConfiguration<UserMovie>
    {
        public void Configure(EntityTypeBuilder<UserMovie> builder)
        {
            /* Low Performance Solution: Applies overhead due to fetching Movie.IsDeleted on each request */
            builder.HasQueryFilter(um => um.Movie.IsDeleted == false);
        }
    }
}
