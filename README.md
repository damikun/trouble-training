
<h2 align="center">Full Stack Workshop (Frontend + Backend + Monitoring) </h2>

<p align="center">
  <img  width="900" src="Doc/Assets/stack_logos.png" alt="Workshop stack tools logos" >
</p>
<p align="center" > The application contains a full frontend and backend implementation with infrastructure and app monitoring. There are several patterns used such as. <b>Mediator, BFF, Domain</b> etc.. and everything is secured with <b>Identity server</b> fully supporting <b>OpenId Connect</b> and <b>OAuth2.0</b></p>


<h5 align="center" >
  
  <a href="https://github.com/damikun/trouble-training/blob/main/Doc/WebHookBackend.md" >WebHook backend setup</a></br>
  <a href="https://github.com/damikun/trouble-training/blob/main/Doc/Logging.md" >Configure Logging</a></br>
  <a href="https://github.com/damikun/trouble-training/blob/main/Doc/OpenTelemetry.md">Configure Tracing</a></br>
  <a href="https://github.com/damikun/trouble-training/blob/main/Doc/Identity.md">Configure Identity</a></br>
  <a href="https://github.com/damikun/trouble-training/blob/main/Doc/ElasticSearch.md">Configure Monitoring</a></br>
  <a href="https://github.com/damikun/trouble-training/blob/main/Doc/GraphQL%20-%20MutationErrors.md">Handle GraphQL Errors</a></br>
  <a href="https://github.com/damikun/trouble-training/blob/main/Doc/BuildSystem.md">Build Automation</a></br>
</br>

