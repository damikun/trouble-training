using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.EntityFramework;

partial class Build : NukeBuild
{

    //---------------
    // Enviroment
    //---------------

    AbsolutePath APIServerMigrationDir => RootDirectory / "Src" / "APIServer" / "Persistence";

    //---------------
    // Build process
    //---------------

    Target API_Migrate_DB => _ => _
        .DependsOn(
            API_Compile,
            API_Restore,
            Restore_Tools
        // Postgresql_Init // Only in case postgresql is used
        )
        .After(
            All,
            API_All,
            Postgresql_Init)
        .Executes(() =>
        {

            EntityFrameworkTasks
                .EntityFrameworkDatabaseUpdate(e => e
                .SetProcessWorkingDirectory(APIServerMigrationDir)
                .SetProcessToolPath(DotNetTasks.DotNetPath)
                // .SetNoBuild(true)
                );

        });

}
