using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using SyncService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<FilesService>(client =>
        client.BaseAddress = new Uri(builder.Configuration["Clients:Files:Url"] ?? String.Empty)
    );

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = int.MaxValue; 
    options.Limits.MaxRequestLineSize = int.MaxValue; 
    options.Limits.MaxRequestBufferSize = long.MaxValue; 
    options.Limits.MaxRequestHeadersTotalSize = int.MaxValue; 
    options.Limits.MaxResponseBufferSize = long.MaxValue; 
});

builder.Services.AddHttpLogging(o => { });

var app = builder.Build();

app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapPost("sync/callback", ([FromBody]string temp) =>
{
    Console.WriteLine(temp);
});

// app.MapControllers();

app.Run();