using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;


partial class Build : NukeBuild
{

    //---------------
    // Enviroment
    //---------------

    AbsolutePath API_TestsDirectory => RootDirectory / "Src" / "Tests";
    AbsolutePath API_Unit_Tests_Directory => RootDirectory / "Src" / "Tests" / "APIServer.Aplication.Unit";
    AbsolutePath API_Integration_Tests_Directory => RootDirectory / "Src" / "Tests" / "APIServer.Aplication.Integration";
    AbsolutePath API_API_Tests_Directory => RootDirectory / "Src" / "Tests" / "APIServer.API";


    //---------------
    // Build process
    //---------------

    Target API_Test => _ => _
        .DependsOn(
            API_UnitTest,
            API_IntegrationTest,
            API_APITest
        );

    Target API_UnitTest => _ => _
        .DependsOn(API_Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(API_Unit_Tests_Directory)
                .SetConfiguration(Configuration));
        });

    Target API_IntegrationTest => _ => _
        .DependsOn(API_Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(API_Integration_Tests_Directory)

                // .SetConfiguration(Configuration)
                // .EnableNoRestore()
                // .EnableNoBuild()

                // Alternativaly you can name your aruments manualy
                .SetProcessArgumentConfigurator(arguments => arguments
                .Add($"--configuration {Configuration}")
                .Add("--logger console;verbosity=normal")));
        });

    Target API_APITest => _ => _
        .DependsOn(API_Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(API_API_Tests_Directory)
                .SetConfiguration(Configuration)
                // .EnableNoRestore()
                // .EnableNoBuild()
                );
        });
}
