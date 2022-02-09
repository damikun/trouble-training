## Logging


App uses **Serilog** library to hadne structured logging. It provides various exporters (sinks) to forward all logs to remote distributed logging storage.

Important to understand:
 - Do not invest your time in writing logging library from scratch
 - Use structured logging - They displays log events in a simple, easy-to-ready JSON encoded format
 - Log exceptions and contextual data for effective production debugging
 - Add request context data (UserId, SpanId etc..) for better information filtering and searching and determinig the source of problem

**Nuget**

```xml
    <!-- Main package -->
    <PackageReference Include="Serilog.AspNetCore" Version="4.1.1-dev-00241" />
    <!-- Extensions for loging -->
    <PackageReference Include="Serilog.Extensions.Logging" Version="3.0.2-dev-10289" />
    <!-- Extensions to include enviroment and machine info -->
    <PackageReference Include="Serilog.Enrichers.Environment" Version="2.2.0-dev-00784" />
    <!-- Extension to include `span` information with logs important for opentelemetry connection with traces -->
    <PackageReference Include="Serilog.Enrichers.Span" Version="1.2.0" />
    <!-- Be able to log Exception details and auto serialize it -->
    <PackageReference Include="Serilog.Exceptions" Version="7.0.0" />
    <!-- Suppor to use Serilog in Hosted services -->
    <PackageReference Include="Serilog.Extensions.Hosting" Version="4.1.2" />
    <!-- Be able to update configuration -->
    <PackageReference Include="Serilog.Settings.Configuration" Version="3.2.0-dev-00272" />
    <!-- Console extension - Used to colorise Console output -->
    <PackageReference Include="Serilog.Sinks.Console" Version="4.0.0" />
    <!-- Sink for exporting log streams to elasticsearch storage -->
    <PackageReference Include="Serilog.Sinks.Elasticsearch" Version="8.5.0-alpha0003" />
```

### Setup


In `Program.cs`, the function `CreateDefaultBuilder` initializes the default `.NetCore` logging. It uses `ILogger` or `ILogger<T>` and writes the output to `Console`.

```c#
// Program.cs
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder => {
            webBuilder.UseStartup<Startup>();
    });
```

