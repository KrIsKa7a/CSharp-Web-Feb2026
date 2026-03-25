namespace CinemaApp.IntegrationTests
{
    using Data;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public class CinemaAppWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
        where TProgram : class
    {
        private readonly string inMemoryDatabaseName = $"CinemaApp_InMemory_{Guid.NewGuid()}";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the original application DbContext configuration
                ServiceDescriptor? dbContextDescriptor = services
                    .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<CinemaAppDbContext>));
                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                }

                services.AddDbContext<CinemaAppDbContext>(opt =>
                {
                    opt.UseInMemoryDatabase(inMemoryDatabaseName, b => b.EnableNullChecks(false));
                });

                IServiceProvider serviceProvider = services.BuildServiceProvider();

                using IServiceScope scope = serviceProvider.CreateScope();
                CinemaAppDbContext dbContext = scope
                    .ServiceProvider
                    .GetRequiredService<CinemaAppDbContext>();

                dbContext.Database.EnsureCreated();

                // Arrange: Seed the in-memory database with additional test data
            });
        }
    }
}
