using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Git;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Docker;
using Nuke.Common.Tools.GitVersion;
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
    AbsolutePath APIServerDockerFile => RootDirectory / "Src" / "APIServer" / "API" / "DockerFile";
    AbsolutePath APIServerDockerOutput => RootDirectory / "Docker" / ".build" / "APIServer";

    //---------------
    // Build process
    //---------------

    Target Backend_Clean => _ => _
        .Before(Backend_Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj")
                .Where(e => !e.ToString().Contains(
                    "node_modules",
                    StringComparison.OrdinalIgnoreCase))
                .Where(e => !e.ToString().Contains(
                    "ClientApp",
                    StringComparison.OrdinalIgnoreCase))
                .ForEach(DeleteDirectory);

            Backend_TestsDirectory.GlobDirectories("**/bin", "**/obj")
                .Where(e => !e.Contains("node_modules"))
                .ForEach(DeleteDirectory);
        });

    Target Backend_Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution)
            //.DisableProcessLogInvocation()
            // .DisableProcessLogOutput()
            );
        });

    Target Backend_Compile => _ => _
        .DependsOn(Backend_Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .SetCopyright($"Copyright © DaliborKundrat {DateTime.Now.Year}")
            // .SetOutputDirectory(OutputDirectory)
            );
        });

    Target Backend_Dockerize => _ => _
        .After(Backend_Compile)
        .Executes(() =>
        {

            // DockerTasks.DockerVersion();


        });

    // Pack is only as exemple
    Target Backend_Pack => _ => _
        .DependsOn(Backend_Compile)
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

}
