using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Npm;
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Tools.DotNet;
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

    AbsolutePath DeviceDir => RootDirectory / "Src" / "Device" / "API";
    AbsolutePath DeviceFrontend => DeviceDir / "ClientApp";

    //---------------
    // Build process
    //---------------

    Target Device_Clean => _ => _
        .Before(Device_Restore, Clean)
        .Executes(() =>
        {
            DeviceDir.GlobDirectories("**/bin", "**/obj")
                .Where(e => !e.Contains("node_modules"))
                .ForEach(DeleteDirectory);

            DeviceDir.GlobDirectories("**/node_modules")
                .ForEach(DeleteDirectory);

            DeviceDir.GlobFiles("**lock.json")
                .ForEach(DeleteFile);

            DeviceDir.GlobFiles("**yarn.lock")
                .ForEach(DeleteFile);

            Npm("cache clean");
        });

    Target Device_Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(DeviceDir)
                );
        });

    Target Device_Compile => _ => _
        .DependsOn(Device_Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(DeviceDir)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .SetCopyright($"Copyright © DaliborKundrat {DateTime.Now.Year}")
            );
        });

    Target Device_Restore_Frontend => _ => _
        .After(Frontend_Restore)
        .DependsOn(Device_Clean)
        .Executes(() =>
        {
            NpmTasks.NpmInstall(settings =>
                settings
                    .EnableProcessLogOutput()
                    .SetProcessWorkingDirectory(DeviceFrontend)
                    .SetProcessArgumentConfigurator(e => e.Add("--legacy-peer-deps"))
                    );
        });
}
