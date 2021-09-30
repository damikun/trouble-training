using System;
using MediatR;
using Serilog;
using System.Threading;
using System.Threading.Tasks;
using SharedCore.Aplication.Interfaces;
using System.Diagnostics;
using SharedCore.Aplication.Core.Commands;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace APIServer.Aplication.Shared.Behaviours {

    /// <summary>
    /// Performance behaviour for MediatR pipeline
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class CommandPerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> {
        private readonly ICurrentUser _currentUserService;
        private readonly ILogger _logger;
        private readonly Stopwatch _timer;
        private readonly IWebHostEnvironment _env;
        
        private const int COMMAND_PROD_TIME_LIMIT = 1000; // 1s
        private const int COMMAND_DEV_TIME_LIMIT = 5000; //3s
        private const int COMMAND_LONG_RUN_LIMIT = 60000; // 1min

        public CommandPerformanceBehaviour(
            ICurrentUser currentUserService,
            ILogger logger,
            IWebHostEnvironment env) {
            _currentUserService = currentUserService;
            _logger = logger;
            _env = env;
            _timer = new Stopwatch();
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next) {

            _timer.Start();
            
            // Continue in pipe
            var response = await next();

            _timer.Stop();

            var timeInMs = _timer.ElapsedMilliseconds;

            HandleTotalTimeMeasurement(request,timeInMs);

            return response;
        }

        public void HandleTotalTimeMeasurement<TRequest>(TRequest request, long timeInMs){

            long limit_time = COMMAND_PROD_TIME_LIMIT;

            if (_env.IsDevelopment()) {
                limit_time = COMMAND_DEV_TIME_LIMIT;
            }

            if (typeof(TRequest).IsSubclassOf(typeof(CommandBase))) {

                ISharedCommandBase I_base_command = request as ISharedCommandBase;

                if(I_base_command.Flags.long_running){
                    limit_time = COMMAND_LONG_RUN_LIMIT;
                }
            }

            if (timeInMs > limit_time){
                _logger.Warning(String.Format("Performense values ​​are out of range: Request<{0}>", typeof(TRequest).FullName));
            }
        }

    }
}