using System;
using System.Linq;
using Nuke.Common;
using System.Net.Http;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Npm;
using System.Threading.Tasks;
using Nuke.Common.Tools.Docker;

partial class Build : NukeBuild
{

    //---------------
    // Enviroment
    //---------------

    IProcess APIProcess;
    IProcess BFFProcess;
    IProcess IdentityProcess;

    string APIProcess_Health_Url = "https://localhost:5022/health";
    string BFFProcess_Health_Url = "https://localhost:5015/health";
    string IdentityProcess_Health_Url = "https://localhost:5001/health";

    bool API_Logging = false;
    bool BFF_Logging = false;
    bool Identity_Logging = false;

    string cypress_test_script_name = "test";


    //---------------
    // Build process
    //---------------

    protected override void OnBuildFinished()
    {
        KillProcess(APIProcess);
        KillProcess(BFFProcess);
        KillProcess(IdentityProcess);

        base.OnBuildFinished();
    }

    Target E2E_RunAs_CI => _ => _
        .OnlyWhenStatic(() => EnvironmentInfo.IsWin)
        .After(All)
        .DependsOn(SetupCertificates_CI, Init)
        .Executes(E2E_Test);

    Target E2E_RunAs_Local => _ => _
        .After(All)
        .DependsOn(SetupCertificates_Local, Init)
        .Executes(E2E_Test);

    Target E2E_Test => _ => _
        .OnlyWhenStatic(() =>
            this.InvokedTargets.Any(e => e.Name == nameof(E2E_RunAs_Local) ||
            (this.InvokedTargets.Any(e => e.Name == nameof(E2E_RunAs_CI) && EnvironmentInfo.IsWin))))
        .DependsOn(
            Start_API_Server,
            Start_Identity_Server,
            Start_BFF_Server)
        .Triggers(
            Stop_API_Server,
            Stop_BFF_Server,
            Stop_Identity_Server)

        .Executes(() =>
            {

                NpmTasks.NpmRun(s => s
                    .SetCommand(cypress_test_script_name)
                    .SetProcessWorkingDirectory(FrontendDirectory)
                );

            }
        );

    //--------------------------------------------

    Target Start_API_Server => _ => _
    .OnlyWhenStatic(() =>
    this.InvokedTargets.Any(e => e.Name == nameof(E2E_RunAs_Local) ||
    (this.InvokedTargets.Any(e => e.Name == nameof(E2E_RunAs_CI) && EnvironmentInfo.IsWin))))
    .After(SetupCertificates_Local, SetupCertificates_CI)
    .Executes(async () =>
    {
        APIProcess = ProcessTasks.StartProcess(
            "dotnet",
            "run",
            APIServerDir,
            null,           // env variables
            null,           // Timeout 
            API_Logging     // logs
        );

        await WaitForHost(APIProcess_Health_Url);
    });

    Target Stop_API_Server => _ => _
    .Executes(() =>
    {
        KillProcess(APIProcess);
    });

    //--------------------------------------------

    Target Start_BFF_Server => _ => _
    .OnlyWhenStatic(() =>
    this.InvokedTargets.Any(e => e.Name == nameof(E2E_RunAs_Local) ||
    (this.InvokedTargets.Any(e => e.Name == nameof(E2E_RunAs_CI) && EnvironmentInfo.IsWin))))
    .After(SetupCertificates_Local, SetupCertificates_CI)
    .Executes(async () =>
    {
        BFFProcess = ProcessTasks.StartProcess(
            "dotnet",
            "run",
            BFFServerDir,
            null,           // env variables
            null,           // Timeout 
            BFF_Logging     // logs
        );

        await WaitForHost(BFFProcess_Health_Url);

        Logger.Info("⏰ Waiting for Frontend to be ready");
        await Task.Delay(10000);
    });

    Target Stop_BFF_Server => _ => _
    .Executes(() =>
    {
        KillProcess(BFFProcess);
    });

    //--------------------------------------------

    Target Start_Identity_Server => _ => _
    .OnlyWhenStatic(() =>
    this.InvokedTargets.Any(e => e.Name == nameof(E2E_RunAs_Local) ||
    (this.InvokedTargets.Any(e => e.Name == nameof(E2E_RunAs_CI) && EnvironmentInfo.IsWin))))
    .After(SetupCertificates_Local, SetupCertificates_CI)
    .Executes(async () =>
    {
        IdentityProcess = ProcessTasks.StartProcess(
            "dotnet",
            "run",
            IdentityServerDir,
            null,               // env variables
            null,               // Timeout 
            Identity_Logging    // logs
        );

        await WaitForHost(IdentityProcess_Health_Url);
    });

    Target Stop_Identity_Server => _ => _
    .Executes(() =>
    {
        KillProcess(IdentityProcess);
    });


    //--------------------------------------------

    public static void KillProcess(IProcess process)
    {

        if (process == null)
        {
            return;
        }

        Logger.Info($"🛑 Stopping server {nameof(process)}");

        try
        {
            process.Kill();
        }
        catch { }

        process = null;
    }


    public async static Task WaitForHost(string host_endpoint)
    {

        var handler = new HttpClientHandler()
        {
            ServerCertificateCustomValidationCallback = delegate { return true; },
        };

        var client = new HttpClient(handler);
        int retry = 15;

        await Task.Delay(500);

        for (int i = 1; i <= retry; i++)
        {
            try
            {
                var response = await client.GetAsync(host_endpoint);

                if (response.IsSuccessStatusCode)
                {
                    Logger.Success($"Host: {host_endpoint} is awailable");
                    return;
                }
                else
                {

                    if (i == retry)
                    {
                        Logger.Error($"Responded with {response.StatusCode}");
                    }
                    continue;
                }
            }
            catch (Exception ex)
            {
                if (i == retry)
                {
                    Logger.Error(ex.ToString());
                }

            }
            finally
            {
                await Task.Delay(500 * retry);
            }

        }

        throw new System.Exception($"Host is not awailable: {host_endpoint}");
    }

}
