using System;
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

    Target E2E_Test => _ => _
        .After(All)
        .DependsOn(
            SetupCertificatesCI,
            Start_API_Server,
            Start_Identity_Server,
            Start_BFF_Server,
            Init)
        .Triggers(
            Stop_API_Server,
            Stop_BFF_Server,
            Stop_Identity_Server)
        // .OnlyWhenStatic(() => EnvironmentInfo.IsLinux || EnvironmentInfo.IsWin)
        .Executes(() =>
        {

            NpmTasks.NpmRun(s => s
                .SetCommand("test")
                .SetProcessWorkingDirectory(FrontendDirectory)
            );

        }
    );

    //--------------------------------------------

    Target Start_API_Server => _ => _
    .After(SetupCertificates, Init)
    .DependsOn(Init)
    .Executes(async () =>
    {
        APIProcess = ProcessTasks.StartProcess(
            "dotnet",
            "run",
            APIServerDir,
            null,
            null,
            false
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
    .After(SetupCertificates, Init)
    .DependsOn(Init)
    .Executes(async () =>
    {
        BFFProcess = ProcessTasks.StartProcess(
            "dotnet",
            "run",
            BFFServerDir,
            null,
            null,
            true
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
    .After(SetupCertificates, Init)
    .DependsOn(Init)
    .Executes(async () =>
    {
        IdentityProcess = ProcessTasks.StartProcess(
            "dotnet",
            "run",
            IdentityServerDir,
            null,
            null,
            false
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
                // client.BaseAddress = new System.Uri(host_base);

                var response = await client.GetAsync(host_endpoint);

                if (response.IsSuccessStatusCode)
                {
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
