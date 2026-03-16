namespace CinemaApp.Data.Configuration
{
    using Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CinemaConfiguration : IEntityTypeConfiguration<Cinema>
    {
        public void Configure(EntityTypeBuilder<Cinema> entity)
        {
            entity.HasQueryFilter(c => c.IsDeleted == false);

            entity.HasData(SeedCinemas());
        }

        private IEnumerable<Cinema> SeedCinemas()
        {
            return new List<Cinema>
            {
                new Cinema
                {
                    Id = Guid.Parse("86e9d655-4bec-4685-b42f-40f93efedda2"),
                    Name = "Grand Cinema",
                    Location = "Downtown",
                    IsDeleted = false
                },
                new Cinema
                {
                    Id = Guid.Parse("ccb61fcf-9bd8-4008-88e8-dc69c9d24566"),
                    Name = "Movie Palace",
                    Location = "Uptown",
                    IsDeleted = false
                },
                new Cinema
                {
                    Id = Guid.Parse("e2e63228-9ddf-491c-888a-f8077c53430e"),
                    Name = "CinemaX",
                    Location = "Suburb",
                    IsDeleted = false
                }
            };
        }
    }
}
