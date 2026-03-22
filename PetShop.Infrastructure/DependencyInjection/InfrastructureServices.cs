using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetShop.Infrastructure.Persistence;
using PetShop.Infrastructure.Services;

namespace PetShop.Infrastructure.DependencyInjection;

public static class InfrastructureServices
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Identity
        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddSignInManager()
        .AddDefaultTokenProviders();

        // Email
        services.AddTransient<IEmailSender<ApplicationUser>, SmtpEmailSender>();

        return services;
    }
}