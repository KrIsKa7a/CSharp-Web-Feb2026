namespace CinemaApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class CinemaMovieConfiguration : IEntityTypeConfiguration<CinemaMovie>
    {
        public static readonly IEnumerable<CinemaMovie> CinemaMoviesSeedData = new List<CinemaMovie>()
        {
            new CinemaMovie { Id = Guid.Parse("5465ab3a-21ce-4cd5-b35b-c5967f1ce98e"), CinemaId = Guid.Parse("e51cd89f-1495-4f8a-8e92-93a1d56d53f1"), MovieId = Guid.Parse("4571bf2f-dbb3-446c-a92a-07cb77f47ed0"), AvailableTickets = 120, Showtime = DateTime.Parse("2026-11-14T19:00:00") },
            new CinemaMovie { Id = Guid.Parse("2ff9ec78-3226-4f7d-a8d4-b7744bc40616"), CinemaId = Guid.Parse("e51cd89f-1495-4f8a-8e92-93a1d56d53f1"), MovieId = Guid.Parse("e673a37c-8ffc-46bf-a66d-18b8078d7c94"), AvailableTickets = 100, Showtime = DateTime.Parse("2026-11-15T20:30:00") },
            new CinemaMovie { Id = Guid.Parse("da876235-1f08-4e0f-b177-c078bbdeb0f1"), CinemaId = Guid.Parse("e51cd89f-1495-4f8a-8e92-93a1d56d53f1"), MovieId = Guid.Parse("2f22c0dd-f7b9-46c3-a753-0d076dafb489"), AvailableTickets = 110, Showtime = DateTime.Parse("2026-11-16T18:15:00") },

            new CinemaMovie { Id = Guid.Parse("b97e76f3-3646-4267-8f59-76a37c8e70dc"), CinemaId = Guid.Parse("76fa26f7-f48f-4397-bf96-744a74128234"), MovieId = Guid.Parse("4571bf2f-dbb3-446c-a92a-07cb77f47ed0"), AvailableTickets = 130, Showtime = DateTime.Parse("2026-11-14T21:00:00") },
            new CinemaMovie { Id = Guid.Parse("302763cb-a86b-4f4b-a66b-f44fbd949836"), CinemaId = Guid.Parse("76fa26f7-f48f-4397-bf96-744a74128234"), MovieId = Guid.Parse("e673a37c-8ffc-46bf-a66d-18b8078d7c94"), AvailableTickets = 90, Showtime = DateTime.Parse("2026-11-15T19:30:00") },
            new CinemaMovie { Id = Guid.Parse("4e203f0f-fafe-44f9-b26c-f4e8a1f2da76"), CinemaId = Guid.Parse("76fa26f7-f48f-4397-bf96-744a74128234"), MovieId = Guid.Parse("2f22c0dd-f7b9-46c3-a753-0d076dafb489"), AvailableTickets = 125, Showtime = DateTime.Parse("2026-11-16T20:00:00") },

            new CinemaMovie { Id = Guid.Parse("4fc4cc82-f924-4875-8abc-f2d15898bc7d"), CinemaId = Guid.Parse("5ad7fbe7-f76f-4e98-826f-b85de6169939"), MovieId = Guid.Parse("4571bf2f-dbb3-446c-a92a-07cb77f47ed0"), AvailableTickets = 140, Showtime = DateTime.Parse("2026-11-14T18:30:00") },
            new CinemaMovie { Id = Guid.Parse("5e6a89cc-f9fd-4f49-9877-e21f211c5cd4"), CinemaId = Guid.Parse("5ad7fbe7-f76f-4e98-826f-b85de6169939"), MovieId = Guid.Parse("e673a37c-8ffc-46bf-a66d-18b8078d7c94"), AvailableTickets = 95, Showtime = DateTime.Parse("2026-11-15T21:15:00") },
            new CinemaMovie { Id = Guid.Parse("8051818b-9f0f-417f-8ee3-117d3fbbf154"), CinemaId = Guid.Parse("5ad7fbe7-f76f-4e98-826f-b85de6169939"), MovieId = Guid.Parse("2f22c0dd-f7b9-46c3-a753-0d076dafb489"), AvailableTickets = 115, Showtime = DateTime.Parse("2026-11-16T19:45:00") }
        };

        public void Configure(EntityTypeBuilder<CinemaMovie> builder)
        {
            builder
                .HasQueryFilter(cm => cm.IsDeleted == false &&
                                      cm.Movie.IsDeleted == false &&
                                      cm.Cinema.IsDeleted == false);

            builder
                .HasIndex(cm => new { cm.CinemaId, cm.MovieId, cm.Showtime })
                .IsUnique();

            builder.HasData(CinemaMoviesSeedData);
        }
    }
}
