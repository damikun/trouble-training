using System;
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

    AbsolutePath BFFServerDockerFile => RootDirectory / "Src" / "BFF" / "API" / "Dockerfile";
    AbsolutePath BFFServerDir => RootDirectory / "Src" / "BFF" / "API";
    AbsolutePath BFFServerDockerOutput => RootDirectory / "Docker" / ".build" / "BFF";

    //---------------
    // Build process
    //---------------

    Target BFF_Clean => _ => _
        .Before(BFF_Restore)
        .Executes(() =>
        {
            BFFServerDir.GlobFiles("**yarn.lock")
                .ForEach(DeleteFile);
        });

    Target BFF_Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(BFFServerDir)
            //.DisableProcessLogInvocation()
            // .DisableProcessLogOutput()
            );
        });

    Target BFF_Compile => _ => _
        .DependsOn(BFF_Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(BFFServerDir)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .SetCopyright($"Copyright © DaliborKundrat {DateTime.Now.Year}")
            // .SetOutputDirectory(OutputDirectory)
            );
        });


    Target BFF_DockerBuild => _ => _
        .DependsOn(BFF_Compile)
        .Executes(() =>
        {
            var tagName = "damikun/bff:{0}";

            DotNetTasks.DotNetPublish(s => s
            .SetConfiguration("Debug")
            .SetProject(BFFServerDir));

            DockerTasks.DockerBuild(s => s
                .SetFile(BFFServerDockerFile)
                .SetTag(
                    string.Format(tagName, "latest"))
                .SetBuildArg($"version={version}",
                    $"build_date={DateTime.Now:s}")
                .SetPath(BFFServerDir));
        });


}