This demo uses ['Serilog'](https://serilog.net/), which overrides the default configuration and is used in addition to the `.NetCore` configuration.

This require to extend the default builder to include `UseSerilog()`

```c#
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
    .UseSerilog(); // <--- This
```

In `Program.cs` the function `ConfigureLogging(...)` is called to define the Serilog configuration.

```c#
using System;
using Serilog;
using Serilog.Exceptions;
using Serilog.Enrichers.Span;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog.Sinks.SystemConsole.Themes;
using Microsoft.Extensions.DependencyInjection;

// Backend/API/ServiceExtensions/LoggingConfiguration.cs
 public static void ConfigureLogging(IHost host){
    // Create log configuration builder
    var logCfg = new LoggerConfiguration();

    using (var scope = host.Services.CreateScope()) {

      var services = scope.ServiceProvider;

      var hostEnvironment = scope
                              .ServiceProvider
                              .GetRequiredService<IWebHostEnvironment>();

      var configuration = GetLogConfigurationFromJson();

      logCfg = logCfg
          .Enrich.FromLogContext()
          .Enrich.WithExceptionDetails()
          .Enrich.WithMachineName()
          .Enrich.WithSpan()
          .Enrich.WithElasticApmCorrelationInfo()
          .Enrich.WithProperty(
              "Environment",
                hostEnvironment.EnvironmentName)
          .ReadFrom.Configuration(configuration);

      try
      {

          if (hostEnvironment.IsDevelopment())
          {
              logCfg = logCfg.WriteTo.Console(
                  theme: AnsiConsoleTheme.Code,
                  outputTemplate: "[{ElasticApmTraceId} {ElasticApmTransactionId} {Message:lj} {NewLine}{Exception}",
                  applyThemeToRedirectedOutput: true);
          }

          logCfg = logCfg.WriteTo.Elasticsearch(
              ConfigureElasticSink(configuration, hostEnvironment.EnvironmentName));
      }
      catch { }
    }

    Log.Logger = logCfg.CreateLogger();
}

public static IConfigurationRoot  GetLogConfigurationFromJson(){
    return new ConfigurationBuilder()
      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
      .AddJsonFile(
          path: $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
          optional: true)
      .Build();
}

```
Details:
- `.Enrich.FromLogContext()` - used to dynamically add and remove properties from the surrounding "execution context". For example, all messages written during a transaction could carry the ID of that transaction
- `.Enrich.WithExceptionDetails()` - adds additional structured properties of exceptions
- `.Enrich.WithMachineName()` - adds the machine name to the logs
- `.Enrich.WithSpan()` - adds the current tracing SpanId to the logs
- `.Enrich.WithProperty("Environment", environment)` - adds current environment information. (prod / dev / etc...)
- `.ReadFrom.Configuration(configuration)` - Source of logging configuration (see next section).
- `.Enrich.With(services.GetService());` - Is a custom extension that adds a UserId to all logging events, if any. Read the last section to learn more.

The last section conditionaly setup logging based on current enviroment. 

```
if (hostEnvironment.IsDevelopment()) {
    logCfg = logCfg.WriteTo.Console(
        theme: AnsiConsoleTheme.Code,
        applyThemeToRedirectedOutput: true);
} else {
    logCfg = logCfg.WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment));
}
```
In the `dev.` environment, logging to the `console` is often needed. In the production environment, this is not recommended as it can slow down the server (writing to `system.io`) and is not efficient. Another point is that some production environments cannot write to `system.io`.

You can extend the configuration and add a custom writer to write logs to a file, for example: `.WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)`

Serilog provides several sinks for writing log events to memory. You can find all available sinks in this [link](https://github.com/serilog/serilog/wiki/Provided-Sinks).

In this Demo the `Serilog.Sinks.Elasticsearch` and `Serilog.Sinks.MSSqlServer` are used for demonstration.

### Log Level Configuration

By default, the app takes the log level configuration from `appsettings.json` or `appsettings.{enviroment}.json` if available. This is the `.NetCore` default configuration for logging. We need to override the default configuration with `Serilog`, as in this example:

```Json
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Microsoft.Hosting.Lifetime": "Information",
      "Microsoft": "Information",
      "Microsoft.EntityFrameworkCore": "Information",
      "Override": {
        "Microsoft": "Information",
        "System": "Information",
        "Microsoft.Hosting.Lifetime": "Information",
        "Microsoft.EntityFrameworkCore": "Information",
        "Elastic.Apm": "Debug",
        "Microsoft.AspNetCore.Authentication": "Information"
      }
    }
  },
```
> &#10240;
> **Info**: Json configuration file support reload in runtime.
> &#10240;

You can always expand the configuration section `json`. For more information, see the official documents.

Extended example (not in the demo)

```json
{
  "Serilog": {
    "Using":  [ "Serilog.Sinks.Console", "Serilog.Sinks.File" ],
    "MinimumLevel": "Debug",
    "WriteTo": [
      { "Name": "Console" },
      { "Name": "File", "Args": { "path": "Logs/log.txt" } }
    ],
    "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId" ],
    "Destructure": [
      { "Name": "With", "Args": { "policy": "Sample.CustomPolicy, Sample" } },
      { "Name": "ToMaximumDepth", "Args": { "maximumDestructuringDepth": 4 } },
      { "Name": "ToMaximumStringLength", "Args": { "maximumStringLength": 100 } },
      { "Name": "ToMaximumCollectionCount", "Args": { "maximumCollectionCount": 10 } }
    ],
    "Properties": {
        "Application": "Sample"
    },
    "Filter": [{
        "Name": "ByIncludingOnly",
        "Args": {
            "expression": "Application = 'Sample'"
        }
    }]
  }
}

```

**Levels:**

Logging levels allow us to put a log message into one of several buckets, sorted by urgency.

- `Verbose` - is the noisiest level, rarely (if ever) activated for a production application.
- `Debug` - used for internal system events that are not necessarily observable from the outside, but are useful for figuring out how something happened.
- `Information` - info events describe things that happen in the system and match its responsibilities and functions. In general, these are the observable actions that the system can perform.
- `Warning` - When a service is impaired, compromised, or possibly behaving outside of expected parameters. Something bad has happened, but the application still has a chance to heal itself or the problem may wait a day or two to be fixed.
- `Error` - When features are unavailable or expectations are not met, an error event is used. Users are affected with no way to work around the problem.
- `Fatal` - The most critical level: fatal events require immediate attention.

 An example between `Warning` and `Error` would be when the system failed to connect to an external resource, but automatically retries. If the retry also fails, this can result in a ERROR log message.

 `Warning`, `Error` and `Severe` are levels that should be active by default in production systems.

### Log example

1) Using injected `ILogger` or `ILogger<T>`

```c#
public class SomeClass {
    private readonly ILogger<SomeClass> _logger;

    public SomeClass(ILogger<SomeClass> logger) {  // Injection
        _logger = logger;
    }

    public void SomeHandler(){
        _logger.LogInformation("Some message")  // Ussage
    }
}

```

2) Serilog interface
```c#
var exampleUser = new User { Id = 1, Name = "Adam", Created = DateTime.Now };

// Need to include namespace (using Serilog)
Log.Information("Created {@User} on {Created}", exampleUser, DateTime.Now);

```

Console Output:

