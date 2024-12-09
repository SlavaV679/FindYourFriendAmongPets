using PetFriend.Accounts.Infrastructure.Seeding;
using PetFriend.Accounts.Presentation;
using PetFriend.Volunteers.Application;
using PetFriend.Volunteers.Infrastructure;
using Serilog;
using Serilog.Events;
using Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.Seq(builder.Configuration.GetConnectionString("Seq")
                 ?? throw new ArgumentNullException("Seq"))
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .CreateLogger();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAccountsModule(builder.Configuration);

// builder.Services.AddFluentValidationAutoValidation(configuration =>
// {
//     configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>();
// });

builder.Services.AddSerilog();

builder.Services
    .AddDataAccess()
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    //.AddAuthorization()
    ;

var app = builder.Build();

var accountsSeeder = app.Services.GetRequiredService<AccountsSeeder>();

await accountsSeeder.SeedAsync();

app.UseExceptionMiddleware();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.ApplyMigration();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();