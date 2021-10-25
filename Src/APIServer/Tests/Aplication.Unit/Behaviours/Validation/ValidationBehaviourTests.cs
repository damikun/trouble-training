using Moq;
using Xunit;
using Serilog;
using System.Linq;
using System.Threading;
using FluentAssertions;
using FluentValidation;
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

            var resposne = await exBehaviour.Handle(
                new ValidationTestCommand {FirstName="Zlobor" },
                new CancellationToken(),
                new TestCommandHandler<ValidationTestPayload>().HandleWithoutThrow
            );

            resposne.Should().NotBeNull();

            resposne.Should().BeOfType<ValidationTestPayload>();

            resposne.errors.Any().Should().BeFalse();
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

            var resposne = await exBehaviour.Handle(
                new ValidationTestCommand {FirstName=null },
                new CancellationToken(),
                new TestCommandHandler<ValidationTestPayload>().HandleWithoutThrow
            );

            resposne.Should().NotBeNull();

            resposne.Should().BeOfType<ValidationTestPayload>();

            resposne.errors.Any().Should().BeTrue();

            resposne.errors.Count().Should()
                .Be(1,"only one validator was defined as: RuleFor(e => e.FirstName).NotNull()");

            resposne.errors.First().Should().BeOfType<ValidationError>();

            resposne.errors.First().Should().BeOfType<ValidationError>()
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

            var resposne = await exBehaviour.Handle(
                new ValidationTestCommand {FirstName=null, Age=18 },
                new CancellationToken(),
                new TestCommandHandler<ValidationTestPayload>().HandleWithoutThrow
            );

            resposne.Should().NotBeNull();

            resposne.Should().BeOfType<ValidationTestPayload>();

            resposne.errors.Any().Should().BeTrue();

            resposne.errors.Count().Should()
                .BeGreaterThan(1);

            resposne.errors.First().Should().BeOfType<ValidationError>();
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
                new ValidationQuery {some_id=0},
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

            var resposne = await exBehaviour.Handle(
                new ValidationQuery {some_id=10},
                new CancellationToken(),
                new TestQueryHandler<ValidationQueryResponse>().HandleDefaultPayload
            );

            resposne.Should().NotBeNull();

            resposne.Should().BeOfType<ValidationQueryResponse>();
        }
    }
}