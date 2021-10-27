using Moq;
using Xunit;
using System;
using Serilog;
using System.Linq;
using System.Threading;
using FluentAssertions;
using System.Diagnostics;
using System.Threading.Tasks;
using SharedCore.Aplication.Interfaces;
using APIServer.Aplication.Shared.Errors;
using APIServer.Aplication.Shared.Behaviours;

namespace APIServer.Application.UnitTests.Behaviours
{
    public class ExceptionBehaviourTests
    {
        private readonly Mock<ILogger> _logger;
        private readonly Mock<ICurrentUser> _currentUserService;
        private readonly Mock<ITelemetry> _telemetry;

        public ExceptionBehaviourTests()
        {
            _currentUserService = new Mock<ICurrentUser>();

            _logger = new Mock<ILogger>();

            _telemetry = new Mock<ITelemetry>();

            _telemetry.Setup(e=>e.AppSource).Returns(new ActivitySource("SomeSource"));
        }

        [Fact]
        public async Task HandleThrowAsCommandInternallError()
        {

            var exBehaviour = new UnhandledExBehaviour<ExceptionTestCommand, ExceptionTestPayload>(
                _currentUserService.Object,
                _logger.Object,
                _telemetry.Object
            );

            var response = await exBehaviour.Handle(
                new ExceptionTestCommand { },
                new CancellationToken(),
                new ExceptionTestCommandHandler<ExceptionTestPayload>().HandleWithThrow
            );

            response.Should().NotBeNull();

            response.Should().BeOfType<ExceptionTestPayload>();

            response.errors.Any().Should().BeTrue();

            response.errors.First().Should().BeOfType<InternalServerError>();

            _telemetry.Verify(e=>e.SetOtelError(It.IsAny<Exception>()),Times.Once);
        }

        [Fact]
        public async Task ProcessCommandWithoutThrowing()
        {

            var exBehaviour = new UnhandledExBehaviour<ExceptionTestCommand, ExceptionTestPayload>(
                _currentUserService.Object,
                _logger.Object,
                _telemetry.Object
            );

            var response = await exBehaviour.Handle(
                new ExceptionTestCommand { },
                new CancellationToken(),
                new ExceptionTestCommandHandler<ExceptionTestPayload>().HandleWithoutThrow
            );

            response.Should().NotBeNull();

            response.Should().BeOfType<ExceptionTestPayload>();

            response.errors.Any().Should().BeFalse();

            _telemetry.Verify(e=>e.SetOtelError(It.IsAny<Exception>()),Times.Never);
        }

        [Fact]
        public async Task ThrowUndefinedErrorMarkerInterface()
        {
            await Task.CompletedTask;
            
            var exBehaviour = new UnhandledExBehaviour<ExceptionUnknownCommand, ExceptionUnknownCommandPayload>(
                _currentUserService.Object,
                _logger.Object,
                _telemetry.Object
            );

            FluentActions.Invoking(() =>
                exBehaviour.Handle(
                new ExceptionUnknownCommand { },
                new CancellationToken(),
                new ExceptionTestCommandHandler<ExceptionUnknownCommandPayload>().HandleWithUnknownPayloadError
            )).Should().Throw<NotSupportedException>();

            _telemetry.Verify(e=>e.SetOtelError(It.IsAny<Exception>()),Times.Once);
        }

        [Fact]
        public async Task ThrowQueryException()
        {
            await Task.CompletedTask;
            
            var exBehaviour = new UnhandledExBehaviour<ExceptionQuery, ExceptionQueryResponse>(
                _currentUserService.Object,
                _logger.Object,
                _telemetry.Object
            );

            FluentActions.Invoking(() =>
                exBehaviour.Handle(
                new ExceptionQuery { },
                new CancellationToken(),
                new ExceptionTestQueryHandler<ExceptionQueryResponse>().HandleWithThrow
            )).Should().Throw<System.Exception>();

            _telemetry.Verify(e=>e.SetOtelError(It.IsAny<Exception>()),Times.Once);
        }

        [Fact]
        public async Task ProcessQueryWithoutThrow()
        {
            await Task.CompletedTask;
            
            var exBehaviour = new UnhandledExBehaviour<ExceptionQuery, ExceptionQueryResponse>(
                _currentUserService.Object,
                _logger.Object,
                _telemetry.Object
            );

            FluentActions.Invoking(() =>
                exBehaviour.Handle(
                new ExceptionQuery { },
                new CancellationToken(),
                new ExceptionTestQueryHandler<ExceptionQueryResponse>().HandleWithoutThrow
            )).Should().NotThrow<System.Exception>();

            _telemetry.Verify(e=>e.SetOtelError(It.IsAny<Exception>()),Times.Never);
        }
    }
}