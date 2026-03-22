using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using PetShot.Web.Components;
using PetShot.Web.Components.Account;
using PetShop.Infrastructure.DependencyInjection;
using PetShop.Application.Services;
using PetShop.Application.Interfaces;
using PetShop.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider,
    IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddIdentityCookies();

// ← Tudo de Infrastructure em uma linha só
builder.Services.AddInfrastructure(builder.Configuration);

// Application Services
builder.Services.AddScoped<IPetService, PetService>();
builder.Services.AddScoped<IServicoService, ServicoService>();
builder.Services.AddScoped<IAgendamentoService, AgendamentoService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.InicializarAsync(services);
}

if (app.Environment.IsDevelopment())
    app.UseMigrationsEndPoint();
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

app.Run();