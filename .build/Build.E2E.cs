using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using System.Net.Http;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Npm;
using System.Threading.Tasks;
using Nuke.Common.Tools.Docker;
using Nuke.Common.Tools.DotNet;

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
    string DBResetEndpoint = "https://localhost:5015/reset";

    AbsolutePath Postman_Dir => RootDirectory / "Src" / "Tests" / "Postman";

    string Postman_Collenction_Cfg_Name = "trouble-training.postman_collection.json";
    string Postman_Enviroment_Cfg_Name = "trouble-training.postman_enviroment.json";

    bool API_Logging = false;
    bool BFF_Logging = false;
    bool Identity_Logging = false;

    string cypress_test_script_name = "test";


#nullable enable
    [PathExecutable("newman")] readonly Tool? Newman;
#nullable disable


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
            InvokedTargets.Any(e => e.Name == nameof(E2E_RunAs_Local) ||
            (InvokedTargets.Any(e => e.Name == nameof(E2E_RunAs_CI) && EnvironmentInfo.IsWin))))
        .DependsOn(
            Start_API_Server,
            Start_Identity_Server,
            Start_BFF_Server,
            Init_Newman)
        .Triggers(
            Stop_API_Server,
            Stop_BFF_Server,
            Stop_Identity_Server)
        .Executes(() =>
            {

                Newman(
                    @$"run {Postman_Collenction_Cfg_Name} -e {Postman_Enviroment_Cfg_Name} --insecure",
                    Postman_Dir,
                    null, // Env variables
                    null, // timeout
                    true  // Log output
                );

                NpmTasks.NpmRun(s => s
                    .SetCommand(cypress_test_script_name)
                    .SetProcessWorkingDirectory(FrontendDirectory)
                );

            }
        );

    //--------------------------------------------

    Target Start_API_Server => _ => _
    .OnlyWhenStatic(() =>
    InvokedTargets.Any(e => e.Name == nameof(E2E_RunAs_Local) ||
    (InvokedTargets.Any(e => e.Name == nameof(E2E_RunAs_CI) && EnvironmentInfo.IsWin))))
    .After(SetupCertificates_Local, SetupCertificates_CI)
    .Executes(async () =>
    {
        APIProcess = ProcessTasks.StartProcess(
             DotNetTasks.DotNetPath,
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
    InvokedTargets.Any(e => e.Name == nameof(E2E_RunAs_Local) ||
    (InvokedTargets.Any(e => e.Name == nameof(E2E_RunAs_CI) && EnvironmentInfo.IsWin))))
    .After(SetupCertificates_Local, SetupCertificates_CI)
    .Executes(async () =>
    {
        BFFProcess = ProcessTasks.StartProcess(
            DotNetTasks.DotNetPath,
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

    Target Init_Newman => _ => _
        .After(Clean)
        .Executes(() =>
        {
            NpmTasks.Npm("install -g newman", Postman_Dir);
        });
    //--------------------------------------------

    Target Start_Identity_Server => _ => _
    .OnlyWhenStatic(() =>
    InvokedTargets.Any(e => e.Name == nameof(E2E_RunAs_Local) ||
    (InvokedTargets.Any(e => e.Name == nameof(E2E_RunAs_CI) && EnvironmentInfo.IsWin))))
    .After(SetupCertificates_Local, SetupCertificates_CI)
    .Executes(async () =>
    {
        IdentityProcess = ProcessTasks.StartProcess(
             DotNetTasks.DotNetPath,
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
                    Logger.Success($"Host: {host_endpoint} is available");
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

        throw new System.Exception($"Host is not available: {host_endpoint}");
    }


}

