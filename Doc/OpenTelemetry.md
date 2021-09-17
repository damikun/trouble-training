## OpenTelemetry

<img src="./Assets/opentelemetry-logo.png" alt="OpenTelemtry logo" width="150"/>

Is a collection of tools, APIs and SDKs. Used to collect telemetry data from distributed systems to troubleshoot, debug, and understand software performance and behavior.

**So what that means?**

Many modern applications are microservice based. These are essentially an interconnected network of services, so understanding system performance from multiple sources is a major challenge. A single call in an application can trigger dozens of events. 

How can developers and engineers isolate a problem when something goes wrong or a request runs slowly? Teams have wrestled with the best way to generate, collect, and analyze telemetry data.

That's why *Open Telemetry* exists as a new standard, and as of 2021, *OpenTelemetry* has reached a major milestone, the [OpenTelemetry Tracing Specification](https://github.com/\open-telemetry/opentelemetry-specification/blob/main/specification/overview.md) version 1.0.

> &#10240;
>**Opentelemetry** is a standardised way to understand the entire chain of events and the complex interaction between microservices.
> &#10240;

**Flow**

The application instrumented by the client sends trace data via one of the various available transports to the collector, which processes the trace data and stores it in memory.

<p align="center">
  <img alt="Opentelemetry flow" src="./Assets/opentelemetry_flow.PNG">
</p>

**OpenTelemetry components**

- `APIs and SDKs` per programming language for generating and emitting traces  (SDK for `Java`,`.Net`,`C++`,`Golang`,`Python`,`Javascript`,`PHP`,`Ruby` etc...)

    ![OpenTelemtry Languages](./Assets/opentelemetry_languages.PNG "OpenTelemtry Languages")

- `Collectors` - provides a vendor-independent implementation for receiving, processing and exporting telemetry data.

- `OTLP protocol`  specification describes the encoding, transport, and delivery mechanism of telemetry data. [You can read more](https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/protocol/otlp.md).
## Distributed trace

A distributed trace is a series of events raised as a result of a single logical operation (*Request = CreateUser*) and consolidated across different components of an application.

**Tracing signal**

`Traces` in OpenTelemetry are implicitly defined by their `spans` (building blocks). These can be projected as contiguous asynchronous diagrams, usually based on the time axis. (*right image*). 

- A `Span` can be connected to zero or more other `Spans`.
- Links can point to `spans` within a single `trace` or across different `traces`.

<p align="center">
  <img alt="Span flow" src="././Assets/trace_flow.png">
</p>

**Spans**

A span is the building block of a `trace` and represents part of the workflow in the distributed system. (*WriteToDB, SendNotification, HandleCommand*)

A span represents an operation within a transaction and encapsulates:
 - An operation name
 - A timestamp for start and end
 - A list of key-value pairs. (Custom Attributes)
 - The span identifier of the parent.
 - SpanContext information required to reference a span.

- `Span name` concisely identifies the work represented by the Span. Example: `get_account/{accountId}`

**SpanContext**

Represents all the information that identifies `Span` and encapsulates:
 - `TraceId` is the identifier for a trace (unique). TraceId is used to group all spans for a specific trace.
 - `SpanId ` is the identifier for a span (unique)
 - `TraceState` - carries tracing-system specific context in a list of key value pairs.

**Span Attributes**

Attributes are key-value pairs that provide detail about a span.

 - `Status` - (standardized propert), It may be set to values like OK, Cancelled, and Permission Denied
 - `SpanKind` - (standardized propert) 
 - `User-Defined` - you can also create your own attribute key/value

**SpanKind**

SpanKind describes the relationship between the Span, its parents, and its children in a Trace.

`SERVER` Indicates that the span covers server-side handling remote request.
`CLIENT` Indicates that the span describes a request to some remote service. This span does not end until the response is received.
`PRODUCER` Indicates that the span describes the initiators of an asynchronous request. In messaging scenarios with batching, tracing individual messages requires a new PRODUCER span per message to be created.
`CONSUMER` Indicates that the span describes a child of an asynchronous PRODUCER request.
`INTERNAL` Default value. Indicates that the span represents an internal operation within an application, as opposed to an operations with remote parents or children.

