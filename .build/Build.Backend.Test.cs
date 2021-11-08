using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build : NukeBuild
{

    //---------------
    // Enviroment
    //---------------

    AbsolutePath Backend_TestsDirectory => RootDirectory / "Src" / "Tests";
    AbsolutePath Backend_Unit_Tests_Directory => RootDirectory / "Src" / "Tests" / "APIServer.Aplication.Unit";
    AbsolutePath Backend_Integration_Tests_Directory => RootDirectory / "Src" / "Tests" / "APIServer.Aplication.Integration";

    //---------------
    // Build process
    //---------------

    Target Backend_Test => _ => _
        .DependsOn(Backend_UnitTest, Backend_IntegrationTest);


    Target Backend_UnitTest => _ => _
        .DependsOn(Backend_Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Backend_Unit_Tests_Directory)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .EnableNoBuild());
        });

    Target Backend_IntegrationTest => _ => _
        .DependsOn(Backend_Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Backend_Integration_Tests_Directory)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .EnableNoBuild());
        });
}
