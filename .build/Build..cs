using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.Execution;
using Nuke.Common.ProjectModel;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Tools.GitVersion;

[GitHubActions(
    "backend-restore-build-and-test",
    GitHubActionsImage.WindowsLatest,
    GitHubActionsImage.MacOsLatest,
    InvokedTargets = new[] {
         nameof(Backend_All)
    },
    // On = new[] {
    //     GitHubActionsTrigger.PullRequest,
    //     GitHubActionsTrigger.Push
    // },
    OnPushIncludePaths = new[] { "Src/**" },
    OnPushBranches = new[] { "main" },
    AutoGenerate = true)]
[GitHubActions(
    "frontend-restore-and-build",
    GitHubActionsImage.WindowsLatest,
    GitHubActionsImage.MacOsLatest,
    InvokedTargets = new[] {
         nameof(Frontend_All)
    },
    // On = new[] {
    //      GitHubActionsTrigger.PullRequest,
    //      GitHubActionsTrigger.Push
    // },
    OnPushIncludePaths = new[] { "Src/**" },
    OnPushBranches = new[] { "main" },
    AutoGenerate = false)]
[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
partial class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Backend_Compile);

    //---------------
    // Params and Definitions
    //---------------

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter] readonly bool CI = false;

    [Solution] readonly Solution Solution;

    [GitVersion] readonly GitVersion GitVersion;

    [GitRepository] readonly GitRepository GitRepository;

    //---------------
    // Enviroment
    //---------------

    AbsolutePath SourceDirectory => RootDirectory / "Src";

    //---------------
    // Build process
    //---------------

    protected override void OnBuildInitialized()
    {
        Logger.Info("🚀 Build process started");

        base.OnBuildInitialized();
    }

    Target Backend_All => _ => _
        .DependsOn(
            Backend_Clean,
            Backend_Test,
            Backend_Dockerize
        );

    Target Frontend_All => _ => _
        .DependsOn(
            Frontend_Clean,
            Frontend_TryBuild
        );

    Target All => _ => _
        .DependsOn(
            Backend_All,
            Frontend_All);


    Target E2ETests => _ => _
        .DependsOn(
            Backend_All,
            Frontend_All)
        .Executes(() =>
        {


        });

}