**LifeCicle**

In general, the life cycle of a span can be interpreted as follows:
1) A request is received from a service. The span context is extracted from the request headers, if it exists.
2) A new span is created as a child of the extracted span context; if none exists, a new root span is created.
3) The service processes the request. Additional attributes and events are added to the scope. 
4) New areas can be created to represent the work of subcomponents of the service.
5) When the service makes a remote call to another service, the current span context is serialized and forwarded to the next service.
6) The work performed by the service is completed, successful or not

### Logs vs Telemtry

`Logs` - Is a timestamped text/structured record.
`Traces` - Track the progression of a single request, called a trace, as it is handled by services that make up an application.

*"Hey isnt logging enaught ?"*

Now, logging is important and helps you diagnose specific errors, but it does not give you the full experience of queries. What you want is basically to use logs with telemetry data to get the full experience.

Telemetry data tries to answer questions like:
 - How are users really engaging with the app?
 - How much time do users spend in each app session and between sessions?
 - What is the slowest part of a request?
 - Etc.

**So what is common process to determin problem?**
First, you need to look in Tracing Graph and identify the problem. Based on the request flow, we can see the slowest part or where the problem occurs in time. Which operations were successful and which failed. We collect `SpanIds` and `TraceId` we are interested in and look for contextual logs to get more details about what really happened. For this reason, we use traces and logs to determine the overall application experience.

In order for `OpenTelemetry` to be successful in the logging space, the company is trying to support existing logs and logging libraries so that they work well together.

Separate-collection             |  Unified-collection
:-------------------------:|:-------------------------:
![separate-collection](./Assets/separate-collection.png "separate-collection")  |  ![unified-collection](./Assets/unified-collection.png "unified-collection")


### NetCore Setup

[Opentelemetry dotnet](https://github.com/open-telemetry/opentelemetry-dotnet) provides `.NetCore` API and SDK that can simplify collecting and exporting traces to external collectors.

**Nuget**

1) Main package SDK
- `dotnet add package OpenTelemetry --version 1.1.0`

2) Main package API
- `dotnet add package OpenTelemetry.Api --version 1.1.0`

3) Extensions Hosting - Support to use in DI
- `dotnet add package OpenTelemetry.Extensions.Hosting --version 1.0.0-rc7`

4) Instrumentation `AspNetCore`
- `dotnet add package OpenTelemetry.Instrumentation.AspNetCore --version 1.0.0-rc7`

5) Instrumentation `Http`
- `dotnet add package OpenTelemetry.Instrumentation.Http --version 1.0.0-rc7`

6) Instrumentation `SQL Client`
- `dotnet add package OpenTelemetry.Instrumentation.SqlClient --version 1.0.0-rc7`

7) Instrumentation `EFCore`
- `dotnet add package OpenTelemetry.Contrib.Instrumentation.EntityFrameworkCore --version 1.0.0-beta2`

**What is instrumentation**

The term *instrumentation* is used by several languages to encapsulate tracing, debugging, etc.

- By definition, it refers to the ability to monitor or measure the performance of an application and diagnose errors.

With `NetCore` you have several ways to measure and collect traces.

1) Opentelemetry SDK 
2) NetCore Framework nativ support
3) 3-Part Libs and SDK (Elastic APM SDK...)

**The nativ support is the prefered way** since the NetCore team inject `opentelemetry` support to [`System.Diagnostics APIs`](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics?view=net-5.0) class implementaion. This helps us to reuse existing instrumentation.

