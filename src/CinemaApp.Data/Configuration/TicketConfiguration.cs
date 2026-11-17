namespace CinemaApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public static readonly IEnumerable<Ticket> TicketsSeedData = new List<Ticket>()
        {
            new Ticket { Id = Guid.Parse("165f6f04-f4a6-4af8-b664-b66dc3298fb5"), ProjectionId = Guid.Parse("5465ab3a-21ce-4cd5-b35b-c5967f1ce98e"), Price = 12.00m },
            new Ticket { Id = Guid.Parse("2fb26c8e-9465-4f1e-88e2-8d370ddfd750"), ProjectionId = Guid.Parse("2ff9ec78-3226-4f7d-a8d4-b7744bc40616"), Price = 11.50m },
            new Ticket { Id = Guid.Parse("39d7396a-cf8a-47a4-b0a6-6a14c9429ffc"), ProjectionId = Guid.Parse("da876235-1f08-4e0f-b177-c078bbdeb0f1"), Price = 12.50m },

            new Ticket { Id = Guid.Parse("4a5e1906-aa85-4258-bdf7-e923aa9f7113"), ProjectionId = Guid.Parse("b97e76f3-3646-4267-8f59-76a37c8e70dc"), Price = 13.00m },
            new Ticket { Id = Guid.Parse("54b10d8f-d10e-4b0a-a459-6dcbe9e4e943"), ProjectionId = Guid.Parse("302763cb-a86b-4f4b-a66b-f44fbd949836"), Price = 11.00m },
            new Ticket { Id = Guid.Parse("61f9b8ec-f906-4f21-92c3-8ec95f59e086"), ProjectionId = Guid.Parse("4e203f0f-fafe-44f9-b26c-f4e8a1f2da76"), Price = 12.75m },

            new Ticket { Id = Guid.Parse("71e6f7db-fc70-4f42-927d-96d1b943c5bb"), ProjectionId = Guid.Parse("4fc4cc82-f924-4875-8abc-f2d15898bc7d"), Price = 13.50m },
            new Ticket { Id = Guid.Parse("841f7d89-2172-4f27-a80d-9bafe2fd89de"), ProjectionId = Guid.Parse("5e6a89cc-f9fd-4f49-9877-e21f211c5cd4"), Price = 11.25m },
            new Ticket { Id = Guid.Parse("9af8d2c6-ee22-4f56-af8c-945d45d52b60"), ProjectionId = Guid.Parse("8051818b-9f0f-417f-8ee3-117d3fbbf154"), Price = 12.25m }
        };

        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasQueryFilter(t => t.Projection.IsDeleted == false);

            builder.HasData(TicketsSeedData);
        }
    }
}
