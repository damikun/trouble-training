using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Docker;
using Nuke.Common.Tools.EntityFramework;

partial class Build : NukeBuild
{

    //---------------
    // Enviroment
    //---------------

    [PathExecutable("docker-compose")] readonly Tool DockerCompose;

    AbsolutePath DatabaseDocker => RootDirectory / "Docker" / "PostgresSql";
    AbsolutePath DB_Volume_Dir => RootDirectory / "Docker" / "PostgresSql" / "tdev";
    AbsolutePath APIServer_DB_Cfg => RootDirectory / "Docker" / "PostgresSql" / "APIServerDB.sh";
    AbsolutePath Identity_DB_Cfg => RootDirectory / "Docker" / "PostgresSql" / "IdentityDB.sh";
    AbsolutePath Scheduler_DB_Cfg => RootDirectory / "Docker" / "PostgresSql" / "SchedulerDB.sh";

    string postgres_db_name = "trouble_db";

    //---------------
    // Build process
    //---------------

    Target Docker_Info => _ => _
        .OnlyWhenStatic(() => EnvironmentInfo.IsLinux || EnvironmentInfo.IsWin)
        .Executes(() =>
        {
            DockerTasks.DockerVersion();
        }
    );

    Target Restore_Tools => _ => _
    .After(Clean)
    .Executes(() =>
    {
        try
        {
            DotNetTasks.DotNetToolRestore();
        }
        catch { }

    });

    Target Postgresql_Init => _ => _
        .OnlyWhenStatic(() => EnvironmentInfo.IsLinux || EnvironmentInfo.IsWin)
        .Executes(() =>
        {
            TryStopAndRemove(postgres_db_name);

            /* ####### YAML #######

            version: "3.7"
            services:
            database:
                image: postgres
                container_name: trouble_db
                restart: always
                ports:
                - "5555:5555"
                environment:
                - POSTGRES_DB=Trouble
                - POSTGRES_PASSWORD=postgres
                - POSTGRES_USER=postgres
                - MASTER_PASSWORD_REQUIRED=False
                volumes:
                - ./dev/tdev-db:/var/lib/postgresql/tdev-db
                - ./APIServerDB.sh:/docker-entrypoint-initdb.d/APIServerDB.sh
                - ./IdentityDB.sh:/docker-entrypoint-initdb.d/IdentityDB.sh
                - ./SchedulerDB.sh:/docker-entrypoint-initdb.d/SchedulerDB.sh
                command: -p 5555

            */

            // Docker Task representation of YAML
            DockerTasks.DockerRun(e => e
                .SetImage("postgres")
                .SetName(postgres_db_name)
                .SetRestart("always")
                .SetPublish("5555:5555")
                .SetEnv(new string[] {
                    "POSTGRES_PASSWORD=postgres",
                    "POSTGRES_USER=postgres",
                    "MASTER_PASSWORD_REQUIRED=False"
                })
                .AddVolume($"{DB_Volume_Dir}:/var/lib/postgresql/tdev-db")
                .AddVolume($"{APIServer_DB_Cfg}:/docker-entrypoint-initdb.d/APIServerDB.sh")
                .AddVolume($"{Identity_DB_Cfg}:/docker-entrypoint-initdb.d/IdentityDB.sh")
                .AddVolume($"{Scheduler_DB_Cfg}:/docker-entrypoint-initdb.d/SchedulerDB.sh")
                .SetCommand("-p 5555")
                .SetDetach(true)
                .SetProcessWorkingDirectory(DatabaseDocker)

            );

        });

    Target Postgresql_Stop => _ => _
    .Executes(() =>
    {
        try
        {
            DockerTasks.Docker($"stop {postgres_db_name}");
        }
        catch { }
    });


    Target API_Init => _ => _
        .DependsOn(
            // Postgresql_Init, // only in case postgres is required
            API_Compile,
            Frontend_Restore,
            API_Migrate_DB
        );

    Target Identity_Init => _ => _
        .DependsOn(
            // Postgresql_Init,  // only in case postgres is required
            Identity_Compile,
            Identity_Migrate_DB
    );

    Target Init => _ => _
        .DependsOn(
        All,
        API_Init,
        Identity_Init
    );

    private static void TryStopAndRemove(string container_name)
    {
        try
        {
            DockerTasks.Docker($"stop {container_name}");
        }
        catch { }

        try
        {
            DockerTasks.Docker($"rm {container_name}");
        }
        catch { }
    }
}
