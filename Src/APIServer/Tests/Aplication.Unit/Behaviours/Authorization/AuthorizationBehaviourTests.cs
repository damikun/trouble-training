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
    public class AuthorizationBehaviourTests
    {
        private readonly Mock<ILogger> _logger;
        private readonly Mock<ICurrentUser> _currentUserService;
        private readonly Mock<ITelemetry> _telemetry;

        public AuthorizationBehaviourTests()
        {
            _currentUserService = new Mock<ICurrentUser>();

            _logger = new Mock<ILogger>();

            _telemetry = new Mock<ITelemetry>();
        }

        [Fact]
        public async Task HandleCommandWithoutAuthorizationAttribute_NoError()
        {            
            var validators = new List<IValidator<AuthorizationTestCommand>>(){

            }.AsEnumerable();

            var exBehaviour = new AuthorizationBehaviour<AuthorizationTestCommand, AuthorizationTestPayload>(
                _currentUserService.Object,
                validators,
                _logger.Object,
                _telemetry.Object
            );

            var resposne = await exBehaviour.Handle(
                new AuthorizationTestCommand {SomeDataField="Zlobor" },
                new CancellationToken(),
                new TestAuthorizationCommandHandler<AuthorizationTestPayload>().Handler
            );

            resposne.Should().NotBeNull();

            resposne.Should().BeOfType<AuthorizationTestPayload>();

            resposne.errors.Any().Should().BeFalse();
        }

        [Fact]
        public async Task HandleCommandWithAuthorizationAttribute_WithError()
        {
            var validators = new List<IValidator<AuthorizationTestCommandWithAttribute>>(){

            }.AsEnumerable();

            var exBehaviour = new AuthorizationBehaviour<AuthorizationTestCommandWithAttribute, AuthorizationTestPayload>(
                _currentUserService.Object,
                validators,
                _logger.Object,
                _telemetry.Object
            );

            var resposne = await exBehaviour.Handle(
                new AuthorizationTestCommandWithAttribute {SomeDataField="Zlobor" },
                new CancellationToken(),
                new TestAuthorizationCommandHandler<AuthorizationTestPayload>().Handler
            );

            _currentUserService.Verify(e=>e.Exist,Times.Once);

            resposne.Should().NotBeNull();

            resposne.Should().BeOfType<AuthorizationTestPayload>();

            resposne.errors.Any().Should().BeTrue();

            resposne.errors.First().Should().BeOfType<UnAuthorised>();
        }

        [Fact]
        public async Task HandleCommandWithAuthorizationAttribute_NoError()
        {
            
            var validators = new List<IValidator<AuthorizationTestCommandWithAttribute>>(){

            }.AsEnumerable();

            _currentUserService.Setup(e=>e.Exist).Returns(true);

            var exBehaviour = new AuthorizationBehaviour<AuthorizationTestCommandWithAttribute, AuthorizationTestPayload>(
                _currentUserService.Object,
                validators,
                _logger.Object,
                _telemetry.Object
            );

            var resposne = await exBehaviour.Handle(
                new AuthorizationTestCommandWithAttribute {SomeDataField="Zlobor" },
                new CancellationToken(),
                new TestAuthorizationCommandHandler<AuthorizationTestPayload>().Handler
            );

            _currentUserService.Verify(e=>e.Exist,Times.Once);

            resposne.Should().NotBeNull();

            resposne.Should().BeOfType<AuthorizationTestPayload>();

            resposne.errors.Any().Should().BeFalse();
        }


        [Fact]
        public async Task HandleCommandWithInnerAuthAtribute_InnerAuthError()
        {
            _currentUserService.Setup(e=>e.Exist).Returns(true);

            var validators = new List<IValidator<AuthorizationTestCommandWithInnerAttribute>>(){
                new AuthorizationTestValidator()
            }.AsEnumerable();

            var exBehaviour = new AuthorizationBehaviour<AuthorizationTestCommandWithInnerAttribute, AuthorizationTestPayload>(
                _currentUserService.Object,
                validators,
                _logger.Object,
                _telemetry.Object
            );

            var resposne = await exBehaviour.Handle(
                new AuthorizationTestCommandWithInnerAttribute {SomeDataField="Zlobor" },
                new CancellationToken(),
                new TestAuthorizationCommandHandler<AuthorizationTestPayload>().Handler
            );

            resposne.Should().NotBeNull();

            resposne.Should().BeOfType<AuthorizationTestPayload>();

            resposne.errors.Any().Should().BeTrue();

            resposne.errors.First().Should().BeOfType<UnAuthorised>();
        } 

        [Fact]
        public async Task  HandleCommandWithRoleAuthAtribute_RoleAuthorisedError()
        {

            var validators = new List<IValidator<AuthorizationTestCommandWithRole>>(){

            }.AsEnumerable();

            _currentUserService.Setup(e=>e.Exist).Returns(true);

            var exBehaviour = new AuthorizationBehaviour<AuthorizationTestCommandWithRole, AuthorizationTestPayload>(
                _currentUserService.Object,
                validators,
                _logger.Object,
                _telemetry.Object
            );

            var resposne = await exBehaviour.Handle(
                new AuthorizationTestCommandWithRole { },
                new CancellationToken(),
                new TestAuthorizationCommandHandler<AuthorizationTestPayload>().Handler
            );

            resposne.Should().NotBeNull();

            resposne.Should().BeOfType<AuthorizationTestPayload>();

            resposne.errors.Any().Should().BeTrue();

            resposne.errors.First().Should().BeOfType<UnAuthorised>();
        }

        [Fact]
        public async Task  HandleCommandWithPolicyAuthAtribute_PolicyAuthorisedError()
        {

            var validators = new List<IValidator<AuthorizationTestCommandWithPolicy>>(){

            }.AsEnumerable();

            _currentUserService.Setup(e=>e.Exist).Returns(true);

            var exBehaviour = new AuthorizationBehaviour<AuthorizationTestCommandWithPolicy, AuthorizationTestPayload>(
                _currentUserService.Object,
                validators,
                _logger.Object,
                _telemetry.Object
            );

            var resposne = await exBehaviour.Handle(
                new AuthorizationTestCommandWithPolicy { },
                new CancellationToken(),
                new TestAuthorizationCommandHandler<AuthorizationTestPayload>().Handler
            );

            resposne.Should().NotBeNull();

            resposne.Should().BeOfType<AuthorizationTestPayload>();

            resposne.errors.Any().Should().BeTrue();

            resposne.errors.First().Should().BeOfType<UnAuthorised>();
        } 

        [Fact]
        public async Task HandleQueryWithoutAtribute_NoError()
        {

            var validators = new List<IValidator<AuthorizationQueryTest>>(){

            }.AsEnumerable();

            _currentUserService.Setup(e=>e.Exist).Returns(true);

            var exBehaviour = new AuthorizationBehaviour<AuthorizationQueryTest, AuthorizationQueryTestResponse>(
                _currentUserService.Object,
                validators,
                _logger.Object,
                _telemetry.Object
            );

            var resposne = await exBehaviour.Handle(
                new AuthorizationQueryTest { },
                new CancellationToken(),
                new TestAuthorizationQueryHandler<AuthorizationQueryTestResponse>().Handler
            );

            resposne.Should().NotBeNull();

            resposne.Should().BeOfType<AuthorizationQueryTestResponse>();
        }  

        [Fact]
        public async Task HandleQueryWithAuthorizeAttribute_NoError()
        {

            var validators = new List<IValidator<AuthorizationQueryTestAuhorisedAtribute>>(){

            }.AsEnumerable();

            _currentUserService.Setup(e=>e.Exist).Returns(true);

            var exBehaviour = new AuthorizationBehaviour<AuthorizationQueryTestAuhorisedAtribute, AuthorizationQueryTestResponse>(
                _currentUserService.Object,
                validators,
                _logger.Object,
                _telemetry.Object
            );

            var resposne = await exBehaviour.Handle(
                new AuthorizationQueryTestAuhorisedAtribute { },
                new CancellationToken(),
                new TestAuthorizationQueryHandler<AuthorizationQueryTestResponse>().Handler
            );

            resposne.Should().NotBeNull();

            resposne.Should().BeOfType<AuthorizationQueryTestResponse>();
        }

        [Fact]
        public async Task HandleQueryWithAuthorizeAttribute_WithAuthExeption()
        {
            await Task.CompletedTask;

            var validators = new List<IValidator<AuthorizationQueryTestAuhorisedAtribute>>(){

            }.AsEnumerable();

            var exBehaviour = new AuthorizationBehaviour<AuthorizationQueryTestAuhorisedAtribute, AuthorizationQueryTestResponse>(
                _currentUserService.Object,
                validators,
                _logger.Object,
                _telemetry.Object
            );

            FluentActions.Invoking(() =>
               exBehaviour.Handle(
                new AuthorizationQueryTestAuhorisedAtribute { },
                new CancellationToken(),
                new TestAuthorizationQueryHandler<AuthorizationQueryTestResponse>().Handler
            )).Should().Throw<SharedCore.Aplication.Shared.Exceptions.AuthorizationException>();
        } 
    }
}