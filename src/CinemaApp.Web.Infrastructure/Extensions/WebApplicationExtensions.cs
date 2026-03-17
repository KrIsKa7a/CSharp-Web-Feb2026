namespace CinemaApp.Web.Infrastructure.Extensions
{
    using Data.Seeding.Contracts;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class WebApplicationExtensions
    {
        public static IApplicationBuilder UseRolesSeeder(this IApplicationBuilder applicationBuilder)
        {
            using IServiceScope scope = applicationBuilder
                .ApplicationServices
                .CreateScope();
            IIdentitySeeder identitySeeder = scope
                .ServiceProvider
                .GetRequiredService<IIdentitySeeder>();

            identitySeeder
                .SeedRolesAsync()
                .GetAwaiter()
                .GetResult();

            return applicationBuilder;
        }

        public static IApplicationBuilder UseAdminUserSeeder(this IApplicationBuilder applicationBuilder)
        {
            using IServiceScope scope = applicationBuilder
                .ApplicationServices
                .CreateScope();
            IIdentitySeeder identitySeeder = scope
                .ServiceProvider
                .GetRequiredService<IIdentitySeeder>();

            identitySeeder
                .SeedAdminUserAsync()
                .GetAwaiter()
                .GetResult();

            return applicationBuilder;
        }
    }
}
