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

    string frontend_relay_compiler_script_name = "relaycompile_presisted_js";

    string frontend_build_script_name = "build";


#nullable enable
    [PathExecutable("npm")] readonly Tool? Npm;
#nullable disable


    //---------------
    // Build process
    //---------------

    Target Frontend_Clean => _ => _
        .Before(Frontend_Restore)
        .Executes(() =>
        {
            FrontendDirectory.GlobDirectories("**/node_modules")
                .ForEach(DeleteDirectory);

            FrontendDirectory.GlobFiles("**lock.json")
                .ForEach(DeleteFile);

            FrontendDirectory.GlobFiles("**yarn.loc")
                .ForEach(DeleteFile);

            // Npm("cache clean -- force");
        });

    Target Frontend_Restore => _ => _
        .Executes(() =>
        {
            // Ecample of intsall packages based on package.json
            NpmTasks.NpmInstall(settings =>
                settings
                    .EnableProcessLogOutput()
                    .SetProcessWorkingDirectory(FrontendDirectory)
                    .SetProcessArgumentConfigurator(e => e.Add("--legacy-peer-deps"))
                    );
        });

    Target Frontend_AddTailwind => _ => _
        .DependsOn(Frontend_Restore)
        .Executes(() =>
        {
            // Example of dirrcty npm install command
            NpmTasks.Npm(
                "install -D tailwindcss@npm:@tailwindcss/postcss7-compat@2.2.17 postcss@7.0.39 autoprefixer@9.8.8",
                FrontendDirectory);
        });

    Target Frontend_RelayCompile => _ => _
        .After(Frontend_Restore)
        .Executes(() =>
        {
            // Compile relay
            NpmTasks.NpmRun(s => s
                .SetCommand(frontend_relay_compiler_script_name)
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
                    .SetCommand(frontend_build_script_name)
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
                    if (
                        output.Contains(
                            "npmWARN",
                            StringComparison.OrdinalIgnoreCase
                        ) ||

                        output.Contains(
                            "npm WARN",
                            StringComparison.OrdinalIgnoreCase
                        ))

                        Logger.Warn(output);

                    else

                        Logger.Error(output);
                    break;
                }
        }
    }

}
