namespace CinemaApp.Data.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> entity)
        {
            entity
                .HasOne(m => m.User)
                .WithOne()
                .HasForeignKey(typeof(Manager), "UserId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
