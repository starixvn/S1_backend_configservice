using ConfigService;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/Config", () =>
{
    var configurationRoot = new ConfigurationBuilder()
             .SetBasePath(AppContext.BaseDirectory)
             .AddJsonFile($"appsettings.json", false, true)
             .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName ?? "Production"}.json", false, true)
             .Build();
    var appConfig = new AppConfig();
    configurationRoot.Bind(appConfig);
    return JsonConvert.SerializeObject(appConfig);
});

app.Run();



