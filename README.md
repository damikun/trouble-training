<h3 align="center">Full Stack Workshop (Frontend + Backend + Monitoring) </h3>

<br />

<p align="center">
  <img width="600" src="Doc/Assets/WorkshopUI.gif" alt="Workshop UI" >
</p>

### Stack
 - **Frontend** - [React](https://reactjs.org/), [Relay](https://relay.dev/), [Typecsript](https://www.typescriptlang.org/), [ReactRouter](https://reactrouter.com/), [TailwindCSS](https://tailwindcss.com/)
 - **Backend** - [Netcore](https://dotnet.microsoft.com/), [Hotchocolate](https://chillicream.com/docs/hotchocolate/v10), [IdentityServer](https://duendesoftware.com/products/identityserver), [ElasticSearch](https://www.elastic.co/), [Opentelemerty](https://opentelemetry.io/), [Serilog](https://serilog.net/), [MediatR](https://github.com/jbogard/MediatR), [Hangfire](https://www.hangfire.io/), [Automapper](https://automapper.org/), [Fluentvalidation](https://fluentvalidation.net/), [Docker](https://www.docker.com/)

### Integration steps:
Please follow this steps because of connection between each part:

1) [Configure logging](Doc/Logging.md) - Teach you how to setup proper distributed logging solution for .Net
2) [Configure telemetry](Doc/OpenTelemetry.md) - Teach you how to setup proper Opentelemetry and Elastic APM
3) [Configure idenity](Doc/Identity.md) - Teach you how to use BFF pattern to secure your app.
4) [Configure monitoring](Doc/ElasticSearch.md) - Teach you how to monitor enitre app stack (This depens on parts 1, 2)


<br />
<p align="center">

<img src="Doc/Assets/workshop_architecture.png" alt="Workshop architecture" >

<br />

</p>


**Content**

<p align="center">

<img src="Doc/Assets/elastic_apm_example.png" alt="Apm distributed tracing" >

</p>