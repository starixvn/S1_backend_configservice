using ConfigService;
 
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var appConfig = new AppConfig();
builder.Configuration.Bind(appConfig);

var dbGroup = Environment.GetEnvironmentVariable("DBGroup") ?? string.Empty;
var env = builder.Environment.EnvironmentName;

// 沒有dbGroup名使用 ex：appsettings.Development.json
// 有dbGroup名稱使用 ex：appsettings.Development.Leobet.json
app.MapGet("/", () => string.IsNullOrEmpty(dbGroup)
    ? appConfig
    : GetCustomizeAppConfig(env, dbGroup));


// 依照服務指定Config
// 沒有dbGroup名使用 ex：appsettings.Development.json
// 有dbGroup名稱使用 ex：appsettings.Development.Leobet.json
var _serviceConfig = new Dictionary<string, ServiceConfig>(builder.Configuration.GetSection("ServiceConfig").Get<Dictionary<string, ServiceConfig>>(), StringComparer.OrdinalIgnoreCase);
app.MapGet("/{serviceName}", (string serviceName) =>
{
    if (_serviceConfig.TryGetValue(serviceName, out var config))
    {
        var serviceConfig = string.IsNullOrEmpty(dbGroup)
            ? new AppConfig()
            : GetCustomizeAppConfig(env, dbGroup);

        if (string.IsNullOrEmpty(dbGroup)) //預設值才要需要再 Bind一次，GetCustomizeAppConfig 已經 Bind 過一次，Bind第二次會被蓋回 default 設定
        {
            builder.Configuration.Bind(serviceConfig);
        }

        serviceConfig.ChangeUserAndPassWord(config);
        return serviceConfig;
    }

    return string.IsNullOrEmpty(dbGroup)
        ? appConfig
        : GetCustomizeAppConfig(env, dbGroup);
});


app.MapGet("/local/{group?}/{serviceName?}", (string group, string serviceName) =>
{
    group ??= "nova";
    var filePath = Path.Combine(AppContext.BaseDirectory, "LocalAppConfig", $"{group.ToLower()}.json");
    var configurationBuilder = new ConfigurationBuilder().AddJsonFile(filePath, optional: true, reloadOnChange: true);

    var configuration = configurationBuilder.Build();
    var localConfig = new AppConfig();
    configuration.Bind(localConfig);
    return localConfig;
});

app.MapGet("/health", async context =>
{
    await context.Response.WriteAsync($"{env}-{DateTime.Now:yyyy/MM/dd HH:mm:ss}");
});

// 取得指定dbGroup設定檔
AppConfig GetCustomizeAppConfig(string env, string dbGroup)
{
    var filePath = Path.Combine(AppContext.BaseDirectory, $"appsettings.{env}.{dbGroup}.json");
    var configurationBuilder = new ConfigurationBuilder().AddJsonFile(filePath, optional: true, reloadOnChange: true);

    var configuration = configurationBuilder.Build();
    var customizeAppConfig = new AppConfig();
    configuration.Bind(customizeAppConfig);
    return customizeAppConfig;
}

app.Run();



