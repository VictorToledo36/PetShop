using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PetShop.Infrastructure.Persistence;

public static class SeedData
{
    public const string RoleAdmin = "Admin";
    public const string RoleCliente = "Cliente";

    public static async Task InicializarAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var config = services.GetRequiredService<IConfiguration>(); // ← lê appsettings

        // Lê os dados do appsettings.json
        var adminEmail = config["AdminSeed:Email"]
            ?? throw new Exception("AdminSeed:Email não configurado!");
        var adminSenha = config["AdminSeed:Senha"]
            ?? throw new Exception("AdminSeed:Senha não configurada!");
        var adminNome = config["AdminSeed:NomeCompleto"] ?? "Administrador";

        await CriarRoleSeNaoExistirAsync(roleManager, RoleAdmin);
        await CriarRoleSeNaoExistirAsync(roleManager, RoleCliente);
        await CriarAdminAsync(userManager, adminEmail, adminSenha, adminNome);
    }

    private static async Task CriarRoleSeNaoExistirAsync(
        RoleManager<IdentityRole> roleManager, string role)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    private static async Task CriarAdminAsync(
        UserManager<ApplicationUser> userManager,
        string adminEmail,
        string adminSenha,
        string adminNome)
    {
        // Se já existe, não cria de novo
        var adminExistente = await userManager.FindByEmailAsync(adminEmail);
        if (adminExistente is not null) return;

        var admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            NomeCompleto = adminNome,
            EmailConfirmed = true,
            DataCadastro = DateTime.UtcNow
        };

        var resultado = await userManager.CreateAsync(admin, adminSenha);

        if (resultado.Succeeded)
            await userManager.AddToRoleAsync(admin, RoleAdmin);
        else
            throw new Exception($"Falha ao criar Admin: " +
                string.Join(", ", resultado.Errors.Select(e => e.Description)));
    }
}