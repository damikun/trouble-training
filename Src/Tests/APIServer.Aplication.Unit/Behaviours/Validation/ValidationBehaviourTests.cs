using Moq;
using Xunit;
using System;
using Serilog;
using System.Linq;
using System.Threading;
using FluentAssertions;
using FluentValidation;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using SharedCore.Aplication.Interfaces;
using APIServer.Aplication.Shared.Errors;
using APIServer.Aplication.Shared.Behaviours;

namespace APIServer.Application.UnitTests.Behaviours
{
    public class ValidationBehaviourTests
    {
        private readonly Mock<ILogger> _logger;
        private readonly Mock<ICurrentUser> _currentUserService;
        private readonly Mock<ITelemetry> _telemetry;

        public ValidationBehaviourTests()
        {
            _currentUserService = new Mock<ICurrentUser>();

            _logger = new Mock<ILogger>();

            _telemetry = new Mock<ITelemetry>();

            _telemetry.Setup(e => e.AppSource).Returns(new ActivitySource("SomeSource"));

            _telemetry.Setup(e => e.Current).Returns(Activity.Current);

            _telemetry.Setup(e => e.SetOtelError(It.IsAny<Exception>()));

            _telemetry.Setup(e => e.SetOtelError(It.IsAny<string>(), false));

            _telemetry.Setup(e => e.SetOtelWarning(It.IsAny<string>()));
        }

        [Fact]
        public async Task HandleWithoutValidationError()
        {

            var validators = new List<IValidator<ValidationTestCommand>>(){
                 new TestValidatorSingleRule()
            }.AsEnumerable();

            var exBehaviour = new ValidationBehaviour<ValidationTestCommand, ValidationTestPayload>(
                _currentUserService.Object,
                validators,
                _logger.Object,
                _telemetry.Object
            );

            var response = await exBehaviour.Handle(
                new ValidationTestCommand { FirstName = "Zlobor" },
                new CancellationToken(),
                new TestCommandHandler<ValidationTestPayload>().HandleWithoutThrow
            );

            response.Should().NotBeNull();

            response.Should().BeOfType<ValidationTestPayload>();

            response.errors.Any().Should().BeFalse();
        }

        [Fact]
        public async Task HandleSingleValidationError()
        {

            var validators = new List<IValidator<ValidationTestCommand>>(){
                 new TestValidatorSingleRule()
            }.AsEnumerable();

            var exBehaviour = new ValidationBehaviour<ValidationTestCommand, ValidationTestPayload>(
                _currentUserService.Object,
                validators,
                _logger.Object,
                _telemetry.Object
            );

            var response = await exBehaviour.Handle(
                new ValidationTestCommand { FirstName = null },
                new CancellationToken(),
                new TestCommandHandler<ValidationTestPayload>().HandleWithoutThrow
            );

            response.Should().NotBeNull();

            response.Should().BeOfType<ValidationTestPayload>();

            response.errors.Any().Should().BeTrue();

            response.errors.Count().Should()
                .Be(1, "only one validator was defined as: RuleFor(e => e.FirstName).NotNull()");

            response.errors.First().Should().BeOfType<ValidationError>();

            response.errors.First().Should().BeOfType<ValidationError>()
                .Subject.message.Should().Be("Some error message");
        }

        [Fact]
        public async Task HandleMultipleValidationErrors()
        {

            var validators = new List<IValidator<ValidationTestCommand>>(){
                 new TestValidatorMultipleRules()
            }.AsEnumerable();

            var exBehaviour = new ValidationBehaviour<ValidationTestCommand, ValidationTestPayload>(
                _currentUserService.Object,
                validators,
                _logger.Object,
                _telemetry.Object
            );

            var response = await exBehaviour.Handle(
                new ValidationTestCommand { FirstName = null, Age = 18 },
                new CancellationToken(),
                new TestCommandHandler<ValidationTestPayload>().HandleWithoutThrow
            );

            response.Should().NotBeNull();

            response.Should().BeOfType<ValidationTestPayload>();

            response.errors.Any().Should().BeTrue();

            response.errors.Count().Should()
                .BeGreaterThan(1);

            response.errors.First().Should().BeOfType<ValidationError>();
        }

        [Fact]
        public async Task HandleSigleQueryValidationError()
        {
            await Task.CompletedTask;

            var validators = new List<IValidator<ValidationQuery>>(){
                new QuerySingleValidationRule(),
            }.AsEnumerable();

            var exBehaviour = new ValidationBehaviour<ValidationQuery, ValidationQueryResponse>(
                _currentUserService.Object,
                validators,
                _logger.Object,
                _telemetry.Object
            );

            FluentActions.Invoking(() =>
               exBehaviour.Handle(
                new ValidationQuery { some_id = 0 },
                new CancellationToken(),
                new TestQueryHandler<ValidationQueryResponse>().HandleDefaultPayload
            )).Should().Throw<SharedCore.Aplication.Shared.Exceptions.ValidationException>();
        }

        [Fact]
        public async Task HandleSigleQueryWithoutValidationError()
        {

            var validators = new List<IValidator<ValidationQuery>>(){
                new QuerySingleValidationRule(),
            }.AsEnumerable();

            var exBehaviour = new ValidationBehaviour<ValidationQuery, ValidationQueryResponse>(
                _currentUserService.Object,
                validators,
                _logger.Object,
                _telemetry.Object
            );

            var response = await exBehaviour.Handle(
                new ValidationQuery { some_id = 10 },
                new CancellationToken(),
                new TestQueryHandler<ValidationQueryResponse>().HandleDefaultPayload
            );

            response.Should().NotBeNull();

            response.Should().BeOfType<ValidationQueryResponse>();
        }
    }
}