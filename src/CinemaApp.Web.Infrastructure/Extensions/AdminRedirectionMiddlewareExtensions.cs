namespace CinemaApp.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Builder;

    using Middlewares;

    public static class AdminRedirectionMiddlewareExtensions
    {
        public static IApplicationBuilder UseAdminHomePageRedirection(this IApplicationBuilder app, string adminHomePagePath)
        {
            app.UseMiddleware<AdminRedirectionMiddleware>(adminHomePagePath);

            return app;
        }
    }
}