```
[13:57:12 INF] Created {"Id": 1, "Name": "Adam", "Created": "2020-09-01T13:56:59.7803740-05:00", "$type": "User"} on 1/09/2020 1:57:12 PM
```

### Formating
You can read more about Formating under this article. [Customized JSON formatting with Serilog](https://nblumhardt.com/2021/06/customize-serilog-json-output/)


### Custom Enricher

Custom Enrichers allows us to add, remove or modify the properties associated with the event log. In the demo, we need to add 'CurrentUserId' to the logs to be able to filter the logs by `userId`.

```c#
// Backend/API/ServiceExtensions/UserIdEnricher.cs
class UserIdEnricher : ILogEventEnricher{
    readonly IHttpContextAccessor _httpContextAccessor;

    public UserIdEnricher(IHttpContextAccessor httpContextAccessor){
        _httpContextAccessor = httpContextAccessor;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory factory){
        if (!(_httpContextAccessor.HttpContext?.User.Identity.IsAuthenticated ?? false))
            return;

        // Expect that Name == UserId as string
        var userId = _contextAccessor?.HttpContext?.User?.GetId<Guid>();
        
        var userNameProperty = factory.CreateProperty("UserId",  userId);
        logEvent.AddPropertyIfAbsent(userNameProperty);
    }
}
```

In `Startup.cs`
```c#
public void ConfigureServices(IServiceCollection services){
    services.AddTransient<UserIdEnricher>();
    services.AddHttpContextAccessor();
    // etc...
}
```

In Serilog configuration
```
.Enrich.With(services.GetService<UserIdEnricher>());
```

**Manualy add enricher property**

`.Enrich.WithProperty("SomeProperty", "SomeValue")`

`.Enrich.WithProperty("ServerId", 007)`

### Filters

As example, If we want to log just specific `UserId` (Specific Enrich property value) we can do this:

`.Filter.ByExcluding(Matching.WithProperty<int>("UserId", p => p == 10))`

### Colorised Console output

The default output of the Serilog console can be BW and depends on the default color setting of the operating system console (teromina). If you want to color it, follow these instructions:

1) The package `Serilog.Sinks.Console` must be installed. 2) The correct theme must be set and you must also enforce these settings for the redirected output.

```
logCfg = logCfg.WriteTo.Console(
    theme: AnsiConsoleTheme.Code,
    applyThemeToRedirectedOutput: true);
```

Or from `AppSettings`

```json
  "WriteTo": [
    { "Name": "Console",
      "Args": {
        "theme": "Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme::Code, Serilog.Sinks.Console",
        "applyThemeToRedirectedOutput": true
      }}
  ],
```

**Themes:**
- `ConsoleTheme.None` - no styling
- `SystemConsoleTheme.Literate` - styled to replicate Serilog.Sinks.Literate, using the System.Console coloring modes supported on all Windows/.NET targets; this is the default when no theme is specified
- `SystemConsoleTheme.Grayscale` - a theme using only shades of gray, white, and black
- `AnsiConsoleTheme.Literate` - an ANSI 16-color version of the "literate" theme; we expect to update this to use 256-colors for a more refined look in future
- `AnsiConsoleTheme.Grayscale` - an ANSI 256-color version of the "grayscale" theme
- `AnsiConsoleTheme.Code` - an ANSI 256-color Visual Studio Code-inspired theme

### Move configuration to `appSettings.json`

- It is possible to move some or all of the logging configuration to the `appSettings.json` file. You must be aware that the same settings will overwrite each other depending on the calling order.

> &#10240;
> **Info**: Be carefull of duplicit definition as `WriteTo.Console` in code and in `appSettings` the logs will be written to console twice. Keep only one way to define output!
> &#10240;

```c#
// ConfigureLogging(...)
.ReadFrom.Configuration(
    GetLogConfigurationFromJson());
//etc...
```

`appSettings.json`
```
{
  "Serilog": {
    "Using":  [ "Serilog.Sinks.Console","Serilog.Enrichers.Span","Serilog.Exceptions"],
    "WriteTo": [
      { "Name": "Console",
        "Args": {
          "theme": "Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme::Code, Serilog.Sinks.Console",
          "applyThemeToRedirectedOutput": true
        }}
    ],
    "Enrich": [ "FromLogContext", "WithExceptionDetails", "WithMachineName","WithSpan" ],
    "MinimumLevel": {
      "Default": "Information",
      "Microsoft.Hosting.Lifetime": "Information",
      "Microsoft": "Information",
      "Microsoft.EntityFrameworkCore": "Information",
      "Override": {
        "Microsoft": "Information",
        "System": "Information",
        "Microsoft.Hosting.Lifetime": "Information",
        "Microsoft.EntityFrameworkCore": "Information"
      }
    }
  }
}

```

