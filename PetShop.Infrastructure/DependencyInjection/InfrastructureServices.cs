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

        // Fábrica (singleton) para código acionado por timer/polling fora do fluxo normal
        // de requisição, que não pode compartilhar o DbContext com escopo no circuito —
        // ver MensagemAgendamentoService. O DbContext "normal" com escopo (usado por todo
        // o resto do código) é resolvido a partir dessa mesma fábrica logo abaixo, para
        // não registrar DbContextOptions duas vezes (o que quebra a validação de escopos).
        services.AddDbContextFactory<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped(sp =>
            sp.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());

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
        services.AddTransient<IAgendamentoNotificationService, SmtpAgendamentoNotificationService>();

        return services;
    }
}