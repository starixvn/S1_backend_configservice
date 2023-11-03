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
        var serviceConfig = new AppConfig();
        builder.Configuration.Bind(serviceConfig);
        serviceConfig.ChangeUserAndPassWord(config);
        return serviceConfig;
    }
    else
    {
        return appConfig;
    }  
});


app.MapGet("/local/{group}/{serviceName?}", (string group, string serviceName) =>
{
    var basePath = Path.Combine(Directory.GetCurrentDirectory(), "LocalAppConfig"); 
    var builder = new ConfigurationBuilder().SetBasePath(basePath).AddJsonFile($"{group.ToLower()}.json", optional: true, reloadOnChange: true);
   
    var configuration = builder.Build();    
    var localConfig = new AppConfig();
    configuration.Bind(localConfig);
    try
    {
        var _serviceConfig = new Dictionary<string, ServiceConfig>(configuration.GetSection("ServiceConfig").Get<Dictionary<string, ServiceConfig>>(), StringComparer.OrdinalIgnoreCase);
        if (serviceName is not null && _serviceConfig.TryGetValue(serviceName, out var config))
        {
            localConfig.ChangeUserAndPassWord(config);
        }

        return localConfig;       
    }
    catch (Exception)
    {

        return localConfig;
    }
});

app.MapGet("/health", async context =>
{
    await context.Response.WriteAsync($"{builder.Environment.EnvironmentName}-{DateTime.Now:yyyy/MM/dd HH:mm:ss}");
});

app.Run();



