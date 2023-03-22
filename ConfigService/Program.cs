using ConfigService;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var appConfig = new AppConfig();
builder.Configuration.Bind(appConfig);

app.MapGet("/", () =>
{
    return appConfig;
});

//依照服務指定Config
var _serviceConfig = new Dictionary<string, ServiceConfig>(builder.Configuration.GetSection("ServiceConfig").Get<Dictionary<string, ServiceConfig>>(), StringComparer.OrdinalIgnoreCase);
app.MapGet("/{serviceName}", (string serviceName) =>
{
    if (_serviceConfig.TryGetValue(serviceName, out var config))
    {
        appConfig.ChangeUserAndPassWord(config);
    }
    return appConfig;
});

app.MapGet("/health", async context =>
{
    await context.Response.WriteAsync($"{builder.Environment.EnvironmentName}-{DateTime.Now:yyyy/MM/dd HH:mm:ss}");
});

app.Run();



