namespace CinemaApp.Web.Infrastructure.Middlewares
{
    using Microsoft.AspNetCore.Http;

    using static GCommon.ApplicationConstants;

    public class AdminRedirectionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly string redirectPath;

        public AdminRedirectionMiddleware(RequestDelegate next, string redirectPath)
        {
            this.next = next;
            this.redirectPath = redirectPath;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == ApplicationHomePagePath)
            {
                bool isAuthenticated = context.User?.Identity?.IsAuthenticated ?? false;
                if (isAuthenticated && context.User!.IsInRole(AdminRoleName))
                {
                    context.Response.Redirect(this.redirectPath);
                    return;
                }
            }

            await next(context);
        }
    }
}
