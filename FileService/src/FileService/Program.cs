using Amazon.S3;
using FileService.Endpoints;
using FileService.Extensions;
using FileService.Middlewares;
using Hangfire;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddLogger(builder.Configuration);
builder.Services.AddHttpLogging(options => { options.CombineLogs = true; });
builder.Services.AddSerilog();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEndpoints();

builder.Services.AddCors();

builder.Services.AddRepositories();

builder.Services.AddMongoDb(builder.Configuration);

builder.Services.AddHangfirePostgres(builder.Configuration);

builder.Services.AddMinio(builder.Configuration);

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

app.UseSerilogRequestLogging();
app.UseExceptionHandling();

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