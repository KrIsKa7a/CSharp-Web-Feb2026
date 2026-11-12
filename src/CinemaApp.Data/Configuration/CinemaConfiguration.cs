namespace CinemaApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class CinemaConfiguration : IEntityTypeConfiguration<Cinema>
    {
        public static readonly IEnumerable<Cinema> CinemasSeedData = new List<Cinema>()
        {
            new Cinema { Id = Guid.Parse("e51cd89f-1495-4f8a-8e92-93a1d56d53f1"), Name = "Cine Grand Mall", Location = "Sofia" },
            new Cinema { Id = Guid.Parse("76fa26f7-f48f-4397-bf96-744a74128234"), Name = "Movie Palace", Location = "Plovdiv" },
            new Cinema { Id = Guid.Parse("5ad7fbe7-f76f-4e98-826f-b85de6169939"), Name = "Sea View Cinema", Location = "Varna" }
        };

        public void Configure(EntityTypeBuilder<Cinema> builder)
        {
            builder.HasQueryFilter(c => c.IsDeleted == false);

            builder.HasData(CinemasSeedData);
        }
    }
}