`Opentelemetry` / `NetCore` equialents are:
- For Opentelemetry `Tracer` equialent is .Net [`ActivitySource`](https://docs.microsoft.com/en-us/dotnet/core/diagnostics/distributed-tracing-collection-walkthroughs) class.
- For Opentelemety `Span` equialent is .Net [`Activity`](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.activity?view=net-5.0) class.

You can read more about nativ `Opntelemerty` support under officail [Microsoft Release Note](https://devblogs.microsoft.com/dotnet/opentelemetry-net-reaches-v1-0/)

All other SDK options give you +- the same functionality. The naming of the library API may be different, of course, but that's a matter of implementation.

>Most SDKs are currently in a migration process to fully support the Opentelemetry standard and may be deprecated over time, as native standard support is all you need.

If your application and infrastructure supports `Opentelemetry`, you can also easily integrate your exports with cloud monitoring as all major cloud providers (Azure, Google, AWS) support `Opentelemetry` as a standard telemetry input.

### Setup

In `Program.cs` function `services.AddTelemerty(...)`is cWalled to configure OpenTelemetry functionality. The method is part of global partial class `ServiceExtension` and help us to spread the configuration to separate files. 

```c#
 public static partial class ServiceExtension {

        public static IServiceCollection AddTelemerty(
            this IServiceCollection serviceCollection,
            IConfiguration Configuration, IWebHostEnvironment Environment) {

            serviceCollection.AddOpenTelemetryTracing((builder) => {
                // Sources
                builder.AddSource(Sources.DemoSource.Name);

                builder.SetResourceBuilder(ResourceBuilder
                  .CreateDefault()
                    //.AddAttributes( new List<KeyValuePair<String, object>> { 
                    //   new KeyValuePair<String, object>("SomeKey", "This is String Value")
                    //  })
                  .AddService(Environment.ApplicationName));

                builder.AddAspNetCoreInstrumentation(opts => {
                    opts.RecordException = true;
                });

                builder.AddElasticsearchClientInstrumentation();

                builder.AddSqlClientInstrumentation();

                builder.AddHttpClientInstrumentation(opts => opts.RecordException = true);

                if (Uri.TryCreate(Configuration.GetConnectionString("Jaeger"), UriKind.Absolute, out var uri)) {
                    builder.AddJaegerExporter(opts => {
                        opts.AgentHost = uri.Host;
                        opts.AgentPort = uri.Port;
                        opts.BatchExportProcessorOptions = new OpenTelemetry.BatchExportProcessorOptions<Activity>() {
                        };
                    });
                
                    // builder.AddZipkinExporter(opts => {
                    //     opts.Endpoint = new Uri("http://localhost:9412/api/v2/spans");
                    // });
                }
            });

            return serviceCollection;
        }
    }
```

`Jaeger` connection string is loaded from `AppSettings.json` `ConnectionStrings` section.

```Json
  "ConnectionStrings": {
    "HangfireConnection": "...",
    "AppDBConnection": "...",
    "Jaeger": "udp://localhost:6831",
    "Elasticsearch": "..."
  },
```

- `builder.AddSource(Sources.DemoSource.Name)` Register one (Global) `ActivitySource` used throughout the application. It requires to import the `System.Diagnostics` namespace to use it.
    > You can create also multiple sub *Sources* as `demo.subname`. Sources are defined in `Domain` project and can be inported to any project in domain (Aplication,Persistence or API). Each microservices out of monolit shoud define own Source.

    ```c#
    // Src/Backend/Domain/Sources/TraceSource.cs
    public static class SourcesExtensions {

        // Implemented based on https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Api/README.md#instrumenting-a-libraryapplication-with-net-activity-api 
        private static readonly AssemblyName AssemblyName
        = typeof(SourcesExtensions).Assembly.GetName();
        internal static readonly ActivitySource ActivitySource
            = new(AssemblyName.Name, AssemblyName.Version.ToString());

        public static readonly ActivitySource DemoSource = new("demo");
        // new("trouble");
        public static readonly ActivitySource SchedulerSource = new("demo.scheduler");
    }
    ```

- `.AddService(Environment.ApplicationName))` - defines aplication name in Trace context. This is important to differ what service trigger the events and also is visible in tracing time graph. You can define any `string` or use `Assembly` name.
    
    ```
    builder.SetResourceBuilder(ResourceBuilder
        .CreateDefault()
        .AddService(Environment.ApplicationName)); // WebAPI
    ```

    ![Trace Service Name](./Assets/trace_context_service_name.png "Trace Service Name")

- `.AddAttributes` - Using the builder you can adittionaly add any other global attributes to traces. This attributes are not scoped but defined for one instance.

```
builder.SetResourceBuilder(ResourceBuilder
    .CreateDefault()
    .AddAttributes( new List<KeyValuePair<String, object>> { 
        new KeyValuePair<String, object>("SomeKey", "This is string Value")
        })
    .AddService(Environment.ApplicationName));
```
![Trace Attributes - Jaeger UI](./Assets/trace_info.png "Trace Attributes - Jaeger UI")

## Jaeger

Jaeger is distributed tracing system released as open source by [Uber Technologies](https://uber.github.io/). It provides full tracing experience include client side SDK. Since 2020 `Jeager` has builded in support for `Opentelemetry`.

**Components**
`Jaeger Client SDK` - language-specific implementations for app instrumenting. (not used in demo)
`Jaeger Agent` - is a network daemon that listens for spans sent over User Datagram Protocol
`Jaeger Collector`- receives spans and places them in a queue for processing.
`Jaeger Query` - is a service that retrieves traces from storage.
`Jaeger UI` - provides a user interface that lets you visualize your distributed tracing data.

If you wanna use Jaeger without `Opentelemetry SDK` You can [explore its SDK](https://github.com/jaegertracing/jaeger-client-csharp). It does similar stuff as `Opentelemetry SDK` or `Elastic APM Agent` SDK..


> **Demo** uses Jeager as **collector** and normalizer for streamed traces and **Jaeger UI** to visualize collected data. Client trace data are exported over Openetelemety SDK!

**Technical info:**
- Backend components implemented in Go
- Frontend React/Javascript UI
- Multiple scallable storage backends:
    - Cassandra
    - Elasticsearch
    - Kafka
    - memory storage

### Docker Install

In folder `./Src/Docker/Jaeger` is `docker-compose.yml` containing jaeger all-in-one image. By running `docker-compose up` you can setup dev. Image.

**Deployment strategy**

Isn`t it a bit strange if we want to track a large distributed service, but only send the metrics to a "monolithic" Docker service (all-in-one solution)?

> **In production enviroment you wanna choose scalable jaeger deployment!**

For deployin `Jaeger` in production please [follow official documentation](https://www.jaegertracing.io/docs/1.24/deployment/).

Example `yaml`:

```yaml
version: "3"

services:
  jaeger-collector:
    image: jaegertracing/jaeger-collector
    ports:
      - "14269:14269"
      - "14268:14268"
      - "14267:14267"
      - "9411:9411"
    networks:
      - elastic-jaeger
    restart: on-failure
    environment:
      - SPAN_STORAGE_TYPE=elasticsearch
    command: [
      "--es.server-urls=http://localhost:9200",
      "--es.num-shards=1",
      "--es.num-replicas=0",
      "--log-level=error"
    ]

  jaeger-agent:
    image: jaegertracing/jaeger-agent
    hostname: jaeger-agent
    command: ["--collector.host-port=jaeger-collector:14267"]
    ports:
      - "5775:5775/udp"
      - "6831:6831/udp"
      - "6832:6832/udp"
      - "5778:5778"
    networks:
      - elastic-jaeger
    restart: on-failure
    environment:
      - SPAN_STORAGE_TYPE=elasticsearch
    depends_on:
      - jaeger-collector

  jaeger-query:
    image: jaegertracing/jaeger-query
    environment:
      - SPAN_STORAGE_TYPE=elasticsearch
      - no_proxy=localhost
    ports:
      - "16686:16686"
      - "16687:16687"
    networks:
      - elastic-jaeger
    restart: on-failure
    command: [
      "--es.server-urls=http://localhost:9200",
      "--span-storage.type=elasticsearch",
      "--log-level=debug"
    ]
    depends_on:
      - jaeger-agent

volumes:
  esdata:
    driver: local

networks:
  elastic-jaeger:
    driver: bridge 
```
