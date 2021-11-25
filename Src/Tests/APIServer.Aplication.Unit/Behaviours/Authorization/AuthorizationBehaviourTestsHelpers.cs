using MediatR;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;
using SharedCore.Aplication.Payload;
using APIServer.Aplication.Shared.Errors;
using SharedCore.Aplication.Core.Commands;
using SharedCore.Aplication.Shared.Attributes;
using APIServer.Aplication.Shared.Behaviours;

namespace APIServer.Application.UnitTests.Behaviours
{
    public class AuthorizationTestCommand : CommandBase<AuthorizationTestPayload>
    {
        public string SomeDataField { get; set; }
    }

    [Authorize]
    public class AuthorizationTestCommandWithAttribute : CommandBase<AuthorizationTestPayload>
    {
        public string SomeDataField { get; set; }
    }

    [Authorize(InnerPolicy = true)]
    public class AuthorizationTestCommandWithInnerAttribute : CommandBase<AuthorizationTestPayload>
    {
        public string SomeDataField { get; set; }
    }

    [Authorize(Roles = "Admin")]
    public class AuthorizationTestCommandWithRole : CommandBase<AuthorizationTestPayload> { }

    [Authorize(Policy = "SomePolicy")]
    public class AuthorizationTestCommandWithPolicy : CommandBase<AuthorizationTestPayload> { }

    public class AuthorizationTestPayload : BasePayload<AuthorizationTestPayload, ITestError>
    {
        public string data { get; set; } = "Some data";
    }

    public class AuthorizationQueryTest : IRequest<AuthorizationQueryTestResponse> { }

    [Authorize]
    public class AuthorizationQueryTestAuhorisedAtribute : IRequest<AuthorizationQueryTestResponse> { }

    public class AuthorizationQueryTestResponse { }

    //---------------------------

    public class AuthorizationTestValidator : AuthorizationValidator<AuthorizationTestCommandWithInnerAttribute>
    {

        public AuthorizationTestValidator()
        {

            RuleFor(e => e.SomeDataField)
            .MustAsync(CanUpdateField);
        }

        public async Task<bool> CanUpdateField(string url, CancellationToken cancellationToken)
        {

            await Task.CompletedTask;

            return false;
        }
    }

    //---------------------------

    public class TestAuthorizationCommandHandler<T> where T : IBasePayload, new()
    {

        public TestAuthorizationCommandHandler() { }

        public async Task<T> Handler()
        {

            await Task.CompletedTask;

            return new T();
        }

    }

    public class TestAuthorizationQueryHandler<T> where T : new()
    {

        public TestAuthorizationQueryHandler() { }

        public async Task<T> Handler()
        {

            await Task.CompletedTask;

            return new T();
        }

    }
}