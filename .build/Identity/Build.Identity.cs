using System;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Docker;
using Nuke.Common.Tools.EntityFramework;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build : NukeBuild
{

    //---------------
    // Enviroment
    //---------------

    AbsolutePath IdentityServerDockerFile => RootDirectory / "Src" / "IdentityServer" / "API" / "Dockerfile";
    AbsolutePath IdentityServerDir => RootDirectory / "Src" / "IdentityServer" / "API";
    AbsolutePath IdentityServerDockerOutput => RootDirectory / "Docker" / ".build" / "IdentityServer";


    //---------------
    // Build process
    //---------------

    Target Identity_Clean => _ => _
        .Before(Identity_Restore)
        .Executes(() =>
        {

        });

    Target Identity_Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution)
            //.DisableProcessLogInvocation()
            // .DisableProcessLogOutput()
            );
        });

    Target Identity_Compile => _ => _
        .DependsOn(Identity_Restore)
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


    Target Identity_Container => _ => _
        .DependsOn(Identity_Compile)
        .Executes(() =>
        {
            var tagName = "damikun/bff:{0}";

            DotNetTasks.DotNetPublish(s => s
            .SetConfiguration("Debug")
            .SetProject(IdentityServerDir));

            DockerTasks.DockerBuild(s => s
                .SetFile(IdentityServerDockerOutput)
                .SetTag(
                    string.Format(tagName, "latest"))
                .SetBuildArg($"version={version}",
                    $"build_date={DateTime.Now:s}")
                .SetPath(IdentityServerDir));
        });

}
