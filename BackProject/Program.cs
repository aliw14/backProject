using BackProject.Areas.AdminPanel.Data;
using BackProject.Data;
using BackProject.DataAccessLayer;
using BackProject.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BackProject;

public class Program
{
    public async static Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Constants.ImagePath = Path.Combine(builder.Environment.WebRootPath, "img");

        builder.Services.AddMvc();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<AppDbContext>(builder =>
        {
            builder.UseSqlServer(connectionString, builder => builder.MigrationsAssembly("BackProject"));
        });

        builder.Services.AddIdentity<User, IdentityRole>(options =>
        {
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;  

            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedPhoneNumber = false;
            options.SignIn.RequireConfirmedEmail = false;

            options.Lockout.MaxFailedAccessAttempts = 3;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);

        }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders().AddErrorDescriber<LocalizeIdentityError>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var dataInitializer = new DataInitializer(userManager, roleManager, dbContext);
            await dataInitializer.SeedData();
        };

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoint =>
        {
            endpoint.MapControllerRoute(
                         name: "areas",
                         pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

            endpoint.MapControllerRoute("default", "{controller=home}/{action=index}/{id?}");
        });

       await app.RunAsync();
    }
}