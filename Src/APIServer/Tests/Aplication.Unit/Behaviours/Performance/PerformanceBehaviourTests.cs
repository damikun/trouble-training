using Moq;
using Xunit;
using System;
using Serilog;
using System.Linq;
using System.Threading;
using FluentAssertions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using SharedCore.Aplication.Interfaces;
using APIServer.Aplication.Shared.Behaviours;

namespace APIServer.Application.UnitTests.Behaviours
{
    public class PerformanceBehaviourTests
    {
        private readonly Mock<ILogger> _logger;
        private readonly Mock<ICurrentUser> _currentUserService;
        private readonly Mock<IWebHostEnvironment> _env;
        private readonly Mock<ITelemetry> _telemetry;

        public PerformanceBehaviourTests()
        {
            _currentUserService = new Mock<ICurrentUser>();

            _logger = new Mock<ILogger>();

            _env = new Mock<IWebHostEnvironment>(); 

            _telemetry = new Mock<ITelemetry>();
        }

        [Fact]
        public async Task TestPerfLogProcessed()
        {
            _env.Setup(e=>e.EnvironmentName).Returns("development");

            var exBehaviour = new PerformanceBehaviour<PerformanceTestCommand, PerformanceTestPayload>(
                _currentUserService.Object,
                _logger.Object,
                _env.Object,
                _telemetry.Object
            );

            var command = new PerformanceTestCommand {};

            command.monitor_time = 70;

            var resposne = await exBehaviour.Handle(
                command,
                new CancellationToken(),
                new TestPerformanceCommandHandler<PerformanceTestPayload>().HandleDelayd
            );

            resposne.Should().NotBeNull();

            resposne.Should().BeOfType<PerformanceTestPayload>();

            resposne.errors.Any().Should().BeFalse();

            _logger.Verify(e=>e.Warning(
                String.Format(
                    "Performense values ​​are out of range: Request<{0}>",
                    typeof(PerformanceTestCommand).FullName)
            ),Times.Once);

        }   
    }
}