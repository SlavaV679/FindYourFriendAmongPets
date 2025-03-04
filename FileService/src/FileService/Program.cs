using Amazon.S3;
using FileService.Endpoints;
using FileService.Extensions;
using FileService.MongoDataAccess;
using Hangfire;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEndpoints();

builder.Services.AddCors();

builder.Services.AddSingleton<IMongoClient>(
    new MongoClient(builder.Configuration.GetConnectionString("MongoDb")));

builder.Services.AddScoped<FileMongoDbContext>();

builder.Services.AddScoped<IFileRepository, FileRepository>();

builder.Services.AddHangfirePostgres(builder.Configuration);

builder.Services.AddSingleton<IAmazonS3>(_ =>
{
    var config = new AmazonS3Config
    {
        ServiceURL = "http://localhost:9000",
        ForcePathStyle = true,
        UseHttp = true
    };

    return new AmazonS3Client("minioadmin", "minioadmin", config);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHangfireServer();
app.UseHangfireDashboard();

app.MapEndpoints();

app.Run();
