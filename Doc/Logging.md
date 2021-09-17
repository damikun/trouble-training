## Logging


App uses `Serilog` library to hadne structured logging. It provides various exporters (sinks) to forward all logs to remote distributed logging storage.

Important to understand:
 - Do not invest your time in writing logging library from scratch
 - Use structured logging - They displays log events in a simple, easy-to-ready JSON encoded format
 - Log exceptions and contextual data for effective production debugging
 - Add request context data (UserId, SpanId etc..) for better information filtering and searching and determinig the source of problem

**Nuget**

1) Main package
- `dotnet add package Serilog.AspNetCore --version 4.1.1-dev-00229`

2) Extensions for loging
- `dotnet add package Serilog.Extensions.Logging --version 3.0.2-dev-10289`

3) Be able to log Exception details and auto serialize it
- `dotnet add package Serilog.Exceptions --version 7.0.0`

4) Extensions to include enviroment and machine info
- `dotnet add package Serilog.Enrichers.Environment --version 2.2.0-dev-00784`

5) Extension to include `span` information with logs important for opentelemetry connection with traces
- `dotnet add package Serilog.Enrichers.Span --version 1.2.0`

6) Sink for exporting log streams to elasticsearch storage
- `dotnet add package Serilog.Sinks.Elasticsearch --version 8.5.0-alpha0003`

7) Suppor to use Serilog in Hosted services
- `dotnet add package Serilog.Extensions.Hosting --version 4.1.2`

8) Console extension - Used to colorise Console output
- `dotnet add package Serilog.Sinks.Console --version 4.0.0`

### Setup

In `Program.cs` the `CreateDefaultBuilder` function initialize the default `.NetCore` logging. It use `ILogger` or `ILogger<T>` and writes output to `Console`.

```c#
// Program.cs
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder => {
            webBuilder.UseStartup<Startup>();
    });
```

This demo uses [`Serilog`](https://serilog.net/) which overwrite default configuration and is used on top of `.NetCore` one.

In `Program.cs` function `ConfigureLogging(...)`is called to define the Serilog configuration.

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

        var hostEnvironment = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

        logCfg = logCfg
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithMachineName()
            .Enrich.WithSpan()
            .Enrich.With(services.GetService<UserIdEnricher>());
            .Enrich.WithProperty("Environment", hostEnvironment.EnvironmentName)
            .ReadFrom.Configuration(GetLogConfigurationFromJson());
            
        try {

            if (hostEnvironment.IsDevelopment()) {
                logCfg = logCfg.WriteTo.Console(theme:AnsiConsoleTheme.Code);
            } else {
                logCfg = logCfg.WriteTo.Elasticsearch(ConfigureElasticSink(GetLogConfigurationFromJson(), environment));
            }

        } catch (Exception ex) { }
    }

    Log.Logger = logCfg.CreateLogger();
}

public static IConfigurationRoot  GetLogConfigurationFromJson(){
    return new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile(
            $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
            optional: true)
        .Build();
}

```
Details:
- `.Enrich.FromLogContext()` - used to dynamically add and remove properties from the ambient "execution context". For example, all messages written during a transaction might carry the id of that transaction
- `.Enrich.WithExceptionDetails()` - adds additional structured properties from exceptions
- `.Enrich.WithMachineName()` - adds machine name to logs
- `.Enrich.WithSpan()` - include current tracing spanId to logs
- `.Enrich.WithProperty("Environment", environment)` - Include current enviromnet info. (prod / dev / etc...)
- `.ReadFrom.Configuration(configuration)` - Source of Logging configuration (see next section)
- `.Enrich.With(services.GetService<UserIdEnricher>());` - Is custom enricher to put UserId to all log events if exist. See end section to understand more.

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
In `dev.` enviroment logging to `console` is in most of cases enaught. It is not recomended in `prod.` since it can slow down the server (writting to `system.io`) and is not efficient. Another thing is that some production deployments cannot write to `system.io`.

You can extend Configuration and add custom writer for example to write logs to file:
    `.WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)`

Serilog provides various sinks for writing log events to storage. You can find all available sinks in this [link](https://github.com/serilog/serilog/wiki/Provided-Sinks).

In this Demo the `Serilog.Sinks.Elasticsearch` and `Serilog.Sinks.MSSqlServer` are used for demonstration.

### Log Level Configuration

App by default takes log level configuration from `appsettings.json` or `appsettings.{enviroment}.json` if exist. That is `.NetCore` default Logging setup. We need to overide default configuration by `Serilog one as on example:

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
        "Microsoft.EntityFrameworkCore": "Information"
      }
    }
  }
```

> **Info**: Json configuration file support reload in runtime.

You can anytime extend `json` configuration section. More information are available under oficial docs.

Extended example (not in demo)

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

- `Verbose` -	is the noisiest level, rarely (if ever) enabled for a production app.
- `Debug` - is used for internal system events that are not necessarily observable from the outside, but useful when determining how something happened.
- `Information` - info. events describe things happening in the system that correspond to its responsibilities and functions. Generally these are the observable actions the system can perform.
- `Warning` -	When service is degraded, endangered, or may be behaving outside of its expected parameters. Something bad happened, but the application still has the chance to heal itself or the issue can wait a day or two to be fixed.
- `Error` -	When functionality is unavailable or expectations broken, an Error event is used. Users are being affected without having a way to work around the issue.
- `Fatal` -	The most critical level, Fatal events demand immediate attention.

> As example between `Warning` and `Error` can be when system failed to connect to an external resource but will try again automatically. It might ultimately result in an ERROR log message when the retry-mechanism also fails.

 `Warning`,  `Error`, `Fatal` are levels that should be active in production systems by default.

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

Custom Enrichers allows us to add, remove or modify the properties attached to event log. In demo we need to add `CurrentUserId` to logs to be able filter logs by `userId`.

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
        var userId = _httpContextAccessor.HttpContext.User.Identity.Name;
        
        if(int.TryParse(userId,out int parsed_user_id)){
            var userNameProperty = factory.CreateProperty("UserId",  parsed_user_id);
            logEvent.AddPropertyIfAbsent(userNameProperty);
        }
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

Default serilog console output can be BW and it depends on operating system console (teromina) default color setup. In case you wanna colorize it follow this setps:

1) The `Serilog.Sinks.Console` package must be installed
2) Right teheme must be set and also force to set this settings on redirected output.

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

- It is possible to move part or entire logging configuration to `appSettings.json`. You need to understand that equal settings gets override one bt each other depending on call order.

> **Info**: Be carefull of duplicit definition as `WriteTo.Console` in code and in `appSettings` the logs will be written to console twice. Keep only one way to define output!

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