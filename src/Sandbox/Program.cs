using Sandbox.Models;
using Sandbox.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<IProjectFilesService, ProjectFilesService>(httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration["Clients:FileService:Url"] ?? String.Empty);
});

builder.Services.AddHttpClient<IProjectManagement, ProjectManagement>(httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration["Clients:ProjectManagement:Url"] ?? String.Empty);
});

builder.Services.AddSingleton<IDockerFileGeneratorFactory, DockerFileGeneratorFactory>();
builder.Services.AddSingleton<IContainerService, ContainerService>();
builder.Services.AddScoped<IStorageService, StorageService>();

var app = builder.Build();

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