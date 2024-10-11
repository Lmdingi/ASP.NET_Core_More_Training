using Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ServiceContracts;
using Services;
using CRUDExample.Middleware;

var builder = WebApplication.CreateBuilder(args);

//logging
// builder.Host.ConfigureLogging(loggingProvider => {
//     loggingProvider.ClearProviders();
//     loggingProvider.AddConsole();
//     loggingProvider.AddDebug();
//     loggingProvider.AddEventLog();
// });
//Serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider service,
LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(service);
});

builder.Services.AddControllersWithViews();

//add services into IoC container
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonsService, PersonsService>();

builder.Services.AddDbContext<PersonsDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//ConnectionString = "Server=localhost\\SQLEXPRESS;Database=PersonsDatabase;Trusted_Connection=True;TrustServerCertificate=True";

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else 
{
    app.UseExceptionHandler();
}

app.UseSerilogRequestLogging();

//app.UseHttpLogging();

// app.Logger.LogDebug("Debug-message");
// app.Logger.LogInformation("Information-message");
// app.Logger.LogWarning("Warning-message");
// app.Logger.LogError("Error-message");
// app.Logger.LogCritical("Critical-message");

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
