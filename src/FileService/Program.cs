using Amazon.S3;
using FileService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<AwsS3Service>();
builder.Services.AddSingleton<BucketsService>();

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