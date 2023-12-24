using Microsoft.IdentityModel.Tokens;
using OpenIddict.Validation.AspNetCore;
using FileService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<AwsS3Service>();
builder.Services.AddSingleton<BucketsService>();
builder.Services.AddHttpClient<ProjectsService>(httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration["Clients:ProjectManagement:Url"] ?? String.Empty);
});


builder.Services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
builder.Services.AddAuthorization();

builder.Services.AddOpenIddict()
    .AddValidation(options =>
    {
        // Note: the validation handler uses OpenID Connect discovery
        // to retrieve the address of the introspection endpoint.
        options.SetIssuer("http://auth");
        options.AddAudiences(builder.Configuration["Clients:Files:ClientId"] ?? string.Empty);

        // Configure the validation handler to use introspection and register the client
        // credentials used when communicating with the remote introspection endpoint.
        options.AddEncryptionKey(new SymmetricSecurityKey(
            Convert.FromBase64String(builder.Configuration["SymmetricSecurityKey"] ?? string.Empty)));


        // Register the System.Net.Http integration.
        options.UseSystemNetHttp();

        // Register the ASP.NET Core host.
        options.UseAspNetCore();

        options.Configure(opts =>
        {
            opts.TokenValidationParameters.ValidIssuers = new List<string>
            {
                "http://auth:80/",
                "http://localhost:5001/",
            };
        });
    });


var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var buckets = scope.ServiceProvider.GetRequiredService<BucketsService>();
    await buckets.EnsureBucketIsCreated("projects");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();