using ConfigService;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
var _serviceConfig = new Dictionary<string, ServiceConfig>(builder.Configuration.GetSection("ServiceConfig").Get<Dictionary<string, ServiceConfig>>(), StringComparer.OrdinalIgnoreCase);
var app = builder.Build();
var appConfig = new AppConfig();
builder.Configuration.Bind(appConfig);

app.MapGet("/", () =>
{
    return JsonConvert.SerializeObject(appConfig);
});

app.MapGet("/{serviceName}", (string serviceName) =>
{
    if (_serviceConfig.TryGetValue(serviceName, out var config))
    {
        appConfig.ChangeUserAndPassWord(config);
    }
    return JsonConvert.SerializeObject(appConfig);
});

app.MapGet("/health", async context =>
{
    await context.Response.WriteAsync($"{builder.Environment.EnvironmentName}-{DateTime.Now:yyyy/MM/dd HH:mm:ss}");
});

app.Run();



