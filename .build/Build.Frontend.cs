using System;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Npm;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

partial class Build : NukeBuild
{
    //---------------
    // Enviroment
    //---------------

    AbsolutePath FrontendDirectory = RootDirectory / "Src" / "BFF" / "API" / "ClientApp";

    //---------------
    // Build process
    //---------------

    Target Frontend_Clean => _ => _
        .Before(Frontend_Restore)
        .Executes(() =>
        {

            SourceDirectory.GlobDirectories("**/BFF/API/ClientApp/node_modules")
                .ForEach(DeleteDirectory);
        });

    Target Frontend_Restore => _ => _
        .Executes(() =>
        {
            // Ecample of intsall packages based on package.json
            NpmTasks.NpmInstall(settings =>
                settings
                    .EnableProcessLogOutput()
                    .SetProcessWorkingDirectory(FrontendDirectory));
        });

    Target Frontend_AddTailwind => _ => _
        .DependsOn(Frontend_Restore)
        .Executes(() =>
        {
            // Example of dirrcty npm install command
            NpmTasks.Npm(
                "install -D tailwindcss@npm:@tailwindcss/postcss7-compat postcss@^7 autoprefixer@^9",
                FrontendDirectory);
        });

    Target Frontend_RelayCompile => _ => _
        .After(Frontend_Restore)
        .Executes(() =>
        {
            // Compile relay
            NpmTasks.NpmRun(s => s
                .SetCommand("relaycompile_presisted_js")
                .SetProcessWorkingDirectory(FrontendDirectory)
            );
        });

    Target Frontend_TryBuild => _ => _
        .DependsOn(Frontend_AddTailwind, Frontend_RelayCompile)
        .Executes(() =>
        {
            NpmTasks.NpmLogger = CustomLogger;

            NpmTasks.NpmRun(settings =>
                settings
                    .SetCommand("build")
                    .SetProcessWorkingDirectory(FrontendDirectory)
            );
        });

    public static void CustomLogger(OutputType type, string output)
    {
        switch (type)
        {
            case OutputType.Std:
                Logger.Normal(output);
                break;
            case OutputType.Err:
                {
                    if (output.StartsWith("npmWARN", StringComparison.OrdinalIgnoreCase) ||
                        output.StartsWith("npm WARN", StringComparison.OrdinalIgnoreCase) ||
                        output.Contains("npmWARN", StringComparison.OrdinalIgnoreCase) ||
                        output.Contains("npm WARN", StringComparison.OrdinalIgnoreCase))
                        Logger.Normal(output);
                    else
                        Logger.Normal(output);

                    break;
                }
        }
    }

}
