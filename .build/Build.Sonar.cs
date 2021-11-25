
using Nuke.Common;
using Nuke.Common.Tools.SonarScanner;
using static Nuke.Common.Tools.SonarScanner.SonarScannerTasks;

partial class Build : NukeBuild
{

    //---------------
    // Enviroment
    //---------------

    [Parameter] readonly string SonarToken;
    const string SonarProjectKey = "trouble-training";

    [Parameter("Sonar server")]
    readonly string SonarServer = IsLocalBuild ? "https://sonarcloud.io" : "https://sonarcloud.io"; // change url for local server

    string[] sonar_path_exclude = new string[]{
        "**/Generated/**/*",
        "**/*.generated.cs",
        "**/*.js",
        "**/*.ts",
        "**/*.tsx",
        "**/*.html",
        "**/*.css",
        "**/*.cmd",
        "**/*.ps1",
        "**/*.json",
        "**/*.sh",
        "**/*.md",
        "**/bin/*",
        "**/obj/*",
        "**/Tests/*",
        "**/Test/*",
        "**/node_modules/**/*"
    };

    //---------------
    // Build process
    //---------------

    Target SonarBegin => _ => _
        .Before(
            API_Compile,
            BFF_Compile,
            Identity_Compile
        )
        .Requires(() => SonarToken)
        .Unlisted()
        .Executes(() =>
        {
            SonarScannerBegin(s => s
                .SetLogin(SonarToken)
                .SetProjectKey(SonarProjectKey)
                .SetServer(SonarServer)
                .AddSourceExclusions(sonar_path_exclude)
                .SetOpenCoverPaths(ArtifactsDirectory / "coverage.xml"));
        });

    Target Sonar => _ => _
        .DependsOn(SonarBegin, Backend_All)
        .Requires(() => SonarToken)
        .AssuredAfterFailure()
        .Executes(() =>
        {
            SonarScannerEnd(s => s
                .SetLogin(SonarToken)
                .SetFramework("net5.0"));
        });

}