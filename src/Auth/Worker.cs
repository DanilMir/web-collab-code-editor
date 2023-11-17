using Auth.Data;
using OpenIddict.Abstractions;

namespace Auth;
using static OpenIddict.Abstractions.OpenIddictConstants;

public class Worker : IHostedService
{

    private readonly IServiceProvider _serviceProvider;
    
    public Worker(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        if (await manager.FindByClientIdAsync("react-client", cancellationToken) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "react-client",
                DisplayName = "React client",
                RedirectUris =
                {
                    //TODO: url to env
                    new Uri($"https://localhost:3000/signin-oidc")
                },
                PostLogoutRedirectUris =
                {
                    //TODO: url to env
                    new Uri($"https://localhost:3000/signout-callback-oidc")
                },
                Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Token,
                    Permissions.Endpoints.Logout,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.GrantTypes.RefreshToken,
                    Permissions.ResponseTypes.Code,
                    Scopes.OpenId,
                    Scopes.OfflineAccess,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles,
                }
            });
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}