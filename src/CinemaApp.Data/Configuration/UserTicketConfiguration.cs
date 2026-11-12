namespace CinemaApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class UserTicketConfiguration : IEntityTypeConfiguration<UserTicket>
    {
        public void Configure(EntityTypeBuilder<UserTicket> builder)
        {
            builder.HasQueryFilter(ut => ut.Ticket.Projection.IsDeleted == false);
        }
    }
}