![Backend](https://github.com/damikun/trouble-training/actions/workflows/backend-restore-build-and-test.yml/badge.svg)
![Frontend](https://github.com/damikun/trouble-training/actions/workflows/frontend-restore-and-build.yml/badge.svg)
</h5>

<p align="center" >Demo Application contains small UI, where you can manage your WebHooks</p>

<p align="center">
  <img width="600" src="Doc/Assets/WorkshopUI.gif" alt="Workshop UI example" >
</p>

## Stack

 - **Frontend**  - [React](https://reactjs.org/), [Relay](https://relay.dev/), [Typecsript](https://www.typescriptlang.org/), [ReactRouter](https://reactrouter.com/), [TailwindCSS](https://tailwindcss.com/)
 - **Backend** - [Netcore](https://dotnet.microsoft.com/), [Hotchocolate](https://chillicream.com), [IdentityServer](https://duendesoftware.com/products/identityserver), [ElasticSearch](https://www.elastic.co/), [Opentelemerty](https://opentelemetry.io/), [Serilog](https://serilog.net/), [MediatR](https://github.com/jbogard/MediatR), [Hangfire](https://www.hangfire.io/), [Automapper](https://automapper.org/), [Fluentvalidation](https://fluentvalidation.net/), [Docker](https://www.docker.com/), [Entity Framework](https://docs.microsoft.com/cs-cz/ef/)

## Project structure

- [APIServer](https://github.com/damikun/trouble-training/tree/main/Src/APIServer) - Is protected GraphQL API
- [BFF](https://github.com/damikun/trouble-training/tree/main/Src/BFF) - Is Backend for Frontend pattern
  - [Frontend React](https://github.com/damikun/trouble-training/tree/main/Src/BFF/API/ClientApp) - This also contains Frontend React app served using static files
- [IdentityServer](https://github.com/damikun/trouble-training/tree/main/Src/IdentityServer) - Idetity server service for providing `OpenId Connect` and `OAuth2.0`
- [Device](https://github.com/damikun/trouble-training/blob/main/Doc/Identity.md#authentication-machine-to-machine-using-client-credentials-flow) - Machine-to-machine comunucation using clinetcredentials for external apps/devices
- [Tests](https://github.com/damikun/trouble-training/blob/main/Src/Tests) - Unit, Integration and API tests

## Integration steps:

Please follow these steps if you want to understand it because of the connection between the parts:

1) [WebHook backend](Doc/WebHookBackend.md) - I`ll show you how to create basic WebHook integration under .NetCore Backend for this demo.
2) [Configure logging](Doc/Logging.md) - I`ll show you how to set up a proper distributed logging solution for .Net
3) [Configure telemetry](Doc/OpenTelemetry.md) - I`ll show you how to properly set up Opentelemetry and Elastic APM.
4) [Configure identity](Doc/Identity.md) - You`ll learn how to use the BFF pattern to secure your application.
5) [Configure monitoring](Doc/ElasticSearch.md) - You will learn how to monitor the entire app stack (this depends on parts 1 and 2).
6) [Handle GraphQL Mutation errors](https://github.com/damikun/trouble-training/blob/main/Doc/GraphQL%20-%20MutationErrors.md) - You will learn how to integrate *6a* pattern.
7) [Build automation and workflows](Doc/BuildSystem.md) - You will learn how to setup build plan and generate workflows.

<br />
<p align="center">

<img src="Doc/Assets/workshop_architecture.png" alt="Workshop architecture" >

<br />

</p>


**Frontend to Backend distributed tracing example**

On this monitoring graphical visualization, you can see entire request sended from Client Frontend (Creating a webhook)

<p align="center">

<img src="Doc/Assets/CreateWebHook_Output.png" alt="Elastic Frontend to backend distributed tracing" >

</p>

<br />

## Setup projects and env.

<p align="center">
<h3>You can use <a href="https://www.youtube.com/watch?v=3LBqZiW_Hvo">step-by-step instalation video</a> which use manual instalation process following this steps:</h3>
</p>

  - [Download docker](#download-docker)
  - [Compose docker images](#compose-docker-images)
  - [Restoring all projects](#restoring-all-projects)
  - [Setup Database](#setup-database)
  - [Migrations](#migrations)
  - [Run Frontned and Backend](#run-frontned-and-backend)
  - [Run Elastic and Beats](#run-elastic-and-beats)

To run this stack locally, you need to ensure thfollowing:
- Make sure you have NET SDK installed. 
- Make sure you have NodeJs installed.
- Make sure you have one of package managers `npm` or `yarn`
- You need to install Docker. Most of the stack runs in a Docker container.
- You need to run all prepared `docker-compose` files to populate all images for the monitoring platform.
- You need to set up PostgreSQL databases (Create any DB server). 
- You need to migrate all DBs (each project has its own migration folder).
- Run the NetCore project from the terminal or back it up to Docker.

</br>

### Download docker

Based on your system download and [install docker](https://docs.docker.com/engine/install/)

</br>

### Compose docker images

Set this configurtaion settings (for localhost) from your `cmd`:

```sh
#For Linux
sysctl -w vm.max_map_count=262144 
echo "vm.max_map_count=262144" >> /etc/sysctl.conf
```

```sh
#For Windows WSL
wsl -d docker-desktop
sysctl -w vm.max_map_count=262144
```

**Continue with images:**

In the `/Docker` folder you will find prepared images for:
- **Elasticsearch** - Elastic, Kibana, OtelCollector, APMserver, Logstash.
  - Shell `cd` to `Docker/ElasticSearch` and run `docker-compose up`
- **PostgresSQL** - Database
  - Shell `cd` to `Docker/PostgresSql` and run `docker-compose up`
- **Beats** - (Optional)  FileBeat, HeartBeat, MetricBeat, PacketBeat (⚠️ Skip this in case you dont have enaught RAM)
  - Shell `cd` to `Docker/Beats` and run `docker-compose up`

In each folder you will find `docker-compose.yml` and you need to run the command `docker-compose up` in your terminal. The setup has been tested on WSL Docker Desktop.

> **Note:** Folder `/Docker` also contains other images for aditional lessons. (For this demo ignor it)

</br>

### Restoring all projects

**Option A:** In case you wanna use Nuke build-scripts to restore Frontend and Backend use the following command  `.\build.cmd All` (this will take some time to processs)

> **NOTE** This scripts use `npm`. In case you wanna use `yarn` you need to install maunaly using Option-B since Nuke does not support yarn yet..

<p align="center">

<img src="Doc/Assets/BuildPipe.PNG" alt="Build automation pipaline" >

</p>


</br>


**Option B:** In case you wanna do it step-by-step or use yarn or scripts does not work for you follow this steps:

Manualy install .net-sdk:

[Make sure you have](https://dotnet.microsoft.com/download) Net 5.0 SDK installed. You can cheque the SDK version by running: `dotnet --list-sdks` in your terminal.

```sh
#Example output on Windows
PS C:\Users\dakupc> dotnet --list-sdks
5.0.100 [C:\Program Files\dotnet\sdk]
5.0.201 [C:\Program Files\dotnet\sdk]
```
</br>

Manualy restore:

Go to dirrectory: `APIServer/API` and run `dotnet restore`

Go to dirrectory: `IdentityServer/API` and run `dotnet restore`

Go to dirrectory: `BFF/API` and run:
1) `dotnet restore`
2) `cd` to `BFF/API/ClientApp` and run `yarn install` or `npm install` (Needed to install `node_modules`)

Go to dirrectory: `Device/API` and run:
1) `dotnet restore`
2) `cd` to `Device/API/ClientApp` and run `yarn install` or `npm install` (Needed to install `node_modules`)

> &#10240;
>**NOTE:** Restoring will take some time, especially for BFF, since the frontend is fully recompiled and served by the BFF backend using static files.
> &#10240;

</br>

### Setup Database

To manage your existing database you can use [PgAdmin](https://www.pgadmin.org/download/). This tool helps you to create and manage databases under postgresql server.

Make sure that PostgreSQL is running. The following empty databases need to be manualy created:

*APIServer Database*
```sh
Host: localhost
DatabaseName: APIServer
Port: 5432
Username: postgres
PasswordL postgres
```

*IdentityServer Database*
```sh
Host: localhost
DatabaseName: IdentityDB
Port: 5432
Username: postgres
PasswordL postgres
```

*Scheduler Database*
```sh
Host: localhost
DatabaseName: Scheduler
Port: 5432
Username: postgres
PasswordL postgres
```

Preconfigured connection strings:

```json
"ConnectionStrings": {
  "HangfireConnection": "Host=localhost;Port=5432;Database=Scheduler;Username=postgres;Password=postgres",
  "ApiDbContext": "Host=localhost;Port=5432;Database=APIServer;Username=postgres;Password=postgres",
  "AppIdnetityDbContext": "Host=localhost;Port=5432;Database=IdentityDB;Username=postgres;Password=postgres",
  "AppIdnetityDbContext": "Host=localhost;Port=5432;Database=IdentityDB;Username=postgres;Password=postgres",
  "Elasticsearch": "http://admin:admin@localhost:9200",
  "Opentelemetry": "http://localhost:55680"
},
```

</br>

### Migrations

Make sure you have installed [Entity Framework Core tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet) to perform any necessary migrations.

```sh
dotnet tool install --global dotnet-ef
```

If you run  `dotnet ef` in terminal you shoud get:

```
                     _/\__       
               ---==/    \\      
         ___  ___   |.    \|\    
        | __|| __|  |  )   \\\   
        | _| | _|   \_/ |  //|\\ 
        |___||_|       /   \\\/\\

Entity Framework Core .NET Command-line Tools 5.0.9
```

> **NOTE:** Make sure PostgreSQL is running for next steps!

Go to dirrectory: `APIServer/Persistence` and run following commands:

```
 dotnet ef database update
```

Go to dirrectory: `Src/IdentityServer/Persistence` and run following commands:
```
 dotnet ef database update --context AppConfigurationDbContext
 dotnet ef database update --context AppPersistedGrantDbContext
 dotnet ef database update --context AppIdnetityDbContext
```
</br>

### Run Frontned and Backend

Since all migrations, databases and infrastructures are prepared, you can start each project from its directory. You can also put each application in a docker container and launch it from there. This is entirely up to you.

> &#10240;
> **NOTE:** Make sure you trust developer certificates: `dotnet dev-certs https --trust`. This is only for development needs make sure you properly setup certificate in production!
> &#10240;

Install Tailwind CLI from cmd: `npx tailwindcss-cli@latest init`

Go to dirrectory: `APIServer/API` and run `dotnet watch run`

Go to dirrectory: `IdentityServer/API` and run `dotnet watch run`

Go to dirrectory: `BFF/API` and run `dotnet watch run`

(only if you are gonna test Device)
Go to dirrectory: `Device/API` and run `dotnet watch run`

</br>


**APIServer** runs on: `https://localhost:5022`

**IdentityServer** runs on: `https://localhost:5001`

**BFF** runs on: `https://localhost:5015`

</br>

### Run Elastic and Beats

Start the elasticsearch container group and then beats. Please stick to this order to avoid exception logging due to a missing connection. You can also put everything into a Docker image and wait until the previous section is complete.

> **Note:** Running beats for demo is optional. This require additional RAM and CPU allocation..

In case you have problem to start elastic due to memory issues adjust `max_map_count`


```sh
#For Linux
sysctl -w vm.max_map_count=262144 
echo "vm.max_map_count=262144" >> /etc/sysctl.conf
```

```sh
#For Windows WSL
wsl -d docker-desktop
sysctl -w vm.max_map_count=262144
```

To run all containers, please make sure you have enough RAM and a good computer - this is not for kids :P Have fun :)