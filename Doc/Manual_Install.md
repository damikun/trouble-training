
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
Port: 5555
Username: postgres
PasswordL postgres
```

*IdentityServer Database*
```sh
Host: localhost
DatabaseName: IdentityDB
Port: 5555
Username: postgres
PasswordL postgres
```

*Scheduler Database*
```sh
Host: localhost
DatabaseName: Scheduler
Port: 5555
Username: postgres
PasswordL postgres
```

Preconfigured connection strings:

```json
"ConnectionStrings": {
  "HangfireConnection": "Host=localhost;Port=5555;Database=Scheduler;Username=postgres;Password=postgres",
  "ApiDbContext": "Host=localhost;Port=5555;Database=APIServer;Username=postgres;Password=postgres",
  "AppIdnetityDbContext": "Host=localhost;Port=5555;Database=IdentityDB;Username=postgres;Password=postgres",
  "AppIdnetityDbContext": "Host=localhost;Port=5555;Database=IdentityDB;Username=postgres;Password=postgres",
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