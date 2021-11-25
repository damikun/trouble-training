using MediatR;
using FluentValidation;
using System.Threading.Tasks;
using SharedCore.Aplication.Payload;
using APIServer.Aplication.Shared.Errors;
using SharedCore.Aplication.Core.Commands;

namespace APIServer.Application.UnitTests.Behaviours
{
    public class ValidationTestCommand : CommandBase<ValidationTestPayload>
    {
        public string FirstName { get; set; }

        public int Age { get; set; }

        public object obj { get; set; }
    }

    public class ValidationTestPayload : BasePayload<ValidationTestPayload, ITestError>
    {
        public object data { get; set; }
    }

    //---------------------------------

    public class QueryCommand : CommandBase<QueryCommandResponse> { }

    public class QueryCommandResponse
    {

        public string some_field { get; set; } = "Some default value";
    }

    //---------------------------------

    public class TestValidatorNoRules : AbstractValidator<ValidationTestCommand>
    {

        public TestValidatorNoRules() { }
    }

    public class TestValidatorSingleRule : AbstractValidator<ValidationTestCommand>
    {

        public TestValidatorSingleRule()
        {

            RuleFor(e => e.FirstName)
            .NotNull()
            .WithMessage("Some error message");
        }
    }

    public class TestValidatorMultipleRules : AbstractValidator<ValidationTestCommand>
    {

        public TestValidatorMultipleRules()
        {

            RuleFor(e => e.Age)
            .GreaterThan(20);

            RuleFor(e => e.obj)
            .NotNull();
        }
    }

    //---------------------------------

    public class ValidationQuery : IRequest<ValidationQueryResponse>
    {
        public long some_id { get; set; }
    }

    public class ValidationQueryResponse { }

    //---------------------------------


    public class QuerySingleValidationRule : AbstractValidator<ValidationQuery>
    {

        public QuerySingleValidationRule()
        {

            RuleFor(e => e.some_id)
            .GreaterThan(0);
        }
    }

    //---------------------------------

    public class TestCommandHandler<T> where T : IBasePayload, new()
    {

        public TestCommandHandler() { }

        public async Task<T> HandleWithThrow()
        {

            await Task.CompletedTask;

            throw new System.Exception();
        }

        public async Task<T> HandleWithoutThrow()
        {

            await Task.CompletedTask;

            return new T();
        }

    }

    //---------------------------------

    public class TestQueryHandler<T> where T : new()
    {

        public TestQueryHandler() { }

        public async Task<T> HandleDefaultPayload()
        {

            await Task.CompletedTask;

            new T();
            return new T();
        }
    }

}