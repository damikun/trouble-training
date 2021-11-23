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

    AbsolutePath IdentityMigrationDir => RootDirectory / "Src" / "IdentityServer" / "Persistence";

    //---------------
    // Build process
    //---------------

    Target Identity_Migrate_DB => _ => _
        .DependsOn(
            Identity_Compile,
            Identity_Restore,
            Restore_Tools
        // Postgresql_Init // Only in case postgres is used
        )
        .After(
            All,
            Identity_All,
            Postgresql_Init
        )
        .Executes(() =>
        {
            EntityFrameworkTasks
                .EntityFrameworkDatabaseUpdate(e => e
                .SetProcessWorkingDirectory(IdentityMigrationDir)
                    .SetContext("AppConfigurationDbContext")
                // .SetNoBuild(true)
                );

            EntityFrameworkTasks
                .EntityFrameworkDatabaseUpdate(e => e
                    .SetProcessWorkingDirectory(IdentityMigrationDir)
                    .SetContext("AppPersistedGrantDbContext")
                // .SetNoBuild(true)
                );

            EntityFrameworkTasks
                .EntityFrameworkDatabaseUpdate(e => e
                .SetProcessWorkingDirectory(IdentityMigrationDir)
                    .SetContext("AppIdnetityDbContext")
                // .SetNoBuild(true)
                );
        });

}
