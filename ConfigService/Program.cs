using ConfigService;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGet("/", () =>
{
    var appConfig = new AppConfig();
    builder.Configuration.Bind(appConfig);
    return JsonConvert.SerializeObject(appConfig);
});

app.MapGet("/health", async context =>
{
    await context.Response.WriteAsync($"{builder.Environment.EnvironmentName}-{DateTime.Now:yyyy/MM/dd HH:mm:ss}");
});

app.Run();



