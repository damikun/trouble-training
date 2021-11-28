using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.Execution;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using Nuke.Common.Tooling;

[GitHubActions(
    "Backend-Frontend-Testing",
    GitHubActionsImage.WindowsLatest,
    GitHubActionsImage.MacOsLatest,
    InvokedTargets = new[] {
        nameof(E2E_RunAs_CI),
        nameof(All)
    },
    OnPushIncludePaths = new[] {
        "Src/**",
        ".build/**"
    },
    OnPushBranches = new[] { "main" },
    AutoGenerate = false)]
// [GitHubActions(
//     "frontend-restore-and-build",
//     GitHubActionsImage.WindowsLatest,
//     GitHubActionsImage.MacOsLatest,
//     InvokedTargets = new[] {
//          nameof(Frontend_All)
//     },
//     // On = new[] {
//     //      GitHubActionsTrigger.PullRequest,
//     //      GitHubActionsTrigger.Push
//     // },
//     OnPushIncludePaths = new[] {
//         "Src/**",
//         ".build/**"
//     },
//     OnPushBranches = new[] { "main" },
//     AutoGenerate = false)]
[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
partial class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.API_Compile);

    //---------------
    // Params and Definitions
    //---------------

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = true ? Configuration.Debug : Configuration.Release;

    [Parameter] readonly bool CI = false;

    [Solution] readonly Solution Solution;

    [GitVersion] readonly GitVersion GitVersion;

    [GitRepository] readonly GitRepository GitRepository;

    [PathExecutable] readonly Tool? PowerShell;

    //---------------
    // Enviroment
    //---------------

    AbsolutePath SourceDirectory => RootDirectory / "Src";

    AbsolutePath APIServer_Project => RootDirectory / "Src" / "APIServer" / "API";

    AbsolutePath BFF_Project => RootDirectory / "Src" / "BFF" / "API";

    AbsolutePath Identity_Project => RootDirectory / "Src" / "IdentityServer" / "API";

    AbsolutePath Build_Root => RootDirectory / ".build";
    AbsolutePath CI_Cert_Script => RootDirectory / ".build" / "ci_certs.ps1";

    //---------------
    // Build process
    //---------------

    protected override void OnBuildInitialized()
    {
        Logger.Info("🚀 Build process started");

        base.OnBuildInitialized();
    }


    //----------------------------
    //----------------------------

    Target Clean => _ => _
        .DependsOn(API_Clean, BFF_Clean, Identity_Clean, Frontend_Clean)
        .Before(Identity_Restore, API_Restore, BFF_Restore, Frontend_Restore)
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
        });

    Target API_All => _ => _
        .DependsOn(
            Clean,
            API_Test
        );

    Target BFF_All => _ => _
        .DependsOn(
            Clean,
            BFF_Compile
        );

    Target Identity_All => _ => _
        .DependsOn(
            Clean,
            Identity_Compile
        );

    Target Frontend_All => _ => _
        .DependsOn(
            Frontend_Clean,
            Frontend_TryBuild
        );

    //----------------------------
    //----------------------------

    Target All => _ => _
        .DependsOn(
            API_All,
            BFF_All,
            Identity_All,
            Frontend_All,
            Restore_Tools
        );


    //----------------------------
    //----------------------------

    Target TrustDevCertificates_CI => _ => _
        .OnlyWhenStatic(() => EnvironmentInfo.IsWin)
        .Executes(() =>
        {
            string homePath = (Environment.OSVersion.Platform == PlatformID.Unix ||
                Environment.OSVersion.Platform == PlatformID.MacOSX)
                ? Environment.GetEnvironmentVariable("HOME")
                : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");

            PowerShell(
                $"-NoProfile -ExecutionPolicy Unrestricted -File \"{CI_Cert_Script}\" {homePath}/.aspnet/https/cert.pfx dk@pass -Verb RunAs");
        });

    Target TrustDevCertificates_Local => _ => _
        .Executes(() =>
        {
            DotNetTasks.DotNet("dev-certs https -t");
        });

    Target Clean_Dev_Certificates => _ => _
        .Executes(() =>
        {
            DotNetTasks.DotNet("dev-certs https --clean");
        });


    Target SetupCertificates_Local => _ => _
        .DependsOn(TrustDevCertificates_Local, SetupDevCertificates, Clean_Dev_Certificates);

    Target SetupCertificates_CI => _ => _
        .OnlyWhenStatic(() => EnvironmentInfo.IsWin)
        .DependsOn(TrustDevCertificates_CI, SetupDevCertificates, Clean_Dev_Certificates);

    Target SetupDevCertificates => _ => _
        .Before(TrustDevCertificates_CI, TrustDevCertificates_Local)
        .After(Clean_Dev_Certificates)
        .Executes(() =>
        {
            string pass = "dk@pass";

            string homePath = (Environment.OSVersion.Platform == PlatformID.Unix ||
                            Environment.OSVersion.Platform == PlatformID.MacOSX)
                ? Environment.GetEnvironmentVariable("HOME")
                : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");

            if (EnvironmentInfo.IsWin)
            {
                DotNetTasks.DotNet($"dev-certs https -ep {homePath}/.aspnet/https/cert.pfx -p {pass}");
            }
            else if (EnvironmentInfo.IsLinux || EnvironmentInfo.IsOsx)
            {
                DotNetTasks.DotNet($"dev-certs https -v -ep ~/.aspnet/https/cert.pfx -p {pass}");
            }
            else
            {
                throw new Exception("Unsuported OS. Cetificates cannot be generates!");
            }

            var projects = new AbsolutePath[] { APIServer_Project, BFF_Project, Identity_Project };

            foreach (var proj in projects)
            {
                try
                {
                    DotNetTasks.DotNet($"user-secrets remove \"Kestrel:Certificates:Default:Password\"", proj);
                }
                catch { }
                try
                {
                    DotNetTasks.DotNet($"user-secrets remove \"Kestrel:Certificates:Default:Path", proj);
                }
                catch { }

                DotNetTasks.DotNet($"user-secrets set \"Kestrel:Certificates:Default:Password\" \"{pass}\"", proj);

                if (EnvironmentInfo.IsWin)
                {
                    DotNetTasks.DotNet($"user-secrets set \"Kestrel:Certificates:Default:Path\" \"{homePath}/.aspnet/https/cert.pfx\"", proj);
                }
                else if (EnvironmentInfo.IsLinux || EnvironmentInfo.IsOsx)
                {
                    DotNetTasks.DotNet($"dev-certs https -v -ep ~/.aspnet/https/cert.pfx -p {pass}");
                }
                else
                {
                    throw new Exception("Unsuported OS. Cetificates cannot be generates!");
                }

            }

        });
}
