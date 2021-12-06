using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Docker;
using Nuke.Common.Tools.EntityFramework;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build : NukeBuild
{

    //---------------
    // Enviroment
    //---------------

    AbsolutePath OutputDirectory => RootDirectory / "out";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    AbsolutePath APIServerDockerFile => RootDirectory / "Src" / "APIServer" / "API" / "Dockerfile";
    AbsolutePath APIServerDir => RootDirectory / "Src" / "APIServer" / "API";
    AbsolutePath APIServerDockerOutput => RootDirectory / "Docker" / ".build" / "APIServer";

    private readonly string version = "0.0.1"; // just example

    //---------------
    // Build process
    //---------------

    Target API_Clean => _ => _
        .Before(API_Restore)
        .Executes(() =>
        {
            API_TestsDirectory.GlobDirectories("**/bin", "**/obj")
                .Where(e => !e.Contains("node_modules"))
                .ForEach(DeleteDirectory);
        });

    Target API_Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(APIServerDir)
            //.DisableProcessLogInvocation()
            // .DisableProcessLogOutput()
            );
        });

    Target API_Compile => _ => _
        .DependsOn(API_Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(APIServerDir)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .SetCopyright(Copyright)
            // .SetOutputDirectory(OutputDirectory)
            );
        });

    Target Api_DockerBuild => _ => _
        .DependsOn(API_All)
        .Executes(() =>
        {
            var tagName = "damikun/apiserver:{0}";

            DotNetTasks.DotNetPublish(s => s
            .SetConfiguration("Debug")
            .SetProject(APIServerDir));

            DockerTasks.DockerBuild(s => s
                .SetFile(APIServerDockerFile)
                .SetTag(
                    string.Format(tagName, "latest"))
                .SetBuildArg($"version={version}",
                    $"build_date={DateTime.Now:s}")
                .SetPath(APIServerDir));
        });


    /*
    // Pack is only as exemple
    Target API_Pack => _ => _
        .DependsOn(API_Compile)
        .OnlyWhenStatic(() => GitRepository.IsOnMasterBranch())
        .Produces(ArtifactsDirectory / "*.nupkg")
        .Executes(() =>
        {
            DotNetPack(s => s
                .EnableNoBuild()
                .EnableNoCache()
                .SetAuthors("Dalibor Kundrat")
                .EnableNoRestore()
                .SetProject(Solution)
                .SetOutputDirectory(ArtifactsDirectory));
        });
    */
}
