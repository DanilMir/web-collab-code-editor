using Auth;
using Auth.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using static OpenIddict.Abstractions.OpenIddictConstants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth API", Version = "v1" });
        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        // c.AddSecurityDefinition(
        //     "oauth",
        //     new OpenApiSecurityScheme
        //     {
        //         Flows = new OpenApiOAuthFlows
        //         {
        //             ClientCredentials = new OpenApiOAuthFlow
        //             {
        //                 Scopes = new Dictionary<string, string>
        //                 {
        //                     ["api"] = "api scope description"
        //                 },
                        
        //             },
        //         },
        //         In = ParameterLocation.Header,
        //         Name = HeaderNames.Authorization,
        //         Type = SecuritySchemeType.OAuth2
        //     }
        // );
        c.AddSecurityRequirement(
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                            { Type = ReferenceType.SecurityScheme, Id = "oauth" },
                    },
                    new[] { "api" }
                }
            }
        );
    }
);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // Configure Entity Framework Core to use Microsoft SQL Server.
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        optionsBuilder => { optionsBuilder.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null); }
    );

    // Register the entity sets needed by OpenIddict.
    // Note: use the generic overload if you need to replace the default OpenIddict entities.
    options.UseOpenIddict();
});

builder.Services
    .AddDefaultIdentity<AuthUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();


builder.Services.AddOpenIddict()

    // Register the OpenIddict core components.
    .AddCore(options =>
    {
        // Configure OpenIddict to use the Entity Framework Core stores and models.
        // Note: call ReplaceDefaultEntities() to replace the default entities.
        options.UseEntityFrameworkCore()
            .UseDbContext<ApplicationDbContext>();
    })

    // Register the OpenIddict server components.
    .AddServer(options =>
    {
        // Enable the authorization, logout, token and userinfo endpoints.
        options
            .SetAuthorizationEndpointUris("connect/authorize")
            .SetDeviceEndpointUris("connect/device")
            .SetVerificationEndpointUris("connect/verify")
            .SetLogoutEndpointUris("connect/logout")
            .SetTokenEndpointUris("connect/token")
            .SetUserinfoEndpointUris("connect/userinfo");

        // Mark the "email", "profile" and "roles" scopes as supported scopes.
        options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles);

        // Enable necessary flows
        options.AllowClientCredentialsFlow();
        options.AllowDeviceCodeFlow();
        options.AllowAuthorizationCodeFlow();
        options.AllowRefreshTokenFlow();

        // Register the signing and encryption credentials.
        options.AddDevelopmentEncryptionCertificate()
            .AddDevelopmentSigningCertificate();

        // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
        options.UseAspNetCore()
            .EnableAuthorizationEndpointPassthrough()
            .EnableLogoutEndpointPassthrough()
            .EnableTokenEndpointPassthrough()
            .EnableUserinfoEndpointPassthrough()
            .EnableVerificationEndpointPassthrough()
            .EnableStatusCodePagesIntegration();
    })

    // Register the OpenIddict validation components.
    .AddValidation(options =>
    {
        // Import the configuration from the local OpenIddict server instance.
        options.UseLocalServer();

        // Register the ASP.NET Core host.
        options.UseAspNetCore();
    });

// Register the worker responsible of seeding the database with the sample clients.
// Note: in a real world application, this step should be part of a setup script.
builder.Services.AddHostedService<Worker>();


// CORS policy to allow SwaggerUI and React clients
builder.Services.AddCors(
    options =>
    {
        options.AddPolicy(
            "default", policy =>
            {
                policy
                    .WithOrigins(
                        "http://localhost:3000",
                        "https://localhost:3000"
                        )
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });

var app = builder.Build();

// app.UseDeveloperExceptionPage();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();
app.UseCors("default");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapDefaultControllerRoute();
app.MapRazorPages();

app.Run();