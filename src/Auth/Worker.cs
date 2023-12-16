using Auth.Data;
using OpenIddict.Abstractions;

namespace Auth;
using static OpenIddict.Abstractions.OpenIddictConstants;

public class Worker : IHostedService
{

    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public Worker(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        var clientId = _configuration["Clients:ReactClient:ClientId"] ?? string.Empty;
        var uri = _configuration["Clients:ReactClient:Host"] ?? string.Empty;
        
        if (await manager.FindByClientIdAsync(clientId, cancellationToken) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = clientId,
                DisplayName = "React client",
                RedirectUris =
                {
                    new Uri($"{uri}/signin-oidc")
                },
                PostLogoutRedirectUris =
                {
                    new Uri($"{uri}/signout-callback-oidc")
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
                    Permissions.Prefixes.Scope + "project_management",
                    Permissions.Prefixes.Scope + "files"
                }
            });
        }
        
        clientId = _configuration["Clients:Postman:ClientId"] ?? string.Empty;
        uri = _configuration["Clients:Postman:Host"] ?? string.Empty;
        
        if (await manager.FindByClientIdAsync(clientId, cancellationToken) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = clientId,
                DisplayName = "postman",
                RedirectUris =
                {
                    new Uri($"{uri}/v1/callback")
                },
                Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.GrantTypes.RefreshToken,
                    Permissions.ResponseTypes.Code,
                    Scopes.OpenId,
                    Scopes.OfflineAccess,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles,
                    Permissions.Prefixes.Scope + "project_management",
                    Permissions.Prefixes.Scope + "files",
                }
            });
        }
        
        clientId = _configuration["Clients:ProjectManagement:ClientId"] ?? string.Empty;
        var clientSecret = _configuration["Clients:ProjectManagement:ClientSecret"] ?? string.Empty;
        
        if (await manager.FindByClientIdAsync(clientId) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                Permissions =
                {
                    Permissions.Endpoints.Introspection
                },
            });
        }
        
        clientId = _configuration["Clients:Files:ClientId"] ?? string.Empty;
        clientSecret = _configuration["Clients:Files:ClientSecret"] ?? string.Empty;
        
        if (await manager.FindByClientIdAsync(clientId) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                Permissions =
                {
                    Permissions.Endpoints.Introspection
                },
            });
        }
        
        var scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();
        
        if (await scopeManager.FindByNameAsync("project_management") is null)
        {
            await scopeManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = "project_management",
                Resources =
                {
                    clientId
                }
            });
        }
        
        if (await scopeManager.FindByNameAsync("files") is null)
        {
            await scopeManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = "files",
                Resources =
                {
                    clientId
                }
            });
        }
    }

    
    
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}