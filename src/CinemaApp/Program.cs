namespace CinemaApp.Web
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Data.Seeding.Utilities;
    using Infrastructure.Extensions;
    using Infrastructure.Filters;
    using Services.Core;
    using Services.Core.Contracts;

    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args); 
            string connectionString = builder.Configuration
                .GetConnectionString("SqlServerConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            
            builder.Services.AddDbContext<CinemaAppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.RegisterRepositoriesFromAssembly(typeof(CinemaAppDbContext).Assembly);

            /* TODO: Implement Automatic Service Discovery with Reflection */
            builder.Services.AddScoped<IMovieService, MovieService>();
            builder.Services.AddScoped<IWatchlistService, WatchlistService>();
            builder.Services.AddScoped<ICinemaService, CinemaService>();
            builder.Services.AddScoped<ITicketService, TicketService>();
            builder.Services.AddScoped<IProjectionService, ProjectionService>();

            /* Setup of example CORS policy */
            /* .AllowAnyOrigin() should be used only for CDN API Endpoint */
            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("AllowSoftUniDomain", cfg =>
                {
                    cfg.WithOrigins("https://softuni.bg")
                        .WithMethods("GET", "POST")
                        .AllowAnyHeader();
                });
            });

            builder.Services.AddScoped<PhoneRequiredActionFilter>();

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    ConfigureIdentityOptions(builder.Configuration, options);
                })
                .AddRoles<IdentityRole>()
                .AddUserManager<UserManager<IdentityUser>>()
                .AddSignInManager<SignInManager<IdentityUser>>()
                .AddDefaultTokenProviders()
                .AddDefaultUI()
                .AddEntityFrameworkStores<CinemaAppDbContext>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();

                app.UseSwagger();
                app.UseSwaggerUI();
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

            app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAdminHomePageRedirection("/Admin/Home");

            app.UseCors("AllowSoftUniDomain");

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            UseDefaultRoles(app, app.Configuration);

            app.Run();
        }

        private static void ConfigureIdentityOptions(ConfigurationManager configuration,
            IdentityOptions options)
        {
            options.SignIn.RequireConfirmedAccount = configuration
                .GetValue<bool>("IdentitySettings:SignIn:RequireConfirmedAccount");

            options.Password.RequireDigit = configuration
                .GetValue<bool>("IdentitySettings:Password:RequireDigit");
            options.Password.RequireLowercase = configuration
                .GetValue<bool>("IdentitySettings:Password:RequireLowercase");
            options.Password.RequireUppercase = configuration
                .GetValue<bool>("IdentitySettings:Password:RequireUppercase");
            options.Password.RequireNonAlphanumeric = configuration
                .GetValue<bool>("IdentitySettings:Password:RequireNonAlphanumeric");
            options.Password.RequiredLength = configuration
                .GetValue<int>("IdentitySettings:Password:RequiredLength");
            options.Password.RequiredUniqueChars = configuration
                .GetValue<int>("IdentitySettings:Password:RequiredUniqueChars");

            options.User.RequireUniqueEmail = configuration
                .GetValue<bool>("IdentitySettings:User:RequireUniqueEmail");
        }

        private static void UseDefaultRoles(WebApplication app, IConfiguration configuration)
        {
            using IServiceScope serviceScope = app.Services.CreateScope();
            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            string adminEmail = configuration.GetValue<string>("UserSeed:AdminAccount:Email") 
                                ?? throw new ArgumentException();
            string adminPassword = configuration.GetValue<string>("UserSeed:AdminAccount:Password")
                                   ?? throw new ArgumentException();

            IdentitySeeder.SeedRoles(serviceProvider);
            IdentitySeeder.SeedAdminUser(serviceProvider, adminEmail, adminPassword);
        }
    }
}
