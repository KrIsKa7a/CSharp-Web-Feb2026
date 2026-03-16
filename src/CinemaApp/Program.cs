namespace CinemaApp.Web
{
    using Data;
    using Data.Models;
    using Data.Repository;
    using Data.Seeding;
    using Data.Seeding.Contracts;
    using Infrastructure.Extensions;
    using Services.Core;
    using Services.Mapping;
    using Services.Models.Movie;
    using ViewModels.Movie;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            string connectionString = builder.Configuration
                .GetConnectionString("SqlServer") ?? throw new InvalidOperationException("Connection string 'SqlServer' not found.");

            AutoMapperConfig.RegisterMappings(typeof(MovieAllDto).Assembly, typeof(AllMoviesIndexViewModel).Assembly);

            builder.Services.AddDbContext<CinemaAppDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.RegisterRepositories(typeof(MovieRepository));
            builder.Services.RegisterUserServices(typeof(MovieService));

            builder.Services.AddTransient<IIdentitySeeder, IdentitySeeder>();

            builder.Services.AddSingleton(AutoMapperConfig.MapperInstance);

            builder.Services
                .AddDefaultIdentity<ApplicationUser>(options =>
                {
                    ConfigureIdentity(builder.Configuration, options);
                })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<CinemaAppDbContext>();
            builder.Services.AddControllersWithViews();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRolesSeeder();
            app.UseAdminUserSeeder();

            app.UseStatusCodePagesWithRedirects("/Home/Error/{0}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }

        private static void ConfigureIdentity(ConfigurationManager configuration,
            IdentityOptions options)
        {
            options.SignIn.RequireConfirmedAccount = configuration
                .GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");
            options.SignIn.RequireConfirmedEmail = configuration
                .GetValue<bool>("Identity:SignIn:RequireConfirmedEmail");
            options.SignIn.RequireConfirmedPhoneNumber = configuration
                .GetValue<bool>("Identity:SignIn:RequireConfirmedPhoneNumber");

            options.Password.RequireDigit = configuration
                .GetValue<bool>("Identity:Password:RequireDigit");
            options.Password.RequiredLength = configuration
                .GetValue<int>("Identity:Password:RequiredLength");
            options.Password.RequiredUniqueChars = configuration
                .GetValue<int>("Identity:Password:RequiredUniqueChars");
            options.Password.RequireLowercase = configuration
                .GetValue<bool>("Identity:Password:RequireLowercase");
            options.Password.RequireNonAlphanumeric = configuration
                .GetValue<bool>("Identity:Password:RequireNonAlphanumeric");
            options.Password.RequireUppercase = configuration
                .GetValue<bool>("Identity:Password:RequireUppercase");
        }
    }
}
