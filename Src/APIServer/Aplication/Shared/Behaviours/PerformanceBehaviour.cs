using System;
using MediatR;
using Serilog;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using SharedCore.Aplication.Interfaces;
using SharedCore.Aplication.Core.Commands;

namespace APIServer.Aplication.Shared.Behaviours
{

    /// <summary>
    /// Performance behaviour for MediatR pipeline
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class PerformanceBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ICurrentUser _currentUserService;
        private readonly ILogger _logger;
        private readonly Stopwatch _timer;
        private readonly IWebHostEnvironment _env;
        private readonly ITelemetry _telemetry;

        private const int DEFAULT_MONITOR = 5000; // 5s

        public PerformanceBehaviour(
            ICurrentUser currentUserService,
            ILogger logger,
            IWebHostEnvironment env,
            ITelemetry telemetry)
        {
            _currentUserService = currentUserService;
            _logger = logger;
            _env = env;
            _timer = new Stopwatch();
            _telemetry = telemetry;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {

            _timer.Start();

            try
            {

                return await next();

            }
            finally
            {
                _timer.Stop();

                var timeInMs = _timer.ElapsedMilliseconds;

                HandleTotalTimeMeasurement(request, timeInMs);
            }
        }

        public void HandleTotalTimeMeasurement<TRequest>(TRequest request, long timeInMs)
        {

            long limit_time = DEFAULT_MONITOR;

            if (SharedCore.Aplication.Shared.Common.IsSubclassOfRawGeneric(
                typeof(CommandBase<>),
                typeof(TRequest))
            )
            {

                ISharedCommandBase I_base_command = request as ISharedCommandBase;

                if (I_base_command.monitor_time.HasValue)
                {
                    if (I_base_command.monitor_time.Value > 50)
                    {
                        limit_time = I_base_command.monitor_time.Value;
                    }
                }
            }

            if (timeInMs > limit_time)
            {

                string message = String.Format(
                        "Performense values ​​are out of range: Request<{0}>",
                        typeof(TRequest).FullName);

                _logger.Warning(message);

                _telemetry.SetOtelWarning(message);
            }
        }
    }
}