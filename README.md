<h3 align="center">Full Stack Workshop (Frontend + Backend + Monitoring) </h3>

<p align="center">
  <img  width="900" src="Doc/Assets/stack_logos.png" alt="Workshop UI" >
</p>
<p align="center" > Aplication contains full Frontend and Backend implementation with infrastructure and app monitoring. There are multiple used patterns around like  <b>Mediator, BFF, Domain</b> etc.. and all is secured using <b>Identity server</b> fully supporting <b>OpenId Connect</b> and <b>OAuth2.0</b></p>
<br />



<p align="center" >Demo Application contains small UI where you can manage your WebHooks</p>


<p align="center">
  <img width="600" src="Doc/Assets/WorkshopUI.gif" alt="Workshop UI" >
</p>

### Stack
 - **Frontend** - [React](https://reactjs.org/), [Relay](https://relay.dev/), [Typecsript](https://www.typescriptlang.org/), [ReactRouter](https://reactrouter.com/), [TailwindCSS](https://tailwindcss.com/)
 - **Backend** - [Netcore](https://dotnet.microsoft.com/), [Hotchocolate](https://chillicream.com), [IdentityServer](https://duendesoftware.com/products/identityserver), [ElasticSearch](https://www.elastic.co/), [Opentelemerty](https://opentelemetry.io/), [Serilog](https://serilog.net/), [MediatR](https://github.com/jbogard/MediatR), [Hangfire](https://www.hangfire.io/), [Automapper](https://automapper.org/), [Fluentvalidation](https://fluentvalidation.net/), [Docker](https://www.docker.com/)

### Integration steps:
Please follow this steps if you wanna understand it step-by-step (because of connection between parts):

1) [Configure logging](Doc/Logging.md) - Teach you how to setup proper distributed logging solution for .Net
2) [Configure telemetry](Doc/OpenTelemetry.md) - Teach you how to setup proper Opentelemetry and Elastic APM
3) [Configure idenity](Doc/Identity.md) - Teach you how to use BFF pattern to secure your app.
4) [Configure monitoring](Doc/ElasticSearch.md) - Teach you how to monitor enitre app stack (This depens on parts 1, 2)


<br />
<p align="center">

<img src="Doc/Assets/workshop_architecture.png" alt="Workshop architecture" >

<br />

</p>


**Monitoring example**

<p align="center">

<img src="Doc/Assets/elastic_apm_example.png" alt="Apm distributed tracing" >

</p>