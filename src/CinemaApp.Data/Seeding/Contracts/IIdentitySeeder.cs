namespace CinemaApp.Data.Seeding.Contracts
{
    public interface IIdentitySeeder
    {
        Task SeedRolesAsync();

        Task SeedAdminUserAsync();
    }
}
